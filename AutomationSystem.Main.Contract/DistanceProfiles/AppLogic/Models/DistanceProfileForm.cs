using CorabeuControl.ModelMetadata;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AutomationSystem.Main.Contract.DistanceProfiles.AppLogic.Models
{
    public class DistanceProfileForm
    {
        [HiddenInput]
        public long DistanceProfileId { get; set; }

        [HiddenInput]
        public long ProfileId { get; set; }

        [HiddenInput]
        public long? CurrentPriceListId { get; set; }
        [HiddenInput]
        public long? CurrentPayPalKeyId { get; set; }

        [Required]
        [DisplayName("Price list")]
        [PickInputOptions(NoItemText = "no price list", Placeholder = "select price list")]
        public long? PriceListId { get; set; }

        [Required]
        [DisplayName("PayPal key")]
        [PickInputOptions(NoItemText = "no PayPal key", Placeholder = "select payPal key")]
        public long? PayPalKeyId { get; set; }

        [Required]
        [DisplayName("Distance coordinator")]
        [PickInputOptions(ControlType = PickControlType.TypeaheadDropDownInput, Placeholder = "select distance coordinator")]
        public long? DistanceCoordinatorId { get; set; }
    }
}
