using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.Registrations.System.Emails.EmailParameterResolvers
{

    /// <summary>
    /// Resolves email parameters from Class registration entity
    /// </summary>
    public class ClassRegistrationParameterResolver : IEmailParameterResolverWithBinding<ClassRegistration>
    {

        // cooridnated
        private const string FullName = "{{Student.FullName}}";
        private const string FullNameCapital = "{{Student.FullNameCapital}}";
        private const string Type = "{{Student.Type}}";
        private const string Email = "{{Student.Email}}";

        // distance
        private const string ParticipantFullName = "{{Participant.FullName}}";
        private const string ParticipantFullNameCapital = "{{Participant.FullNameCapital}}";
        private const string RegistrantFullName = "{{Registrant.FullName}}";
        private const string RegistrantFullNameCapital = "{{Registrant.FullNameCapital}}";
        private const string RegistrantEmail = "{{Registrant.Email}}";

        // shared
        private const string AdminRegistrationUrl = "{{Registration.AdminRegistrationUrl}}";
        private const string PaymentUrl = "{{Registration.PaymentUrl}}";


        private ClassRegistration registration;
        private readonly HashSet<string> supportedParameters;
        private readonly IEmailServiceHelper helper;


        // constructor
        public ClassRegistrationParameterResolver(IEmailServiceHelper helper)
        {
            this.helper = helper;
            supportedParameters = new HashSet<string>(new[] {
                FullName, FullNameCapital, Type, Email,
                ParticipantFullName, ParticipantFullNameCapital, RegistrantFullName, RegistrantFullNameCapital, RegistrantEmail,
                AdminRegistrationUrl, PaymentUrl });
        }


        // binds value to email parameters
        public void Bind(ClassRegistration data)
        {
            registration = data;
        }

        // determines whether parameter name is supported in resolver
        public bool IsSupportedParameters(string parameterNameWithBrackets)
        {
            return supportedParameters.Contains(parameterNameWithBrackets);
        }

        // gets value
        public string GetValue(LanguageEnum languageId, string parameterNameWithBrackets)
        {
            if (registration == null)
                throw new InvalidOperationException("Class registration was not binded.");

            object resultObject;
            var langInfo = helper.LanguageInfoProvider.GetLanguageInfo(languageId);
            switch (parameterNameWithBrackets)
            {
                case FullName:
                case ParticipantFullName:
                    resultObject = MainTextHelper.GetFullName(registration.StudentAddress.FirstName,
                        AddressConvertor.ToLogicString(registration.StudentAddress.LastName));
                    break;

                case FullNameCapital:
                case ParticipantFullNameCapital:
                    resultObject = MainTextHelper.GetFullName(registration.StudentAddress.FirstName,
                        AddressConvertor.ToLogicString(registration.StudentAddress.LastName)).ToUpper();
                    break;

                case RegistrantFullName:
                    resultObject = registration.RegistrantAddress == null ? "" 
                        : MainTextHelper.GetFullName(registration.RegistrantAddress.FirstName, registration.RegistrantAddress.LastName);
                    break;

                case RegistrantFullNameCapital:
                    resultObject = registration.RegistrantAddress == null ? "" 
                        : MainTextHelper.GetFullName(registration.RegistrantAddress.FirstName, registration.RegistrantAddress.LastName).ToUpper();
                    break;

                case Type:
                    resultObject = helper.GetLocalisedEnumItem(langInfo, EnumTypeEnum.MainRegistrationType, registration.RegistrationTypeId).Description;
                    break;

                case Email:
                    resultObject = registration.StudentEmail ?? registration.RegistrantEmail;
                    break;

                case RegistrantEmail:
                    resultObject = registration.RegistrantEmail ?? "";
                    break;

                case AdminRegistrationUrl:
                    resultObject = $"Registration/Detail/{registration.ClassRegistrationId}";
                    break;

                case PaymentUrl:
                    resultObject = $"Home/Payment/{registration.ClassRegistrationId}";
                    break;

                default:
                    return null;
            }
            var result = helper.EmailParameterConvertor.Convert(languageId, parameterNameWithBrackets, resultObject);
            return result;
        }

    }

}
