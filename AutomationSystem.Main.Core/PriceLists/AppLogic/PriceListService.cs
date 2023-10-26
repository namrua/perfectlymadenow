using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.PriceLists.AppLogic;
using AutomationSystem.Main.Contract.PriceLists.AppLogic.Models;
using AutomationSystem.Main.Core.PriceLists.AppLogic.Models;
using AutomationSystem.Main.Core.PriceLists.AppLogic.Models.Events;
using AutomationSystem.Main.Core.PriceLists.Data;
using AutomationSystem.Main.Core.PriceLists.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationSystem.Main.Core.PriceLists.AppLogic
{
    /// <summary>
    /// Provides price list services
    /// </summary>
    public class PriceListService : IPriceListService
    {

        // private components
        private readonly IPriceListDatabaseLayer priceListDb;
        private readonly IPriceListFactory priceListFactory;
        private readonly IMainMapper mainMapper;
        private readonly IIdentityResolver identityResolver;
        private readonly IEventDispatcher eventDispatcher;


        // constructor
        public PriceListService(
            IPriceListDatabaseLayer priceListDb,
            IPriceListFactory priceListFactory,
            IMainMapper mainMapper,
            IIdentityResolver identityResolver,
            IEventDispatcher eventDispatcher)
        {
            this.priceListDb = priceListDb;
            this.priceListFactory = priceListFactory;
            this.mainMapper = mainMapper;
            this.identityResolver = identityResolver;
            this.eventDispatcher = eventDispatcher;
        }

        // gets price list main page model
        public PriceListMainPageModel GetPriceListMainPageModel()
        {
            var priceLists = priceListDb.GetPriceLists(PriceListIncludes.PriceListType | PriceListIncludes.Currency);
            var result = new PriceListMainPageModel
            {
                Items = priceLists.Select(mainMapper.Map<PriceListListItem>).ToList(),
                PriceListTypes = priceListDb.GetPriceListTypes()
            };                      
            return result;
        }

        // gets new price list for edit
        public PriceListForEdit GetNewPriceListForEdit(PriceListTypeEnum priceListTypeId)
        {
            var result = priceListFactory.InitializePriceListForEdit(priceListTypeId);
            result.Form.PriceListTypeId = priceListTypeId;
            result.Form.CurrencyId = LocalisationInfo.DefaultCurrency;
            return result;
        }

        // gets price list for edit by id
        public PriceListForEdit GetPriceListForEditById(long priceListId)
        {
            var priceList = priceListDb.GetPriceListById(priceListId, PriceListIncludes.PriceListItems);
            if (priceList == null)
                throw new ArgumentException($"There is no Price list with id {priceListId}");

            if (priceList.IsApprovded || priceList.IsDiscarded)
                throw new InvalidOperationException($"Price list with id {priceListId} is already approved or discarded and cannot be changed.");
            
            var result = priceListFactory.InitializePriceListForEdit(priceList.PriceListTypeId);
            result.Form = mainMapper.Map<PriceListForm>(priceList);
            return result;
        }

        // gets price list form for edit
        public PriceListForEdit GetPriceListForEditByForm(PriceListForm form)
        {
            // creates result
            var result = priceListFactory.InitializePriceListForEdit(form.PriceListTypeId);
            result.Form = form;
            return result;
        }

        // gets price list detail by id
        public PriceListDetail GetPriceListDetailById(long priceListId)
        {
            var priceList = priceListDb.GetPriceListById(priceListId, PriceListIncludes.PriceListItemsRegistrationType | PriceListIncludes.PriceListType | PriceListIncludes.Currency);
            if (priceList == null)
                throw new ArgumentException($"There is no Price list with id {priceListId}.");

            var result = mainMapper.Map<PriceListDetail>(priceList);
            result.CanDelete = CanDeletePriceList(priceListId);
            return result;
        }


        // validate price list
        public bool ValidatePriceList(PriceListForm form)
        {
            var items = mainMapper.Map<List<PriceListItemForValidation>>(form);
            var result = items.All(x => x.Price.HasValue);
            return result;
        }


        // saves price list
        public long SavePriceList(PriceListForm form)
        {
            var dbPriceList = mainMapper.Map<PriceList>(form);
            var result = dbPriceList.PriceListId;
            if (result == 0)
            {          
                dbPriceList.OwnerId = identityResolver.GetOwnerId();
                dbPriceList.IsApprovded = false;
                dbPriceList.IsDiscarded = false;
                result = priceListDb.InsertPriceList(dbPriceList);
            }
            else
                priceListDb.UpdatePriceList(dbPriceList, PriceListOperationOption.KeepOwnerId 
                                                         | PriceListOperationOption.KeepStatus | PriceListOperationOption.CheckPriceListType);

            return result;
        }

        // approve price list
        public void ApprovePriceList(long priceListId)
        {
            priceListDb.ApprovePriceList(priceListId);
        }

        // discard price list
        public void DiscardPriceList(long priceListId)
        {
            priceListDb.DiscardPriceList(priceListId);
        }

        // deletes price list
        public void DeletePriceList(long priceListId)
        {
            if (!CanDeletePriceList(priceListId))
                throw new InvalidOperationException($"Price list with id {priceListId} was assigned to some class and cannot be deleted.");
            priceListDb.DeletePriceList(priceListId);
        }


        #region private methods

        // determines whether pricelist with id can be deleted
        private bool CanDeletePriceList(long priceListId)
        {
            var result = eventDispatcher.Check(new PriceListDeletingEvent(priceListId));
            return result;
        }

        #endregion

    }

}