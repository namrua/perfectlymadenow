using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.PriceLists.Data.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.PriceLists.Data
{
    /// <summary>
    /// Provides price list database layer
    /// </summary>
    public interface IPriceListDatabaseLayer
    {

        // gets price list types
        List<PriceListType> GetPriceListTypes();

        // gets active price lists
        List<PriceList> GetActivePriceLists(long? currentId, PriceListTypeEnum allowedPriceListTypeId, CurrencyEnum currencyId);

        // gets list of price lists with price list items
        List<PriceList> GetPriceLists(PriceListIncludes includes = PriceListIncludes.None);       

        // gets price list with price list items by id
        PriceList GetPriceListById(long id, PriceListIncludes includes = PriceListIncludes.None);

        // insert price list
        long InsertPriceList(PriceList priceList);
       
        // update price list
        void UpdatePriceList(PriceList priceList, PriceListOperationOption operations = PriceListOperationOption.None);
      
        // delete price list
        void DeletePriceList(long id);

        // approves price list by id
        void ApprovePriceList(long priceListId);

        // discards price list by id
        void DiscardPriceList(long priceListId);


        // gets price list items by price list Id
        List<PriceListItem> GetPriceListItemsByPriceListId(long priceListId);
    }
}
