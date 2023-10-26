using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.PriceLists.AppLogic.Models
{
    /// <summary>
    /// Price list item detail
    /// </summary>
    public class PriceListItemDetail
    {
        // public properties
        [DisplayName("Registration type")]
        public RegistrationTypeEnum RegistrationTypeId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Price")]
        public decimal Price { get; set; }
    }
}