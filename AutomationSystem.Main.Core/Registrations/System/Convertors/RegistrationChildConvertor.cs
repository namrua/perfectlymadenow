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
    /// Registration child convertor
    /// </summary>
    public class RegistrationChildConvertor : ITypedRegistrationConvertor<RegistrationChildForm, RegistrationChildDetail>
    {

        private readonly IBaseRegistrationConvertor baseConvertor;
        private readonly IAddressConvertor addressConvertor;
        private readonly IClassOperationChecker classOperationChecker;
        
        public RegistrationChildConvertor(IRegistrationLogicProvider registrationLogicProvider, IClassOperationChecker classOperationChecker)
        {
            baseConvertor = registrationLogicProvider.BaseRegistrationConvertor;
            addressConvertor = registrationLogicProvider.AddressConvertor;
            this.classOperationChecker = classOperationChecker;

            ControllerInfo = new RegistrationControllerInfo
            {
                ViewForDetail = "ChildPersonalData",
                ViewForForm = "ChildEdit",
                PartialViewDetailForHome = "~/Views/Home/_ChildDetail.cshtml",
                ActionForSave = "ChildEdit"
                
            };
        }


        // determines registration form type
        public RegistrationFormTypeEnum RegistrationFormType => RegistrationFormTypeEnum.Child;

        // gets controller info
        public RegistrationControllerInfo ControllerInfo { get; }       

        
        public RegistrationForEdit<RegistrationChildForm> InitializeRegistrationForEdit(Class cls)
        {
            var result = new RegistrationForEdit<RegistrationChildForm>(new RegistrationChildForm());           
            baseConvertor.FillRegistrationForEdit(result, cls);
            return result;
        }

        // creates new TForm
        public RegistrationChildForm InitializeRegistrationForm(RegistrationTypeEnum registrationTypeId, long classId)
        {
            var result = new RegistrationChildForm
            {
                ClassRegistrationId = 0,
                ClassId = classId,
                RegistrationTypeId = registrationTypeId,
            };
            return result;
        }
        
        public RegistrationChildForm ConvertToRegistrationForm(ClassRegistration registration)
        {
            if (registration.StudentAddress == null)
                throw new InvalidOperationException("StudentAddress is not included into ClassRegistration object.");
            if (registration.RegistrantAddress == null)
                throw new InvalidOperationException("RegistrantAddress is not included into ClassRegistration object.");

            var result = new RegistrationChildForm
            {
                ParentAddress = addressConvertor.ConvertToAddressForm(registration.RegistrantAddress),
                ParentEmail = registration.RegistrantEmail,
                ParentPhone = registration.RegistrantPhone,
                ChildAddress = addressConvertor.ConvertToAddressForm(registration.StudentAddress),
                ChildEmail = registration.StudentEmail,
            };
            baseConvertor.FillRegistrationForm(result, registration);
            return result;
        }
        
        public RegistrationChildDetail ConvertToRegistrationDetail(ClassRegistration registration)
        {
            if (registration.StudentAddress == null)
                throw new InvalidOperationException("StudentAddress is not included into ClassRegistration object.");
            if (registration.RegistrantAddress == null)
                throw new InvalidOperationException("RegistrantAddress is not included into ClassRegistration object.");

            var result = new RegistrationChildDetail
            {
                ParentAddress = addressConvertor.ConvertToAddressDetail(registration.RegistrantAddress),
                ParentEmail = registration.RegistrantEmail,
                ParentPhone = registration.RegistrantPhone,
                ChildAddress = addressConvertor.ConvertToAddressDetail(registration.StudentAddress),
                ChildEmail = registration.StudentEmail,
            };
            baseConvertor.FillRegistrationDetail(result, registration);
            result.CanEdit = classOperationChecker.IsOperationAllowed(ClassOperation.EditRegistration, result.ClassState);
            return result;
        }
        
        public ClassRegistration ConvertToClassRegistration(RegistrationChildForm form)
        {
            var result = baseConvertor.ConvertToClassRegistration(form);
            result.RegistrantAddress = addressConvertor.ConvertToAddress(form.ParentAddress, true);
            result.RegistrantEmail = form.ParentEmail;
            result.RegistrantPhone = form.ParentPhone;
            result.StudentAddress = addressConvertor.ConvertToAddress(form.ChildAddress, true);
            result.StudentEmail = form.ChildEmail;
            return result;
        }

    }

}
