using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.System.Convertors
{

    /// <summary>
    /// Registration WWA convertor
    /// </summary>
    public class RegistrationWwaConvertor : ITypedRegistrationConvertor<RegistrationWwaForm, RegistrationWwaDetail>
    {

        private readonly IBaseRegistrationConvertor baseConvertor;
        private readonly IAddressConvertor addressConvertor;
        private readonly IClassOperationChecker classOperationChecker;
        
        public RegistrationWwaConvertor(IRegistrationLogicProvider registrationLogicProvider, IClassOperationChecker classOperationChecker)
        {
            baseConvertor = registrationLogicProvider.BaseRegistrationConvertor;
            addressConvertor = registrationLogicProvider.AddressConvertor;
            this.classOperationChecker = classOperationChecker;

            ControllerInfo = new RegistrationControllerInfo
            {
                ViewForDetail = "WwaPersonalData",
                ViewForForm = "WwaEdit",
                PartialViewDetailForHome = "~/Views/Home/_WwaDetail.cshtml",
                ActionForSave = "WwaEdit"
            };
        }

        
        public RegistrationFormTypeEnum RegistrationFormType => RegistrationFormTypeEnum.WWA;

        public RegistrationControllerInfo ControllerInfo { get; }      

        
        public RegistrationForEdit<RegistrationWwaForm> InitializeRegistrationForEdit(Class cls)
        {
            var result = new RegistrationForEdit<RegistrationWwaForm>(new RegistrationWwaForm());          
            baseConvertor.FillRegistrationForEdit(result, cls);
            return result;
        }
        
        public RegistrationWwaForm InitializeRegistrationForm(RegistrationTypeEnum registrationTypeId, long classId)
        {
            var result = new RegistrationWwaForm
            {
                ClassRegistrationId = 0,
                ClassId = classId,
                RegistrationTypeId = registrationTypeId,
            };
            return result;
        }
        
        public RegistrationWwaForm ConvertToRegistrationForm(ClassRegistration registration)
        {
            if (registration.StudentAddress == null)
                throw new InvalidOperationException("StudentAddress is not included into ClassRegistration object.");
            if (registration.RegistrantAddress == null)
                throw new InvalidOperationException("RegistrantAddress is not included into ClassRegistration object.");

            var result = new RegistrationWwaForm
            {
                RegistrantAddress = addressConvertor.ConvertToAddressForm(registration.RegistrantAddress),
                RegistrantEmail = registration.RegistrantEmail,
                ParticipantAddress = addressConvertor.ConvertToIncompleteAddressForm(registration.StudentAddress),                
            };
            baseConvertor.FillRegistrationForm(result, registration);
            return result;
        }
        
        public RegistrationWwaDetail ConvertToRegistrationDetail(ClassRegistration registration)
        {
            if (registration.StudentAddress == null)
                throw new InvalidOperationException("StudentAddress is not included into ClassRegistration object.");
            if (registration.RegistrantAddress == null)
                throw new InvalidOperationException("RegistrantAddress is not included into ClassRegistration object.");

            var result = new RegistrationWwaDetail
            {
                RegistrantAddress = addressConvertor.ConvertToAddressDetail(registration.RegistrantAddress),
                RegistrantEmail = registration.RegistrantEmail,
                ParticipantAddress = addressConvertor.ConvertToAddressDetail(registration.StudentAddress),                
            };
            baseConvertor.FillRegistrationDetail(result, registration);
            result.CanEdit = classOperationChecker.IsOperationAllowed(ClassOperation.EditRegistration, result.ClassState);
            return result;
        }
        
        public ClassRegistration ConvertToClassRegistration(RegistrationWwaForm form)
        {
            var result = baseConvertor.ConvertToClassRegistration(form);
            result.RegistrantAddress = addressConvertor.ConvertToAddress(form.RegistrantAddress, true);
            result.RegistrantEmail = form.RegistrantEmail;
            result.StudentAddress = addressConvertor.ConvertToAddress(form.ParticipantAddress, true);            
            return result;
        }

    }

}
