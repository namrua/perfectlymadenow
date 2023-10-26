using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Payment.AppLogic.Models
{
    /// <summary>
    /// PayPal list detail
    /// </summary>
    public class PayPalKeyListItem
    {

        [DisplayName("ID")]
        public long PayPalKeyId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Environment")]
        public string Environment { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }

        [DisplayName("User group type code")]
        public UserGroupTypeEnum? UserGroupTypeId { get; set; }

        [DisplayName("User group ID")]
        public long? UserGroupId { get; set; }

        [DisplayName("Currency")]
        public string CurrencyCode { get; set; }

    }
}