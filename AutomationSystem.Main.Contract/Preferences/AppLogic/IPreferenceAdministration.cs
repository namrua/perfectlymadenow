using AutomationSystem.Main.Contract.Preferences.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Preferences.AppLogic
{
    /// <summary>
    /// Providers services for preference administration
    /// </summary>
    public interface IPreferenceAdministration
    {

        // gets email settings
        EmailPreferencesForm GetEmailPreferencesForEdit();

        // saves email information
        void SaveEmailPreferences(EmailPreferencesForm form);


        // returns extended languages form
        LanguagePreferencesForEdit GetLanguagePreferencesForEdit();

        // gets languages for edit for validation
        LanguagePreferencesForEdit GetFormLanguagePreferencesForEdit(LanguagePreferencesForm form);

        // saves allowed languages information
        void SaveLanguagePreferences(LanguagePreferencesForm form);


        // gets IZI preferences
        IziPreferenceForEdit GetIziPreferenceForEdit();

        // gets IZI preferences by form
        IziPreferenceForEdit GetFormIziPreferenceForEdit(IziPreferenceForm form);

        // saves IZI preferences
        void SaveIziPreferences(IziPreferenceForm form);

    }

}
