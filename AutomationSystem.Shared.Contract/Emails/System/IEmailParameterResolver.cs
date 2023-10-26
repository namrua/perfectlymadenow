using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Emails.System
{
    /// <summary>
    /// Resolves values of parameters
    /// </summary>
    public interface IEmailParameterResolver
    {

        // determines whether parameter name is supported in resolver
        bool IsSupportedParameters(string parameterNameWithBrackets);

        // gets value
        string GetValue(LanguageEnum languageId, string parameterNameWithBrackets);

    }

}
