using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Model;
using System;

namespace AutomationSystem.Main.Core.Registrations.System.Convertors
{

    /// <summary>
    /// Registration student convertor
    /// </summary>
    public class RegistrationStudentConvertor : ITypedRegistrationConvertor<RegistrationStudentForm, RegistrationStudentDetail>
    {

        private readonly IBaseRegistrationConvertor baseConvertor;
        private readonly IAddressConvertor addressConvertor;
        private readonly IClassOperationChecker classOperationChecker;


        public RegistrationStudentConvertor(
            IRegistrationLogicProvider registrationLogicProvider,
            IClassOperationChecker classOperationChecker)
        {
            baseConvertor = registrationLogicProvider.BaseRegistrationConvertor;
            addressConvertor = registrationLogicProvider.AddressConvertor;
            this.classOperationChecker = classOperationChecker;

            ControllerInfo = new RegistrationControllerInfo
            {
                ViewForDetail = "StudentPersonalData",
                ViewForForm = "StudentEdit",
                PartialViewDetailForHome = "~/Views/Home/_StudentDetail.cshtml",
                ActionForSave = "StudentEdit"
            };
        }

        
        public RegistrationFormTypeEnum RegistrationFormType => RegistrationFormTypeEnum.Adult;
        
        public RegistrationControllerInfo ControllerInfo { get; }
        
        public RegistrationForEdit<RegistrationStudentForm> InitializeRegistrationForEdit(Class cls)
        {
            var result = new RegistrationForEdit<RegistrationStudentForm>(new RegistrationStudentForm());           
            baseConvertor.FillRegistrationForEdit(result, cls);
            return result;
        }
        
        public RegistrationStudentForm InitializeRegistrationForm(RegistrationTypeEnum registrationTypeId, long classId)
        {
            var result = new RegistrationStudentForm
            {
                ClassRegistrationId = 0,
                ClassId = classId,
                RegistrationTypeId = registrationTypeId,
            };
            return result;
        }
        
        public RegistrationStudentForm ConvertToRegistrationForm(ClassRegistration registration)
        {
            if (registration.StudentAddress == null)
                throw new InvalidOperationException("StudentAddress is not included into ClassRegistration object.");

            var result = new RegistrationStudentForm
            {
                Address = addressConvertor.ConvertToAddressForm(registration.StudentAddress),
                Email = registration.StudentEmail,
                Phone = registration.StudentPhone
            };
            baseConvertor.FillRegistrationForm(result, registration);
            return result;
        }
        
        public RegistrationStudentDetail ConvertToRegistrationDetail(ClassRegistration registration)
        {
            if (registration.StudentAddress == null)
                throw new InvalidOperationException("StudentAddress is not included into ClassRegistration object.");

            var result = new RegistrationStudentDetail
            {
                Address = addressConvertor.ConvertToAddressDetail(registration.StudentAddress),
                Email = registration.StudentEmail,
                Phone = registration.StudentPhone
            };
            baseConvertor.FillRegistrationDetail(result, registration);
            result.CanEdit = classOperationChecker.IsOperationAllowed(ClassOperation.EditRegistration, result.ClassState);
            return result;
        }
        
        public ClassRegistration ConvertToClassRegistration(RegistrationStudentForm form)
        {
            var result = baseConvertor.ConvertToClassRegistration(form);
            result.StudentAddress = addressConvertor.ConvertToAddress(form.Address, true);
            result.StudentEmail = form.Email;
            result.StudentPhone = form.Phone;
            return result;
        }

    }

}
