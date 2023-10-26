using System.ComponentModel;

namespace AutomationSystem.Main.Contract.Payment.AppLogic.Models
{
    /// <summary>
    /// Main PayPalKey list detail
    /// </summary>
    public class MainPayPalKeyListItem
    {

        [DisplayName("ID")]
        public long PayPalKeyId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Environment")]
        public string Environment { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }

        [DisplayName("Profile")]
        public string Profile { get; set; }

        [DisplayName("Currency")]
        public string CurrencyCode { get; set; }

    }

}
