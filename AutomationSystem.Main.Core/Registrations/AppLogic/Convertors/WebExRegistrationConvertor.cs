using System;
using System.Text;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;

namespace AutomationSystem.Main.Core.Registrations.AppLogic.Convertors
{

    /// <summary>
    /// WebEx convertor
    /// </summary>
    public class WebExRegistrationConvertor: IWebExRegistrationConvertor
    {

        private const string noDiacriticEncoding = "ISO-8859-8";

        private readonly Encoding noDiacriticEncodingClass;
        private readonly IRegistrationStateProvider registrationStateProvider;
        private readonly IRegistrationTypeResolver registrationTypeResolver;


        // constructor
        public WebExRegistrationConvertor(IRegistrationStateProvider registrationStateProvider, IRegistrationTypeResolver registrationTypeResolver)
        {
            this.registrationStateProvider = registrationStateProvider;
            this.registrationTypeResolver = registrationTypeResolver;
            noDiacriticEncodingClass = Encoding.GetEncoding(noDiacriticEncoding);
        }


        // converts class registration to WebEx integration state
        public IntegrationStateDto ConvertToIntegrationState(ClassRegistration registration, long? attendeeId = null)
        {     
            if (registrationTypeResolver.IsWwaRegistration(registration.RegistrationTypeId))
                throw new InvalidOperationException($"Class registration with id {registration.ClassRegistrationId} is WWA and does not support integration with WebEx.");
            
            var result = ConvertChildStudentToIntegrationState(registration);
            result.IntegrationStateTypeId = ResolveIntegrationStateTypeByRegistration(registration);           
            result.EntityTypeId = EntityTypeEnum.MainClassRegistration;
            result.EntityId = registration.ClassRegistrationId;
            result.AttendeeId = attendeeId;
            return result;
        }

        // creates empty WebEx integration state
        public IntegrationStateDto CreateEmptyIntegrationState(long? attendeeId = null)
        {
            var result = new IntegrationStateDto
            {
                IntegrationStateTypeId = IntegrationStateTypeEnum.NotInWebEx,
                AttendeeId = attendeeId,
                EntityTypeId = EntityTypeEnum.MainClassRegistration,
                EntityId = 0,                
            };
            return result;
        }
        

        // resolves correct integration state by registration
        public IntegrationStateTypeEnum ResolveIntegrationStateTypeByRegistration(ClassRegistration registration)
        {
            var result = registrationStateProvider.GetRegistrationState(registration) == RegistrationState.Approved
                ? IntegrationStateTypeEnum.InWebEx
                : IntegrationStateTypeEnum.NotInWebEx;
            return result;
        }


        #region private methods

        // gets WebExPerson from Child/Student registration
        private IntegrationStateDto ConvertChildStudentToIntegrationState(ClassRegistration registration) 
        {
            var address = registration.StudentAddress;          
            var result = new IntegrationStateDto
            {
                FirstName = RemoveDiacriticAndTrim(address.FirstName),
                LastName = RemoveDiacriticAndTrim(address.LastName),
                Street = RemoveDiacriticAndTrim(address.Street),
                Street2 = RemoveDiacriticAndTrim(address.Street2),
                City = RemoveDiacriticAndTrim(address.City),
                State = RemoveDiacriticAndTrim(address.State),
                Country = RemoveDiacriticAndTrim(address.Country.Description),
                ZipCode = RemoveDiacriticAndTrim(address.ZipCode),
                Email = RemoveDiacriticAndTrim(registration.StudentEmail)
            };
            return result;
        }
 

        // removes diacritic form texts
        private string RemoveDiacriticAndTrim(string text)
        {
            if (text == null) return null;            
            var tempBytes = noDiacriticEncodingClass.GetBytes(text.Trim());
            var result = Encoding.UTF8.GetString(tempBytes);
            return result;
        }

        #endregion

    }

}
