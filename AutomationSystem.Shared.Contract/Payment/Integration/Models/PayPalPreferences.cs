using System.Collections.Generic;

namespace AutomationSystem.Shared.Contract.Payment.Integration.Models
{
    /// <summary>
    /// Encapsulates paypal preferences
    /// </summary>
    public class PayPalPreferences
    {

        // public properties       
        public HashSet<string> SupportedLocale { get; set; }

    }

}
