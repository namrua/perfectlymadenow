using System.Collections.Generic;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Contract.PriceLists.AppLogic.Models
{
    /// <summary>
    /// Model for price list main page
    /// </summary>
    public class PriceListMainPageModel
    {
        public List<PriceListListItem> Items { get; set; }
        public List<PriceListType> PriceListTypes { get; set; }

        // constructor
        public PriceListMainPageModel()
        {
            Items = new List<PriceListListItem>();
            PriceListTypes = new List<PriceListType>();
        }
    }
}