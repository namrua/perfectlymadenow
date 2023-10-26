using System.ComponentModel;

namespace AutomationSystem.Main.Contract.DistanceProfiles.AppLogic.Models
{
    public class DistanceProfileDetail
    {
        [DisplayName("ID")]
        public long DistanceProfileId { get; set; }

        [DisplayName("Profile")]
        public string Profile { get; set; }

        [DisplayName("PayPal key")]
        public string PayPalKey { get; set; }

        [DisplayName("Price list")]
        public string PriceList { get; set; }

        [DisplayName("Distance coordinator")]
        public string DistanceCoordinator { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }

    }
}
