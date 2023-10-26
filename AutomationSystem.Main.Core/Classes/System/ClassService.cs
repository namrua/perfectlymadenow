using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.Factories;
using AutomationSystem.Main.Core.Classes.AppLogic.Models.Events;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Classes.System.Model;
using AutomationSystem.Main.Core.PriceLists.Data;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.EntityIntegration.System;
using AutomationSystem.Shared.Contract.Files.System;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationSystem.Main.Core.Classes.System
{
    public class ClassService : IClassService
    {
        public const string HeaderPictureNamePattern = "Header picture ({0})";

        private readonly IClassConvertor classConvertor;
        private readonly IClassBusinessConvertor classBusinessConvertor;
        private readonly IClassStyleConvertor classStyleConvertor;
        private readonly IIdentityResolver identityResolver;
        private readonly IMainMapper mainMapper;
        private readonly ICoreFileService coreFileService;
        private readonly IClassDatabaseLayer classDb;
        private readonly IGenericEntityIntegrationProvider integrationProvider;
        private readonly IEventDispatcher eventDispatcher;
        private readonly IPriceListDatabaseLayer priceListDb;
        private readonly IProfileDatabaseLayer profileDb;
        private readonly IClassEventFactory classEventFactory;
        private readonly IClassTypeResolver classTypeResolver;

        public ClassService(
            IClassConvertor classConvertor,
            IClassBusinessConvertor classBusinessConvertor,
            IClassStyleConvertor classStyleConvertor,
            IIdentityResolver identityResolver,
            IMainMapper mainMapper,
            ICoreFileService coreFileService,
            IClassDatabaseLayer classDb,
            IGenericEntityIntegrationProvider integrationProvider,
            IEventDispatcher eventDispatcher, 
            IPriceListDatabaseLayer priceListDb,
            IProfileDatabaseLayer profileDb,
            IClassEventFactory classEventFactory,
            IClassTypeResolver classTypeResolver)
        {
            this.classConvertor = classConvertor;
            this.classBusinessConvertor = classBusinessConvertor;
            this.classStyleConvertor = classStyleConvertor;
            this.identityResolver = identityResolver;
            this.mainMapper = mainMapper;
            this.coreFileService = coreFileService;
            this.classDb = classDb;
            this.integrationProvider = integrationProvider;
            this.eventDispatcher = eventDispatcher;
            this.priceListDb = priceListDb;
            this.profileDb = profileDb;
            this.classEventFactory = classEventFactory;
            this.classTypeResolver = classTypeResolver;
        }

        public ClassForm GetNewClassForm(long profileId, ClassCategoryEnum classCategoryId, CurrencyEnum currencyId, IPersonHelper personHelper)
        {
            var formConfiguration = classTypeResolver.GetClassFormConfigurationByClassCategoryId(classCategoryId);

            var result = new ClassForm
            {
                ProfileId = profileId,
                ClassCategoryId = classCategoryId,
                ProfileCurrencyId = currencyId,
                ClassState = ClassState.New,
                TimeZoneId = formConfiguration.ShowOnlyDates
                    ? TimeZoneEnum.UTC
                    : TimeZoneEnum.HawaiianStandardTime,
                CoordinatorId = personHelper.GetDefaultPersonId(
                    formConfiguration.UseDistanceCoordinator ? PersonRoleTypeEnum.DistanceDoordinator : PersonRoleTypeEnum.Coordinator),
                GuestInstructorId = personHelper.GetDefaultPersonId(PersonRoleTypeEnum.GuestInstructor),
                InstructorIds = personHelper.GetDefaultPersonIds(PersonRoleTypeEnum.Instructor),
                IsWwaFormAllowed = formConfiguration.IsWwaAllowedValue ?? false
            };

            if (formConfiguration.ShowApprovedStaffIds)
            {
                result.ApprovedStaffIds = personHelper.GetDefaultPersonIds(PersonRoleTypeEnum.ApprovedStaff);
            }

            if (!formConfiguration.ShowIntegrationCode)
            {
                result.IntegrationCode = classConvertor.GetNoIntegrationCode();
            }

            if (!formConfiguration.ShowPayPalKey)
            {
                result.PayPalKeyId = ClassConvertor.NoPaymentId;
            }

            return result;
        }

        public List<ClassWithClassForm> GetClassesWithClassFormsByIds(List<long> classIds)
        {
            var classes = classDb.GetClassesByIds(classIds, ClassIncludes.ClassPersons);
            var result = classes.Select(x => new ClassWithClassForm(x, classConvertor.ConvertToClassForm(x))).ToList();
            return result;
        }

        public long InsertClass(ClassForm form, EnvironmentTypeEnum? env)
        {
            var currencyId = GetPriceListCurrencyId(form.PriceListId);

            var profile = profileDb.GetProfileById(form.ProfileId, ProfileIncludes.ClassPreferenceClassPreferenceExpenses);
            if (profile == null)
            {
                throw new ArgumentException($"There is no Profile with Id {form.ProfileId}.");
            }

            var classId = InsertClass(form, currencyId, profile.ClassPreference, env);
            return classId;
        }

        public long InsertClass(ClassForm form, CurrencyEnum currencyId, ClassPreference classPreference, EnvironmentTypeEnum? env)
        {
            var dbClass = classConvertor.ConvertToClass(form);

            // sets defaults and preferences of class
            dbClass.CurrencyId = currencyId;
            dbClass.OwnerId = identityResolver.GetOwnerId();
            dbClass.EnvironmentTypeId = env ?? EnvironmentTypeEnum.Production;
            dbClass.ClassBusiness = classBusinessConvertor.CreateClassBusinessByClassPreference(dbClass, classPreference);
            dbClass.ClassReportSetting = mainMapper.Map<ClassReportSetting>(classPreference);
            dbClass.ClassStyle = classStyleConvertor.CreateClassStyleByClassPreference(classPreference, classTypeResolver.ShowClassBehaviorSettings(form.ClassCategoryId));

            // clones header picture if any
            var headerPictureId = dbClass.ClassStyle.HeaderPictureId;
            if (headerPictureId.HasValue)
            {
                var newHeaderPictureId = coreFileService.CloneFile(headerPictureId.Value, string.Format(HeaderPictureNamePattern, "new-class"));
                dbClass.ClassStyle.HeaderPictureId = newHeaderPictureId;
            }

            // inserts class into database
            var classId = classDb.InsertClass(dbClass);
            integrationProvider.AttachEntity(EntityTypeEnum.MainClass, classId, dbClass.IntegrationTypeId, dbClass.IntegrationEntityId, false);

            // dispatch class created event
            eventDispatcher.Dispatch(new ClassCreatedEvent(classId));

            return classId;
        }

        public void UpdateClass(ClassForm form, Class originClass, bool isFullyEditable)
        {
            var currencyId = GetPriceListCurrencyId(form.PriceListId);
            UpdateClass(form, currencyId, originClass, isFullyEditable);
        }

        public void UpdateClass(ClassForm form, CurrencyEnum currencyId, Class originClass, bool isFullyEditable)
        {
            // updates class
            var dbClass = classConvertor.ConvertToClass(form);
            dbClass.CurrencyId = currencyId;
            classDb.UpdateClass(dbClass, isFullyEditable);

            // reattach entity if needed
            if (isFullyEditable)
            {
                integrationProvider.AttachEntity(EntityTypeEnum.MainClass, form.ClassId, dbClass.IntegrationTypeId, dbClass.IntegrationEntityId, true);
            }

            // dispatch class person changed event
            var classPersonChangedEvent = classEventFactory.CreateClassPersonsChangedEventWhenChanged(originClass, dbClass);
            if (classPersonChangedEvent != null)
            {
                eventDispatcher.Dispatch(classPersonChangedEvent);
            }
        }

        #region private methods

        private CurrencyEnum GetPriceListCurrencyId(long? priceListId)
        {
            var priceList = priceListDb.GetPriceListById(priceListId ?? 0);
            if (priceList == null)
            {
                throw new ArgumentException($"There is no PriceList with Id {priceListId}.");
            }

            var currencyId = priceList.CurrencyId;
            return currencyId;
        }

        #endregion

    }
}
