using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Preferences.AppLogic;
using AutomationSystem.Main.Contract.Preferences.AppLogic.Models;
using AutomationSystem.Main.Core.Persons.AppLogic;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Preferences.System.Models;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using AutomationSystem.Shared.Contract.Preferences.Data;
using AutomationSystem.Shared.Contract.Preferences.System.Models;

namespace AutomationSystem.Main.Core.Preferences.AppLogic
{

    /// <summary>
    /// Providers services for settings administration
    /// </summary>
    public class PreferenceAdministration: IPreferenceAdministration
    {

        // private components
        private readonly IPreferenceDatabaseLayer preferenceDb;
        private readonly IEnumDatabaseLayer enumDb;
        private readonly IPersonDatabaseLayer personDb;

        // constructor
        public PreferenceAdministration(IPreferenceDatabaseLayer preferenceDb, IEnumDatabaseLayer enumDb, IPersonDatabaseLayer personDb)
        {
            this.preferenceDb = preferenceDb;
            this.enumDb = enumDb;
            this.personDb = personDb;
        }

        // returns setting languages object
        public LanguagePreferencesForEdit GetLanguagePreferencesForEdit()
        {
            var result = new LanguagePreferencesForEdit();
            result.AllLanguages = enumDb.GetItemsByFilter(EnumTypeEnum.Language);            
            result.DefaultLanguage = LocalisationInfo.DefaultLanguageCode;
            result.Form.SupportedLanguages = GetSupportedLanguages().ToArray();  
            return result;
        }

        // returns setting languages object
        public LanguagePreferencesForEdit GetFormLanguagePreferencesForEdit(LanguagePreferencesForm form)
        {
            var result = new LanguagePreferencesForEdit();
            result.AllLanguages = enumDb.GetItemsByFilter(EnumTypeEnum.Language);
            result.DefaultLanguage = LocalisationInfo.DefaultLanguageCode;
            result.Form = form;
            return result;
        }

        // saves allowed languages information
        public void SaveLanguagePreferences(LanguagePreferencesForm form)
        {
            var supportedLanguages = new HashSet<string>(form.SupportedLanguages.Select(x => x.Trim()));
            supportedLanguages.Add(LocalisationInfo.DefaultLanguageCode);
            string formatedString = string.Join("|", supportedLanguages);
            preferenceDb.Update(CorePreferenceKey.LocalisationSupportedLanguages, formatedString);
        }


       

        // gets email settings
        public EmailPreferencesForm GetEmailPreferencesForEdit()
        {
            var result = new EmailPreferencesForm();
            result.AdminRecipient = preferenceDb.GetPreferenceByKey(CorePreferenceKey.EmailSenderAdminRecipient, false).Value;
            result.HelpdeskRecipient = preferenceDb.GetPreferenceByKey(CorePreferenceKey.EmailSenderHelpdeskRecipient, false).Value;            
            result.SendGridApi = preferenceDb.GetPreferenceByKey(CorePreferenceKey.EmailSenderSendGridApi, false).Value;
            result.SenderEmail = preferenceDb.GetPreferenceByKey(CorePreferenceKey.EmailSenderSenderEmail, false).Value;
            result.SenderName = preferenceDb.GetPreferenceByKey(CorePreferenceKey.EmailSenderSenderName, false).Value;
            return result;
        }

        // saves email information
        public void SaveEmailPreferences(EmailPreferencesForm form)
        {
            preferenceDb.Update(CorePreferenceKey.EmailSenderAdminRecipient, form.AdminRecipient);
            preferenceDb.Update(CorePreferenceKey.EmailSenderHelpdeskRecipient, form.HelpdeskRecipient);            
            preferenceDb.Update(CorePreferenceKey.EmailSenderSendGridApi, form.SendGridApi);
            preferenceDb.Update(CorePreferenceKey.EmailSenderSenderEmail, form.SenderEmail);
            preferenceDb.Update(CorePreferenceKey.EmailSenderSenderName, form.SenderName);
        }


        // gets IZI preferences
        public IziPreferenceForEdit GetIziPreferenceForEdit()
        {
            var form = new IziPreferenceForm();
            form.MasterCoordinatorRecipient = preferenceDb.GetPreferenceByKey(MainPreferenceKey.EmailMasterCoordinatorRecipient, false).Value;
            form.MasterLeadInstructorId = GetNullableLong(preferenceDb.GetPreferenceByKey(MainPreferenceKey.PersonMasterLeadInstructor).Value);

            var result = new IziPreferenceForEdit
            {
                Form = form,
                PersonHelper = new PersonHelper(personDb.GetMinimizedPersonsByProfileId(null))
            };
            return result;
        }

        // gets IZI preferences by form
        public IziPreferenceForEdit GetFormIziPreferenceForEdit(IziPreferenceForm form)
        {
            var result = new IziPreferenceForEdit
            {
                Form = form,
                PersonHelper = new PersonHelper(personDb.GetMinimizedPersonsByProfileId(null))
            };
            return result;
        }

        // saves IZI preferences
        public void SaveIziPreferences(IziPreferenceForm form)
        {            
            preferenceDb.Update(MainPreferenceKey.EmailMasterCoordinatorRecipient, form.MasterCoordinatorRecipient);
            preferenceDb.Update(MainPreferenceKey.PersonMasterLeadInstructor, form.MasterLeadInstructorId?.ToString() ?? "");
        }


        #region private methods

        // gets supported languages
        public HashSet<string> GetSupportedLanguages()
        {
            var allowedLanguages = preferenceDb.GetPreferenceByKey(CorePreferenceKey.LocalisationSupportedLanguages, false);
            var result = allowedLanguages == null
                ? new HashSet<string>(new[] { LocalisationInfo.DefaultLanguageCode })
                : new HashSet<string>(allowedLanguages.Value.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
            return result;
        }

        // gets bool
        private bool GetBool(string parameterValue)
        {
            bool result;
            bool.TryParse(parameterValue, out result);
            return result;
        }

        // gets long? from string
        public long? GetNullableLong(string value)
        {
            if (long.TryParse(value, out var result))
                return result;
            return null;
        }

        #endregion

    }
}
