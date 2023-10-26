using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Core.Emails.System.Models
{
    /// <summary>
    /// The class that serves as key for language wwa maps
    /// </summary>
    internal class LanguageWwaKey
    {

        public LanguageEnum LanguageId { get; }
        public bool IsWwa { get; }

        // constructor
        public LanguageWwaKey(LanguageEnum languageId, bool isWwa)
        {
            LanguageId = languageId;
            IsWwa = isWwa;
        }

    }
}