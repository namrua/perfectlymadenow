using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.Registrations.System.Emails.EmailParameterResolvers
{

    /// <summary>
    /// Resolves email parameters for ClassRegistrationLastClass entity
    /// </summary>
    public class LastClassParameterResolver : IEmailParameterResolverWithBinding<ClassRegistration>
    {

        private const string Location = "{{LastClass.Location}}";
        private const string Month = "{{LastClass.Month}}";
        private const string Year = "{{LastClass.Year}}";

        private ClassRegistration registration;
        private readonly HashSet<string> supportedParameters;
        private readonly IEmailServiceHelper helper;

        // constructor
        public LastClassParameterResolver(IEmailServiceHelper helper)
        {
            this.helper = helper;
            supportedParameters = new HashSet<string>(new[] { Location, Month, Year });
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
                case Location:
                    resultObject = registration.ClassRegistrationLastClass?.Location ?? "";
                    break;

                case Month:
                    var month = registration.ClassRegistrationLastClass?.Month;
                    resultObject = month.HasValue ? MainTextHelper.GetMonthName(month.Value, langInfo.CultureInfo) : "";                 
                    break;

                case Year:
                    resultObject = registration.ClassRegistrationLastClass?.Year ?? (object) "";
                    break;             

                default:
                    return null;
            }
            var result = helper.EmailParameterConvertor.Convert(languageId, parameterNameWithBrackets, resultObject);
            return result;
        }

    }








}
