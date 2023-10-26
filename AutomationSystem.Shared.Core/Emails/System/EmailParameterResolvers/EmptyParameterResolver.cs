using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Shared.Core.Emails.System.EmailParameterResolvers
{
    /// <summary>
    /// Empty object pattern parameter resolver
    /// </summary>
    public class EmptyParameterResolver : IEmailParameterResolver
    {

        // determines whether parameter name is supported in resolver
        public bool IsSupportedParameters(string parameterNameWithBrackets)
        {
            return false;
        }

        // gets value
        public string GetValue(LanguageEnum languageId, string parameterNameWithBrackets)
        {
            return null;
        }

    }

}
