using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.PriceLists.AppLogic.Models;

namespace AutomationSystem.Main.Contract.PriceLists.AppLogic
{
    /// <summary>
    /// Provides price list services
    /// </summary>
    public interface IPriceListService
    {
        // gets price list main page model
        PriceListMainPageModel GetPriceListMainPageModel();

        // gets new pricelist for edit
        PriceListForEdit GetNewPriceListForEdit(PriceListTypeEnum priceListTypeId);

        // gets price list form for edit
        PriceListForEdit GetPriceListForEditByForm(PriceListForm form);

        // get price list for edit
        PriceListForEdit GetPriceListForEditById(long personId);

        // gets price list detail
        PriceListDetail GetPriceListDetailById(long priceListId);

        // validate price list
        bool ValidatePriceList(PriceListForm form);

        // saves price list
        long SavePriceList(PriceListForm priceListForm);

        // discard price list
        void DiscardPriceList(long priceListId);

        // approve price list
        void ApprovePriceList(long priceListId);

        // deletes price list
        void DeletePriceList(long priceListId);
    }
}