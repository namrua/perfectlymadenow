using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.Registrations.System.Emails.EmailParameterResolvers
{

    /// <summary>
    /// Resolves email parameters from Class registration invitation
    /// </summary>
    public class ClassRegistrationInvitationParameterResolver : IEmailParameterResolverWithBinding<ClassRegistrationInvitation>
    {
        
        private const string FormUrl = "{{Invitation.FormUrl}}";        
       
        private ClassRegistrationInvitation invitation;
        private readonly HashSet<string> supportedParameters;
        private readonly IEmailServiceHelper helper;


        // constructor
        public ClassRegistrationInvitationParameterResolver(IEmailServiceHelper helper)
        {
            this.helper = helper;
            supportedParameters = new HashSet<string>(new[] { FormUrl });
        }


        // binds value to email parameters
        public void Bind(ClassRegistrationInvitation data)
        {
            invitation = data;
        }

        // determines whether parameter name is supported in resolver
        public bool IsSupportedParameters(string parameterNameWithBrackets)
        {
            return supportedParameters.Contains(parameterNameWithBrackets);
        }

        // gets value
        public string GetValue(LanguageEnum languageId, string parameterNameWithBrackets)
        {
            if (invitation == null)
                throw new InvalidOperationException("Class registration invitation was not binded.");

            object resultObject;
            var langInfo = helper.LanguageInfoProvider.GetLanguageInfo(languageId);
            switch (parameterNameWithBrackets)
            {
                case FormUrl:
                    resultObject = $"Home/Invitation?request={invitation.RequestCode}";
                    break;               

                default:
                    return null;
            }
            var result = helper.EmailParameterConvertor.Convert(languageId, parameterNameWithBrackets, resultObject);
            return result;
        }

    }

}
