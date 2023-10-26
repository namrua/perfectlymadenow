using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract.Classes.AppLogic;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Core.Home.AppLogic;
using AutomationSystem.Main.Core.PriceLists.Data;
using AutomationSystem.Main.Core.PriceLists.System;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.EntityIntegration.System;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using CorabeuControl.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace AutomationSystem.Main.Core.Classes.AppLogic
{
    public class ClassAdministration : IClassAdministration
    {
        private readonly IClassDatabaseLayer classDb;
        private readonly IPriceListDatabaseLayer priceListDb;
        private readonly IGenericEntityIntegrationProvider integrationProvider;
        private readonly IProfileDatabaseLayer profileDb;
        private readonly IIdentityResolver identityResolver;
        private readonly IPublicPaymentResolver publicPaymentResolver;
        private readonly IClassService classService;
        private readonly IClassConvertor classConvertor;
        private readonly IClassOperationChecker classOperationChecker;
        private readonly IPriceListTypeResolver priceListTypeResolver;
        private readonly IClassTypeResolver classTypeResolver;

        public ClassAdministration(
            IClassDatabaseLayer classDb,
            IPriceListDatabaseLayer priceListDb,
            IGenericEntityIntegrationProvider integrationProvider,
            IProfileDatabaseLayer profileDb,
            IIdentityResolver identityResolver,
            IPublicPaymentResolver publicPaymentResolver,
            IClassConvertor classConvertor,
            IClassService classService,
            IClassOperationChecker classOperationChecker,
            IPriceListTypeResolver priceListTypeResolver,
            IClassTypeResolver classTypeResolver)
        {
            this.classDb = classDb;
            this.priceListDb = priceListDb;
            this.integrationProvider = integrationProvider;
            this.profileDb = profileDb;
            this.identityResolver = identityResolver;
            this.publicPaymentResolver = publicPaymentResolver;
            this.classService = classService;
            this.classConvertor = classConvertor;
            this.classOperationChecker = classOperationChecker;
            this.priceListTypeResolver = priceListTypeResolver;
            this.classTypeResolver = classTypeResolver;
        }

        public ClassesForList GetClassesForList(ClassFilter filter, bool search)
        {
            var result = new ClassesForList(filter);
            result.WasSearched = search;
            
            var profileFilter = new ProfileFilter()
            {
                ProfileIds = identityResolver.GetProfileIdsForClasses()?.ToList()
            };
            var profiles = profileDb.GetProfilesByFilter(profileFilter);

            // sets filter ddls
            result.Profiles = profiles.Select(x => DropDownItem.Item(x.ProfileId, x.Name)).ToList();
            result.ClassStates = new List<ClassState> { ClassState.New, ClassState.InRegistration, ClassState.AfterRegistration, ClassState.Completed, ClassState.Canceled };

            var grantedClassCategories = identityResolver.GetGrantedClassCategories();
            result.ClassCategories = classDb.GetClassCategoriesByIds(grantedClassCategories);

            // executes class searching
            if (search)
            {
                var classFilter = classConvertor.MergeProfileFilterToClassFilter(filter, profileFilter, grantedClassCategories);
                var classes = classDb.GetClassesByFilter(classFilter, ClassIncludes.TimeZone | ClassIncludes.ClassType | ClassIncludes.Profile);
                result.Items = classes.Select(classConvertor.ConvertToClassListItem).ToList();

                // todo: #BICH batch item checking
            }
            return result;
        }

        public ClassDetail GetClassDetailById(long classId)
        {
            var cls = GetClassById(classId, ClassIncludes.TimeZone | ClassIncludes.ClassType | ClassIncludes.ClassPersons | ClassIncludes.PriceList | ClassIncludes.Profile | ClassIncludes.Currency);
            identityResolver.CheckEntitleForClass(cls);
            var result = classConvertor.ConvertToClassDetail(cls);
            result.ShowLink = publicPaymentResolver.IsPublicPaymentAllowedForClass(cls);
            result.ApprovedRegistrations = classDb.GetApprovedRegistrationCountByClassId(classId);
            result.CanEdit = classOperationChecker.IsOperationAllowed(ClassOperation.EditClass, result.ClassState);
            result.CanDelete = classOperationChecker.IsOperationAllowed(ClassOperation.DeleteClass, result.ClassState) && result.ApprovedRegistrations == 0;
            return result;
        }
        
        public ClassForEdit GetNewClassForEdit(long profileId, ClassCategoryEnum classCategoryId)
        {
            identityResolver.CheckEntitleForProfileIdAndClassCategory(profileId, classCategoryId);

            var currencyId = GetProfileCurrencyId(profileId, classCategoryId);

            var result = classConvertor.InitializeClassForEdit(profileId, classCategoryId, currencyId);
            result.Form = classService.GetNewClassForm(profileId, classCategoryId, currencyId, result.PersonHelper);
            result.CanFullEditClass = classOperationChecker.IsOperationAllowed(ClassOperation.FullEditClass, result.Form.ClassState);
            return result;
        }

        public ClassForEdit GetClassForEditById(long classId)
        {
            var cls = GetClassById(classId, ClassIncludes.ClassPersons);
            identityResolver.CheckEntitleForClass(cls);

            var result = classConvertor.InitializeClassForEdit(cls.ProfileId, cls.ClassCategoryId, cls.CurrencyId, cls.PriceListId, cls.PayPalKeyId, 
                integrationProvider.GetIntegrationCode(cls.IntegrationTypeId, cls.IntegrationEntityId));
            result.Form = classConvertor.ConvertToClassForm(cls);
            classOperationChecker.CheckOperation(ClassOperation.EditClass, result.Form.ClassState, classId);
            result.CanFullEditClass = classOperationChecker.IsOperationAllowed(ClassOperation.FullEditClass, result.Form.ClassState);
            return result;
        }

        public ClassForEdit GetFormClassForEdit(ClassForm form, ClassValidationResult classValidationResult)
        {
            identityResolver.CheckEntitleForProfileIdAndClassCategory(form.ProfileId, form.ClassCategoryId);

            var result = classConvertor.InitializeClassForEdit(form.ProfileId, form.ClassCategoryId, form.ProfileCurrencyId, form.CurrentPriceListId, 
                form.CurrentPayPayKeyId, form.CurrentIntegrationCode, classValidationResult);
            result.Form = form;
            result.CanFullEditClass = classOperationChecker.IsOperationAllowed(ClassOperation.FullEditClass, result.Form.ClassState);
            return result;
        }

        public ClassValidationResult ValidateClassForm(ClassForm form)
        {
            identityResolver.CheckEntitleForProfileIdAndClassCategory(form.ProfileId, form.ClassCategoryId);

            var result = new ClassValidationResult();

            // check Security consistency issues
            // class type correspond with class type category
            if (form.ClassTypeId.HasValue && !classTypeResolver.GetClassTypesByClassCategoryId(form.ClassCategoryId).Contains(form.ClassTypeId ?? 0))
            {
                throw new SecurityException($"Class type id {form.ClassTypeId} does not correspond to class category {form.ClassCategoryId}");
            }

            // is wwa allowed is false for lectures
            if (form.ClassCategoryId == ClassCategoryEnum.Lecture && form.IsWwaFormAllowed)
            {
                throw new SecurityException("Lecture does not support IsWwaFormAllowed = true.");
            }

            // checks that approved staffs are empty and integration is set to No integration for WwaClass category
            if (form.ClassCategoryId == ClassCategoryEnum.DistanceClass)
            {
                if (form.ApprovedStaffIds.Any())
                    throw new SecurityException("Distance classes does not support Approved staffs.");
                if (form.IntegrationCode != classConvertor.GetNoIntegrationCode())
                    throw new SecurityException("Distance classes does not support integration.");
            }

            // checks whether price list id and class category are consistent
            if (form.PriceListId.HasValue && form.ClassTypeId.HasValue)
            {
                var priceList = priceListDb.GetPriceListById(form.PriceListId.Value);
                if (priceList != null && !priceListTypeResolver.IsPriceListAllowedForClassCategoryId(priceList.PriceListTypeId, form.ClassCategoryId))
                {
                    result.IsInconsistentClassAndPriceListType = true;
                    result.ForbiddenPriceListId = form.PriceListId.Value;
                    result.ForbiddenClassTypeId = form.ClassTypeId.Value;
                    return result;
                }
            }

            // checks whether event start is lower that event end and  so on
            if (form.RegistrationStart.HasValue && form.RegistrationEnd.HasValue && form.RegistrationStart > form.RegistrationEnd)
            {
                return result;
            }

            if (form.RegistrationEnd.HasValue && form.EventStart.HasValue && form.RegistrationEnd > form.EventStart)
            {
                return result;
            }

            if (form.EventStart.HasValue && form.EventEnd.HasValue && form.EventStart > form.EventEnd)
            {
                return result;
            }

            result.IsValid = true;
            return result;
        }

        public long SaveClass(ClassForm form, EnvironmentTypeEnum? env)
        {
            var classId = form.ClassId;
            if (classId == 0)
            {
                identityResolver.CheckEntitleForProfileIdAndClassCategory(form.ProfileId, form.ClassCategoryId);
                classId = classService.InsertClass(form, env);
            }
            else
            {
                var originClass = GetClassById(form.ClassId, ClassIncludes.ClassPersons);
                identityResolver.CheckEntitleForClass(originClass);

                var classState = ClassConvertor.GetClassState(originClass);
                classOperationChecker.CheckOperation(ClassOperation.EditClass, classState, classId);
                var isFullyEditable = classOperationChecker.IsOperationAllowed(ClassOperation.FullEditClass, classState);

                classService.UpdateClass(form, originClass, isFullyEditable);
            }
            return classId;
        }

        public void DeleteClass(long classId)
        {
            var toCheck = GetClassById(classId);
            identityResolver.CheckEntitleForClass(toCheck);

            var approvedRegistrations = classDb.GetApprovedRegistrationCountByClassId(classId);
            if (approvedRegistrations > 0)
                throw new InvalidOperationException($"Class with id {classId} contains approved registrations and cannot be deleted.");
            classDb.DeleteClass(classId, ClassOperationOption.CheckOperation);
            integrationProvider.DetachEntity(EntityTypeEnum.MainClass, classId);
        }

        #region private methods

        public Class GetClassById(long classId, ClassIncludes includes = ClassIncludes.None)
        {
            var result = classDb.GetClassById(classId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Class with id {classId}.");
            }

            return result;
        }

        private CurrencyEnum GetProfileCurrencyId(long profileId, ClassCategoryEnum classCategoryId)
        {
            if (!classTypeResolver.IsCurrencyAllowed(classCategoryId))
            {
                return LocalisationInfo.DefaultCurrency;
            }

            var profile = profileDb.GetProfileById(profileId, ProfileIncludes.ClassPreference);
            if (profile == null)
            {
                throw new ArgumentException($"There is no Profile with Id {profileId}.");
            }

            var currencyId = profile.ClassPreference.CurrencyId;
            return currencyId;
        }

        #endregion
    }
}