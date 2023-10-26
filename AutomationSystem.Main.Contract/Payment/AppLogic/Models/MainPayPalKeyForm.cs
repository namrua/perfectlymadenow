using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Payment.AppLogic.Models
{
    /// <summary>
    /// PayPal key form
    /// </summary>
    public class MainPayPalKeyForm
    {

        [HiddenInput]
        public long PayPalKeyId { get; set; }
        [HiddenInput]
        public long ProfileId { get; set; }

        [DisplayName("Name")]
        [MaxLength(128)]
        public string Name { get; set; }

        [DisplayName("BraintreeToken")]
        [MaxLength(128)]
        public string BraintreeToken { get; set; }

        [DisplayName("Environment")]
        [PickInputOptions(Placeholder = "select environment")]
        public string Environment { get; set; }

        [DisplayName("Is active")]
        public bool Active { get; set; }

        [DisplayName("Currency")]
        [PickInputOptions]
        public CurrencyEnum CurrencyId { get; set; }
    }

}
