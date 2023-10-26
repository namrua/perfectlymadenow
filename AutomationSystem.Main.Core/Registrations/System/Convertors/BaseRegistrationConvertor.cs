using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Enums.System;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Localisation.System;

namespace AutomationSystem.Main.Core.Registrations.System.Convertors
{

    /// <summary>
    /// Provides base conversion of class registration entities
    /// </summary>
    public class BaseRegistrationConvertor : IBaseRegistrationConvertor
    {
       
        private readonly IEnumDatabaseLayer enumDb;
        private readonly ILocalisationService localisationService;
        private readonly IRegistrationStateProvider registrationStateProvider;
        private readonly IRegistrationTypeResolver registrationTypeResolver;

        private readonly bool useLocalisation;
        
        
        public BaseRegistrationConvertor(
            IEnumDatabaseLayer enumDb,
            IRegistrationStateProvider registrationStateProvider,
            IRegistrationTypeResolver registrationTypeResolver,
            ILocalisationService localisationService = null)
        {
            this.enumDb = enumDb;
            this.localisationService = localisationService;
            this.registrationStateProvider = registrationStateProvider;
            this.registrationTypeResolver = registrationTypeResolver;

            useLocalisation = localisationService != null;
        }


        // fills registration for edit
        // LOCALISED
        public void FillRegistrationForEdit<TForm>(RegistrationForEdit<TForm> registrationForEdit, Class cls)
            where TForm : BaseRegistrationForm
        {
            registrationForEdit.Countries = useLocalisation 
                ? localisationService.GetLocalisedEnumItemsByFilter(EnumTypeEnum.Country)
                : enumDb.GetItemsByFilter(EnumTypeEnum.Country);
            registrationForEdit.ClassCategoryId = cls.ClassCategoryId;
            registrationForEdit.Countries = registrationForEdit.Countries.SortDefaultCountryFirst();
            registrationForEdit.Languages.Add(GetEnumItemById(EnumTypeEnum.Language, (int)cls.OriginLanguageId));
            if (cls.TransLanguageId.HasValue)
                registrationForEdit.Languages.Add(GetEnumItemById(EnumTypeEnum.Language, (int)cls.TransLanguageId.Value));
            
        }       

        public void FillRegistrationForm(BaseRegistrationForm form, ClassRegistration registration)
        {
            form.ClassId = registration.ClassId;
            form.ClassRegistrationId = registration.ClassRegistrationId;
            form.RegistrationTypeId = registration.RegistrationTypeId;
            form.LanguageId = registration.LanguageId;
        }

        // fills base registration detail
        // LOCALISED
        public void FillRegistrationDetail(BaseRegistrationDetail detail, ClassRegistration registration)
        {
            if (registration.RegistrationType == null)
                throw new InvalidOperationException("RegistrationType is not included into ClassRegistration object.");
            if (registration.Class == null)
                throw new InvalidOperationException("RegistrationType is not included into Class object.");

            detail.ClassRegistrationId = registration.ClassRegistrationId;
            detail.ClassId = registration.ClassId;
            detail.RegistrationTypeId = registration.RegistrationTypeId;
            detail.RegistrationType = useLocalisation 
                ? localisationService.GetLocalisedEnumItem(EnumTypeEnum.MainRegistrationType, (int) registration.RegistrationTypeId).Description
                : registration.RegistrationType.Description;
            detail.LanguageId = registration.LanguageId;
            detail.Language = GetEnumItemById(EnumTypeEnum.Language, (int) registration.LanguageId).Description;
            detail.ClassState = ClassConvertor.GetClassState(registration.Class);
        }
        
        public ClassRegistration ConvertToClassRegistration(BaseRegistrationForm form)
        {
            var result = new ClassRegistration
            {
                ClassRegistrationId = form.ClassRegistrationId,
                RegistrationTypeId = form.RegistrationTypeId,
                RegistrationFormTypeId = registrationTypeResolver.GetRegistrationFormType(form.RegistrationTypeId),
                ClassId = form.ClassId,
                LanguageId = form.LanguageId ?? 0,
            };
            return result;
        }


        // converts ClassRegistraton to Registration list item
        // NOT LOCALISED
        public RegistrationListItem ConvertToRegistrationListItem(ClassRegistration registration)
        {
            if (registration.RegistrationType == null)
                throw new InvalidOperationException("RegistrationType is not included into ClassRegistration object.");
            if (registration.ApprovementType == null)
                throw new InvalidOperationException("ApprovementType is not included into ClassRegistration object.");
            if (registration.StudentAddress.Country == null)
                throw new InvalidOperationException("StudentAddress.Country is not included into ClassRegistration object.");
            if (registration.RegistrantAddressId.HasValue && registration.RegistrantAddress == null)
                throw new InvalidOperationException("RegistrantAddress is not included into ClassRegistration object.");

            var result = new RegistrationListItem
            {
                ClassRegistrationId = registration.ClassRegistrationId,
                StudentName = MainTextHelper.GetFullName(
                    AddressConvertor.ToLogicString(registration.StudentAddress.FirstName), 
                    AddressConvertor.ToLogicString(registration.StudentAddress.LastName)),
                RegistrantName = registration.RegistrantAddress == null ? null : MainTextHelper.GetFullName(
                    AddressConvertor.ToLogicString(registration.RegistrantAddress.FirstName),
                    AddressConvertor.ToLogicString(registration.RegistrantAddress.LastName)),
                Country = registration.StudentAddress.Country.Description,
                Email = registration.StudentEmail ?? registration.RegistrantEmail,
                RegistrationTypeId = registration.RegistrationTypeId,
                RegistrationType = registration.RegistrationType.Description,
                RegistrationState = registrationStateProvider.GetRegistrationState(registration),
                ApprovementTypeId = registration.ApprovementTypeId,
                ApprovementType = registration.ApprovementType.Description,
                IsReviewed = registrationStateProvider.IsReviewed(registration),
                LanguageId = registration.LanguageId,
                Language = enumDb.GetItemById(EnumTypeEnum.Language, (int)registration.LanguageId).Description,                
                Created = registration.Created,
                Approved = registration.Approved,
                Canceled = registration.Canceled
            };
            result.Name = MainTextHelper.GetRegistrationName(result.StudentName, result.RegistrantName);

            return result;
        }

        
        
        #region private fields
        
        private IEnumItem GetEnumItemById(EnumTypeEnum enumTypeId, int enumId)
        {
            return useLocalisation 
                ? localisationService.GetLocalisedEnumItem(enumTypeId, enumId) 
                : enumDb.GetItemById(enumTypeId, enumId);
        }

        #endregion

      
    }

}
