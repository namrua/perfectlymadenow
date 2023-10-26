using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Persons.AppLogic;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.PriceLists.Data;
using AutomationSystem.Main.Core.PriceLists.System;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.EntityIntegration.System;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Payment.Data;
using AutomationSystem.Shared.Contract.Payment.Data.Model;
using AutomationSystem.Shared.Contract.TimeZones.System;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Core.Classes.System.Convertors
{
    /// <summary>
    /// Converts class objects
    /// </summary>
    public class ClassConvertor : IClassConvertor
    {
        private const string NoPaymentText = "No payment";
        public const long NoPaymentId = -1;

        // private components
        private readonly IEnumDatabaseLayer enumDb;
        private readonly IPersonDatabaseLayer personDb;
        private readonly IPriceListDatabaseLayer priceListDb;
        private readonly IPaymentDatabaseLayer paymentDb;
        private readonly IGenericEntityIntegrationProvider integrationProvider;
        private readonly ITimeZoneService timeZoneService;
        private readonly ILanguageTranslationProvider languageProvider;
        private readonly IPriceListTypeResolver priceListTypeResolver;
        private readonly IClassTypeResolver classTypeResolver;


        // constructor
        public ClassConvertor(
            IEnumDatabaseLayer enumDb,
            IPersonDatabaseLayer personDb,
            IPriceListDatabaseLayer priceListDb,
            IPaymentDatabaseLayer paymentDb,
            IGenericEntityIntegrationProvider integrationProvider,
            ITimeZoneService timeZoneService,
            ILanguageTranslationProvider languageProvider,
            IPriceListTypeResolver priceListTypeResolver,
            IClassTypeResolver classTypeResolver)
        {
            this.enumDb = enumDb;
            this.personDb = personDb;
            this.priceListDb = priceListDb;
            this.paymentDb = paymentDb;
            this.integrationProvider = integrationProvider;
            this.timeZoneService = timeZoneService;
            this.languageProvider = languageProvider;
            this.priceListTypeResolver = priceListTypeResolver;
            this.classTypeResolver = classTypeResolver;
        }

        // merges class filter and profile filter
        public ClassFilter MergeProfileFilterToClassFilter(ClassFilter classFilter, ProfileFilter profileFilter, HashSet<ClassCategoryEnum> grantedClassCategories)
        {
            classFilter.ProfileIds = profileFilter.ProfileIds;

            if (grantedClassCategories.Count == 1)
            {
                classFilter.ClassCategoryId = grantedClassCategories.First();
            }

            return classFilter;
        }

        // initializes class for edit
        public ClassForEdit InitializeClassForEdit(
            long profileId,
            ClassCategoryEnum classCategoryId,
            CurrencyEnum currencyId,
            long? currentPriceListId = null,
            long? currentPayPalKeyId = null, 
            long? currentIntegrationCode = null,
            ClassValidationResult classValidationResult = null)
        {
            var allowedClassTypes = classTypeResolver.GetClassTypesByClassCategoryId(classCategoryId);
            var allowedPriceListTypeId = priceListTypeResolver.GetPriceListTypeIdAllowedForClassCategoryId(classCategoryId);

            var result = new ClassForEdit
            {
                FormConfiguration = classTypeResolver.GetClassFormConfigurationByClassCategoryId(classCategoryId),
                ClassTypes = enumDb.GetItemsByFilter(EnumTypeEnum.MainClassType)
                    .Where(x => allowedClassTypes.Contains((ClassTypeEnum)x.Id)).ToList(),

                Translations = languageProvider.GetTranslationOptions(),
                PersonHelper = new PersonHelper(personDb.GetMinimizedPersonsByProfileId(profileId)),

                PriceLists = priceListDb.GetActivePriceLists(currentPriceListId, allowedPriceListTypeId, currencyId).Select(x => DropDownItem.Item(x.PriceListId, x.Name)).ToList()
            };

            // loads timezones
            if (!result.FormConfiguration.ShowOnlyDates)
            {
                result.TimeZones = enumDb.GetItemsByFilter(EnumTypeEnum.TimeZone);
            }

            // loads integration codes
            if (result.FormConfiguration.ShowIntegrationCode)
            {
                result.IntegrationEntities = integrationProvider
                    .GetActiveIntegrationEntities(currentIntegrationCode, profileId)
                    .Select(x => DropDownItem.Item(x.Item1, x.Item2)).ToList();
            }
            
            // loads paypal keys
            if (result.FormConfiguration.ShowPayPalKey)
            {
                currentPayPalKeyId = currentPayPalKeyId != NoPaymentId ? currentPayPalKeyId : null;
                var payPalKeyFilter = new PayPalKeyFilter
                {
                    UserGroupTypeId = UserGroupTypeEnum.MainProfile,
                    UserGroupId = profileId,
                    CurrencyId = currencyId,
                    IsActive = true
                };

                var payPalKeys = new List<DropDownItem>();
                payPalKeys.Add(DropDownItem.Item(NoPaymentId, NoPaymentText));
                payPalKeys.AddRange(paymentDb.GetActivePayPalKeys(currentPayPalKeyId, payPalKeyFilter).Select(x => DropDownItem.Item(x.PayPalKeyId, x.Name)));
                result.PayPalKeys = payPalKeys;
            }

            // checks class type and price list consistency
            if (classValidationResult != null && classValidationResult.IsInconsistentClassAndPriceListType)
            {
                result.IsInconsistentClassAndPriceListType = true;
                result.ForbiddenClassTypeId = classValidationResult.ForbiddenClassTypeId;
                result.ForbiddenPriceListId = classValidationResult.ForbiddenPriceListId;
            }

            return result;
        }

        // converts class to class form
        public ClassForm ConvertToClassForm(Class cls)
        {
            if (cls.ClassPersons == null)
                throw new InvalidOperationException("ClassPersons is not included into Class object.");

            var integrationCode = integrationProvider.GetIntegrationCode(cls.IntegrationTypeId, cls.IntegrationEntityId);
            var result = new ClassForm
            {
                ClassId = cls.ClassId,
                ProfileId = cls.ProfileId,
                ClassTypeId = cls.ClassTypeId,
                ClassCategoryId = cls.ClassCategoryId,
                Location = cls.Location,
                TranslationCode = languageProvider.GetTranslationCode(cls.OriginLanguageId, cls.TransLanguageId),
                TimeZoneId = cls.TimeZoneId,

                RegistrationStart = cls.RegistrationStart,
                RegistrationEnd = cls.RegistrationEnd,
                EventStart = cls.EventStart,
                EventEnd = cls.EventEnd,

                CoordinatorId = cls.CoordinatorId,
                GuestInstructorId = cls.GuestInstructorId,
                InstructorIds = cls.ClassPersons.Where(x => x.RoleTypeId == PersonRoleTypeEnum.Instructor).Select(x => x.PersonId).ToList(),
                ApprovedStaffIds = cls.ClassPersons.Where(x => x.RoleTypeId == PersonRoleTypeEnum.ApprovedStaff).Select(x => x.PersonId).ToList(),

                PriceListId = cls.PriceListId,
                PayPalKeyId = cls.PayPalKeyId ?? NoPaymentId,
                IntegrationCode = integrationCode,                

                IsWwaFormAllowed = cls.IsWwaFormAllowed,               
               
                // current common codelist ids                
                CurrentPriceListId = cls.PriceListId,
                CurrentPayPayKeyId = cls.PayPalKeyId,
                CurrentIntegrationCode = integrationCode,
                ClassState = GetClassState(cls)

            };
            return result;
        }


        // converts class to class list item
        public ClassListItem ConvertToClassListItem(Class cls)
        {
            if (cls.ClassType == null)
                throw new InvalidOperationException("ClassType is not included into Class object.");
            if (cls.TimeZone == null)
                throw new InvalidOperationException("TimeZone is not included into Class object.");
            if (cls.Profile == null)
                throw new InvalidOperationException("Profile is not included into Class object.");

            // creates result
            var result = new ClassListItem
            {
                ClassId = cls.ClassId,
                ClassState = GetClassState(cls),
                ClassTypeId = cls.ClassTypeId,
                ClassCategoryId = cls.ClassCategoryId,
                ClassType = cls.ClassType.Description,
                Location = cls.Location,              
                TimeZone = cls.TimeZone.Name,
                RegistrationStart = cls.RegistrationStart,
                RegistrationEnd = cls.RegistrationEnd,
                EventStart = cls.EventStart,
                EventEnd = cls.EventEnd,
                EnvironmentTypeId = cls.EnvironmentTypeId,
                ProfileId = cls.ProfileId,
                Profile = cls.Profile.Name,
                ShowOnlyDate = classTypeResolver.GetClassFormConfigurationByClassCategoryId(cls.ClassCategoryId).ShowOnlyDates
            };
            result.Title = MainTextHelper.GetEventOneLineHeader(result.EventStart, result.EventEnd, result.Location, result.ClassType);

            // language loading
            result.OriginLanguage = enumDb.GetItemById(EnumTypeEnum.Language, (int)cls.OriginLanguageId).Description;
            if (cls.TransLanguageId.HasValue)
                result.TransLanguage = enumDb.GetItemById(EnumTypeEnum.Language, (int) cls.TransLanguageId.Value).Description;

            return result;
        }

        // converts class to class detail
        public ClassDetail ConvertToClassDetail(Class cls)
        {
            if (cls.ClassType == null)
                throw new InvalidOperationException("ClassType is not included into Class object.");
            if (cls.TimeZone == null)
                throw new InvalidOperationException("TimeZone is not included into Class object.");
            if (cls.ClassPersons == null)
                throw new InvalidOperationException("ClassPersons is not included into Class object.");
            if (cls.PriceList == null)
                throw new InvalidOperationException("PriceList is not included into Class object.");
            if (cls.Profile == null)
                throw new InvalidOperationException("Profile is not included into Class object.");
            if (cls.Currency == null)
                throw new InvalidOperationException("Currency is not included into Class object.");

            // creates result
            var classPersonIds = GetClassPersonIds(cls);
            var personHelper = new PersonHelper(personDb.GetMinimizedPersonsByIds(classPersonIds));
            var result = new ClassDetail
            {
                ClassId = cls.ClassId,
                ClassState = GetClassState(cls),
                ClassTypeId = cls.ClassTypeId,
                ClassCategoryId = cls.ClassCategoryId,
                ClassType = cls.ClassType.Description,
                Location = cls.Location,
                TimeZone = cls.TimeZone.Name,
                RegistrationStart = cls.RegistrationStart,
                RegistrationEnd = cls.RegistrationEnd,
                EventStart = cls.EventStart,
                EventEnd = cls.EventEnd,
                Coordinator = personHelper.GetPersonNameById(cls.CoordinatorId),
                GuestInstructor = personHelper.GetPersonNameById(cls.GuestInstructorId),
                Instructors = cls.ClassPersons.Where(x => x.RoleTypeId == PersonRoleTypeEnum.Instructor).Select(x => personHelper.GetPersonNameById(x.PersonId)).ToList(),
                ApprovedStaffs = cls.ClassPersons.Where(x => x.RoleTypeId == PersonRoleTypeEnum.ApprovedStaff).Select(x => personHelper.GetPersonNameById(x.PersonId)).ToList(),
                IsWwaFormAllowed = cls.IsWwaFormAllowed,
                Currency = MainTextHelper.GetCurrencyFullName(cls.Currency.Description, cls.Currency.Name),
                PriceList = cls.PriceList.Name,                                              
                EnvironmentTypeId = cls.EnvironmentTypeId, 
                
                IntegrationEntityId = cls.IntegrationEntityId,
                IntegrationEntityName = integrationProvider.GetIntegrationEntityName(cls.IntegrationTypeId, cls.IntegrationEntityId),
                IntegrationTypeId = cls.IntegrationTypeId,
                ProfileId = cls.ProfileId,
                Profile = cls.Profile.Name,

                FormConfiguration = classTypeResolver.GetClassFormConfigurationByClassCategoryId(cls.ClassCategoryId)
            };
            result.Title = MainTextHelper.GetEventOneLineHeader(result.EventStart, result.EventEnd, result.Location, result.ClassType);

            // language loading
            result.OriginLanguage = enumDb.GetItemById(EnumTypeEnum.Language, (int)cls.OriginLanguageId).Description;
            if (cls.TransLanguageId.HasValue)
                result.TransLanguage = enumDb.GetItemById(EnumTypeEnum.Language, (int)cls.TransLanguageId.Value).Description;

            // paypal loading
            if (cls.PayPalKeyId.HasValue)
            {
                var payPalKey = paymentDb.GetPayPalKeyById(cls.PayPalKeyId.Value);
                if (payPalKey == null)
                {
                    throw new ArgumentException($"There is no PayPal key with id {cls.PayPalKeyId}.");
                }
                result.PayPalKey = payPalKey.Name;
            }
            else
            {
                result.PayPalKey = NoPaymentText;
            }

            // integration type info
            result.IntegrationType = integrationProvider.GetIntegrationTypeById(result.IntegrationTypeId)?.Description;

            return result;
        }

        // converts class form to class
        public Class ConvertToClass(ClassForm form)
        {           
            var defaultDate = default(DateTime);
            var integrationRelation = integrationProvider.GetIntegrationRelationByCode(form.IntegrationCode);
            var result = new Class
            {
                ClassId = form.ClassId,
                ProfileId = form.ProfileId,
                ClassTypeId = form.ClassTypeId ?? 0,
                ClassCategoryId = form.ClassCategoryId,
                Location = form.Location,
                // todo: it is possible to add consistency validation (into Validation method of ClassAdministration)
                OriginLanguageId = languageProvider.GetOriginalLanguageId(form.TranslationCode ?? 0),
                TransLanguageId = languageProvider.GetTranslationLanguageId(form.TranslationCode ?? 0),
                TimeZoneId = form.TimeZoneId ?? 0,
                RegistrationStart = form.RegistrationStart,                
                RegistrationEnd = form.RegistrationEnd ?? defaultDate,
                EventStart = form.EventStart ?? defaultDate,
                EventEnd = form.EventEnd ?? defaultDate,                

                CoordinatorId = form.CoordinatorId ?? 0,
                GuestInstructorId = form.GuestInstructorId,

                PriceListId = form.PriceListId ?? 0,
                PayPalKeyId = form.PayPalKeyId != NoPaymentId ? (form.PayPalKeyId ?? 0) : (long?)null,
                IntegrationEntityId = integrationRelation.IntegrationEntityId,
                IntegrationTypeId = integrationRelation.IntegrationTypeId,

                IsWwaFormAllowed = form.IsWwaFormAllowed,             
            };

            // sets utcs
            result.RegistrationStartUtc = timeZoneService.GetUtcDateTime(result.RegistrationStart, result.TimeZoneId);
            result.RegistrationEndUtc = timeZoneService.GetUtcDateTime(result.RegistrationEnd, result.TimeZoneId);
            result.EventStartUtc = timeZoneService.GetUtcDateTime(result.EventStart, result.TimeZoneId);
            result.EventEndUtc = timeZoneService.GetUtcDateTime(result.EventEnd, result.TimeZoneId);

            // adds persons
            foreach(var personId in form.InstructorIds)
                result.ClassPersons.Add(CreateClassPerson(form.ClassId, personId, PersonRoleTypeEnum.Instructor));
            foreach (var personId in form.ApprovedStaffIds)
                result.ClassPersons.Add(CreateClassPerson(form.ClassId, personId, PersonRoleTypeEnum.ApprovedStaff));

            return result;
        }


        // gets no integration IntegrationCode
        public long GetNoIntegrationCode()
        {
            var result = integrationProvider.GetIntegrationCode(IntegrationTypeEnum.NoIntegration, null);
            return result;
        }


        #region private fields 

        // creates class person
        public ClassPerson CreateClassPerson(long classId, long personId, PersonRoleTypeEnum roleId)
        {
            var result = new ClassPerson
                {
                    ClassId = classId,
                    PersonId = personId,
                    RoleTypeId = roleId
            };
            return result;
        }

        #endregion


        #region static methods

        // gets person ids
        public static HashSet<long> GetClassPersonIds(Class cls)
        {
            if (cls.ClassPersons == null)
            {
                throw new InvalidOperationException("ClassPersons is not included into Class object.");
            }

            var result = new HashSet<long>();
            result.Add(cls.CoordinatorId);
            if (cls.GuestInstructorId.HasValue)
            {
                result.Add(cls.GuestInstructorId.Value);
            }

            result.UnionWith(cls.ClassPersons.Select(x => x.PersonId));
            return result;
        }

        // gets person ids for specified roles
        public static HashSet<long> GetClassPersonIdsForRoles(Class cls, HashSet<PersonRoleTypeEnum> roles)
        {
            if (cls.ClassPersons == null)
            {
                throw new InvalidOperationException("ClassPersons is not included into Class object.");
            }

            var result = new HashSet<long>();
            if (roles.Contains(PersonRoleTypeEnum.Coordinator))
            {
                result.Add(cls.CoordinatorId);
            }

            if (roles.Contains(PersonRoleTypeEnum.GuestInstructor) && cls.GuestInstructorId.HasValue)
            {
                result.Add(cls.GuestInstructorId.Value);
            }

            result.UnionWith(cls.ClassPersons.Where(x => roles.Contains(x.RoleTypeId)).Select(x => x.PersonId));
            return result;
        }

        // gets persons ids in class including ClassReportSetting's persons
        public static HashSet<long> GetClassPersonIdsIncludingReportSetting(Class cls)
        {
            if (cls.ClassReportSetting == null)
            {
                throw new InvalidOperationException("ClassReportSetting is not included into Class object.");
            }

            var result = GetClassPersonIds(cls);
            if (cls.ClassReportSetting.LocationInfoId.HasValue)
            {
                result.Add(cls.ClassReportSetting.LocationInfoId.Value);
            }

            return result;
        }

        // gets class's persons ids with person role types
        public static Dictionary<long, List<PersonRoleTypeEnum>> GetClassPersonIdsWithRoles(Class cls)
        {
            if (cls.ClassPersons == null)
                throw new InvalidOperationException("ClassPersons is not included into Class object.");

            var result = new Dictionary<long, List<PersonRoleTypeEnum>>();
            AddPersonAndRole(result, cls.CoordinatorId, PersonRoleTypeEnum.Coordinator);            
            if (cls.GuestInstructorId.HasValue)
                AddPersonAndRole(result, cls.GuestInstructorId.Value, PersonRoleTypeEnum.GuestInstructor);

            // adds class persons
            foreach (var classPerson in cls.ClassPersons)
                AddPersonAndRole(result, classPerson.PersonId, classPerson.RoleTypeId);          
            return result;
        }

        // resolves class state
        public static ClassState GetClassState(Class cls)
        {
            var nowUtc = DateTime.UtcNow;
            if (cls.IsCanceled)
                return ClassState.Canceled;
            if (cls.IsFinished)
                return ClassState.Completed;
            if (cls.RegistrationStartUtc.HasValue && cls.RegistrationEndUtc <= nowUtc)
                return ClassState.AfterRegistration;
            if (cls.RegistrationStartUtc.HasValue && cls.RegistrationStartUtc.Value <= nowUtc)
                return ClassState.InRegistration;
            return ClassState.New;
        }

        public static bool IsTerminalState(Class cls)
        {
            var classState = ClassConvertor.GetClassState(cls);
            return classState == ClassState.Canceled || classState == ClassState.Completed;
        }
      
        // fills class base page model
        public static ClassShortDetail ConvertToClassShortDetial(Class cls)
        {
            var result = new ClassShortDetail
            {
                ClassId = cls.ClassId,
                ClassCategoryId = cls.ClassCategoryId,
                ClassState = GetClassState(cls),
                ClassTitle = GetClassTitle(cls)
            };
            return result;
        }   

        // gets class title from class
        public static string GetClassTitle(Class cls)
        {
            if (cls.ClassType == null)
                throw new InvalidOperationException("ClassType is not included into Class object.");

            var result = MainTextHelper.GetEventOneLineHeader(cls.EventStart, cls.EventEnd, cls.Location, cls.ClassType.Description);
            return result;
        }

        #endregion


        #region static helpers

        // adds person id and role type id into dictionary
        private static void AddPersonAndRole(Dictionary<long, List<PersonRoleTypeEnum>> personsWithRoles, long personId, PersonRoleTypeEnum roleTypeId)
        {
            if (!personsWithRoles.TryGetValue(personId, out var roles))
                personsWithRoles[personId] = roles = new List<PersonRoleTypeEnum>();
            roles.Add(roleTypeId);
        }

        #endregion

    }

}
