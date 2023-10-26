using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Emails.System
{
    /// <summary>
    /// converts parameters values
    /// </summary>
    public interface IEmailParameterConvertor
    {

        // convert parameter value
        string Convert(LanguageEnum languageId, string parameterNameWithBrackets, object parameterValue);

    }

}
