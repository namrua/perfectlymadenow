using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.Classes.System.Emails.EmailParameterResolvers
{

    /// <summary>
    /// Resolves common email parameters for Main version
    /// </summary>
    public class MainCommonParameterResolver : IEmailParameterResolver
    {

        private const string Helpdesk = "{{Email.Helpdesk}}";
        private const string RegistrationListNote = "{{Localised.RegistrationListNote}}";       
       
        private readonly HashSet<string> supportedParameters;
        private readonly IEmailServiceHelper helper;


        // constructor
        public MainCommonParameterResolver(IEmailServiceHelper helper)
        {
            this.helper = helper;
            supportedParameters = new HashSet<string>(new[] { Helpdesk, RegistrationListNote });
        }

      

        // determines whether parameter name is supported in resolver
        public bool IsSupportedParameters(string parameterNameWithBrackets)
        {
            return supportedParameters.Contains(parameterNameWithBrackets);
        }

        // gets value
        public string GetValue(LanguageEnum languageId, string parameterNameWithBrackets)
        {          
            object resultObject;
            var langInfo = helper.LanguageInfoProvider.GetLanguageInfo(languageId);
            switch (parameterNameWithBrackets)
            {
                case Helpdesk:
                    resultObject = helper.GetHelpdeskEmail();
                    break;

                case RegistrationListNote:
                    resultObject = helper.GetLocalisedText(langInfo, "RegistrationListNote");
                    break;               

                default:
                    return null;
            }
            var result = helper.EmailParameterConvertor.Convert(languageId, parameterNameWithBrackets, resultObject);
            return result;
        }

    }

}
