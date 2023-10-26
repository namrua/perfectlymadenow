using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AutomationSystem.Main.Contract.PriceLists.AppLogic.Models
{
    /// <summary>
    /// Price list detail
    /// </summary>
    public class PriceListDetail
    {
        // public properties
        [DisplayName("ID")]
        public long PriceListId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Price list type")]
        public string PriceListType { get; set; }

        [DisplayName("Currency")]
        public string Currency { get; set; }

        [DisplayName("Currency code")]
        public string CurrencyCode { get; set; }

        [DisplayName("Approved")]
        public DateTime? Approved { get; set; }

        [DisplayName("Discarded")]
        public DateTime? Discarded { get; set; }

        [DisplayName("State")]
        public PriceListState State { get; set; }

        [DisplayName("Items")]
        public List<PriceListItemDetail> PriceListItems { get; set; }


        public bool CanDelete { get; set; }


        // constructor
        public PriceListDetail()
        {
            PriceListItems = new List<PriceListItemDetail>();
        }
    }
}