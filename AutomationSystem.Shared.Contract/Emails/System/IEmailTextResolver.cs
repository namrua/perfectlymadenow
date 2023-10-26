using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Emails.System
{
    /// <summary>
    /// Resolves emailt texts
    /// </summary>
    public interface IEmailTextResolver
    {

        // gets email text
        string GetText(LanguageEnum languageId, string templateText);

    }

}
