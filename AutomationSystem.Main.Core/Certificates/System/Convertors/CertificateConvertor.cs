using System;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.Certificates.System.Models;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Certificates.System.Convertors
{
    public class CertificateConvertor: ICertificateConvertor
    {
        private readonly IRegistrationTypeResolver registrationTypeResolver;

        public CertificateConvertor(IRegistrationTypeResolver registrationTypeResolver)
        {
            this.registrationTypeResolver = registrationTypeResolver;
        } 

        public CertificateInfo ConvertToCertificateInfo(Class cls, ClassRegistration registration)
        {           
            if (registration.StudentAddress == null)
                throw new InvalidOperationException("StudentAddress is not included into ClassRegistration object.");
            
            var result = GetCertificateInfo(cls, registration.StudentAddress);
            if (registrationTypeResolver.IsWwaRegistration(registration.RegistrationTypeId))
            {
                result.Registrant = MainTextHelper.GetFullName(registration.RegistrantAddress.FirstName.Trim(), registration.RegistrantAddress.LastName.Trim());
                result.UseWwaTemplate = true;
                result.RegistrantParticipant = $"{result.Registrant}  {result.Name}";
            }
            return result;
        }

        public CertificateInfo ConvertToCertificateInfo(Class cls, Person person)
        {           
            if (person.Address == null)
                throw new InvalidOperationException("Address is not included into Person object.");

            var result = GetCertificateInfo(cls, person.Address);
            return result;
        }

        #region private methods

        private CertificateInfo GetCertificateInfo(Class cls, Address address)
        {            
            var result = new CertificateInfo
            {
                Name = MainTextHelper.GetFullName(address.FirstName.Trim(), AddressConvertor.ToLogicString(address.LastName?.Trim())),
                // Description = $"{MainTextHelper.GetBriefDate(cls.EventStart)} & {MainTextHelper.GetBriefDate(cls.EventEnd)}        {cls.Location.Trim()}"
                Description = $"{MainTextHelper.GetEventDate(cls.EventStart, cls.EventEnd)}, {cls.EventStart.Year}        {cls.Location.Trim()}"
            };
            return result;
        }

        #endregion
    }
}
