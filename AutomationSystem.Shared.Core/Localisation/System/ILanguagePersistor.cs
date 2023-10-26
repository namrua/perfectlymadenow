using System.Globalization;

namespace AutomationSystem.Shared.Core.Localisation.System
{
    /// <summary>
    /// Provides persistence of language
    /// </summary>
    public interface ILanguagePersistor
    {

        // gets current language code
        string CurrentLanguageCode { get; }

        // get currrent culture info
        CultureInfo CurrentCultureInfo { get; }

        // initializes language code
        void Initialize();

        // changes current language
        void ChangeLanguage(string languageCode, bool saveCookies = true);

    }

}
