using System.Collections.Generic;
using AutomationSystem.Shared.Contract.Emails.Integration.Models;
using AutomationSystem.Shared.Contract.Payment.Integration.Models;

namespace AutomationSystem.Shared.Contract.Preferences.System
{
    /// <summary>
    /// Provides saved preferences
    /// </summary>
    public interface ICorePreferenceProvider
    {

        // gets supported languages
        HashSet<string> GetSupportedLanguages();

        // gets paypal preferences
        PayPalPreferences GetPayPalPreferences();

        // gets administration email address
        string GetAdminRecipient();

        // gets helpdesk email address
        string GetHelpdeskEmail();        

        // gets email sender settings
        EmailSenderSettings GetEmailSenderSettings();      
        
    }
}
