namespace AutomationSystem.Shared.Contract.Preferences.System.Models
{
    /// <summary>
    /// Core preference constants
    /// </summary>
    public static class CorePreferenceKey
    {

        // preference keys        
        public const string LocalisationSupportedLanguages = "Localisation.SupportedLanguages";

        public const string PayPalPreferencesSupportedLocale = "PayPalPreferences.SupportedLocale";

        public const string EmailSenderAdminRecipient = "EmailSender.AdminRecipient";
        public const string EmailSenderHelpdeskRecipient = "EmailSender.HelpdeskRecipient";
        public const string EmailSenderSendGridApi = "EmailSender.SendGridApi";
        public const string EmailSenderSenderEmail = "EmailSender.SenderEmail";
        public const string EmailSenderSenderName = "EmailSender.SenderName";
        
    }

}
