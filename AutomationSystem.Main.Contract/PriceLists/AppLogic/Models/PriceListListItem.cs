using System.ComponentModel;

namespace AutomationSystem.Main.Contract.PriceLists.AppLogic.Models
{
    /// <summary>
    /// Encapsulates pricelist for list item
    /// </summary>
    public class PriceListListItem
    {
        // public properties
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("ID")]
        public long PriceListId { get; set; }

        [DisplayName("Price list type")]
        public string PriceListType { get; set; }

        [DisplayName("Currency")]
        public string CurrencyCode { get; set; }

        [DisplayName("State")]
        public PriceListState State { get; set; }
    }
}