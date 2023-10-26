using System;
using System.Collections.Generic;
using AutomationSystem.Shared.Contract.Emails.Integration.Models;
using AutomationSystem.Shared.Contract.Payment.Integration.Models;
using AutomationSystem.Shared.Contract.Preferences.Data;
using AutomationSystem.Shared.Contract.Preferences.System;
using AutomationSystem.Shared.Contract.Preferences.System.Models;

namespace AutomationSystem.Shared.Core.Preferences.System
{
    /// <summary>
    /// Provides saved preferences
    /// </summary>       
    public class CorePreferenceProvider : ICorePreferenceProvider
    {

        // private components
        private readonly IPreferenceDatabaseLayer preferenceDb;

        // constructor
        public CorePreferenceProvider(IPreferenceDatabaseLayer preferenceDb)
        {
            this.preferenceDb = preferenceDb;
        }


        // gets supported languages
        public HashSet<string> GetSupportedLanguages()
        {
            var allowedLanguages =
                preferenceDb.GetPreferenceByKey(CorePreferenceKey.LocalisationSupportedLanguages);
            var result = allowedLanguages == null
                ? new HashSet<string>(new[] {"en"})
                : new HashSet<string>(allowedLanguages.Value.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries));
            return result;
        }


        // gets paypal preferences
        public PayPalPreferences GetPayPalPreferences()
        {
            var result = new PayPalPreferences();           
            var supportedLocale =
                preferenceDb.GetPreferenceByKey(CorePreferenceKey.PayPalPreferencesSupportedLocale);
            result.SupportedLocale = supportedLocale == null
                ? new HashSet<string>(new[] {"en_US"})
                : new HashSet<string>(supportedLocale.Value.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries));
            return result;
        }


        // gets administration email address
        public string GetAdminRecipient()
        {
            var result = preferenceDb.GetPreferenceByKey(CorePreferenceKey.EmailSenderAdminRecipient).Value;
            return result;
        }

        // gets helpdesk email address
        public string GetHelpdeskEmail()
        {
            var result = preferenceDb.GetPreferenceByKey(CorePreferenceKey.EmailSenderHelpdeskRecipient).Value;
            return result;
        }
       
        // gets email sender settings
        public EmailSenderSettings GetEmailSenderSettings()
        {
            var result = new EmailSenderSettings();
            result.SendGridApi = preferenceDb.GetPreferenceByKey(CorePreferenceKey.EmailSenderSendGridApi).Value;
            result.SenderEmail = preferenceDb.GetPreferenceByKey(CorePreferenceKey.EmailSenderSenderEmail).Value;
            result.SenderName = preferenceDb.GetPreferenceByKey(CorePreferenceKey.EmailSenderSenderName).Value;
            return result;
        }     

        #region privat methods

        // gets bool
        private bool GetBool(string parameterValue)
        {
            bool result;
            bool.TryParse(parameterValue, out result);
            return result;
        }

        #endregion


    }

}
