using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.PriceLists.Data.Extensions;
using AutomationSystem.Main.Core.PriceLists.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using PerfectlyMadeInc.Helpers.Database;

namespace AutomationSystem.Main.Core.PriceLists.Data
{
    /// <summary>
    /// Provides price list database layer
    /// </summary>
    public class PriceListDatabaseLayer : IPriceListDatabaseLayer
    {
        // gets price list types
        public List<PriceListType> GetPriceListTypes()
        {
            using (var context = new MainEntities())
            {
                var result = context.PriceListTypes.ToList();
                return result;
            }
        }

        // gets active price lists
        public List<PriceList> GetActivePriceLists(long? currentId, PriceListTypeEnum allowedPriceListTypeId, CurrencyEnum currencyId)
        {
            using (var context = new MainEntities())
            {
                var result = context.PriceLists.Active().Where(x => 
                    !x.IsDiscarded
                    && x.IsApprovded
                    && x.PriceListTypeId == allowedPriceListTypeId 
                    && x.CurrencyId == currencyId)
                    .ToList();
                if (!currentId.HasValue || result.Any(x => x.PriceListId == currentId)) return result;

                // adds current if not listed - !!! Active() is ignored !!!
                var current = context.PriceLists.FirstOrDefault(x => x.PriceListId == currentId);
                if (current == null)
                    throw new InvalidOperationException($"Current PriceList with id {currentId} does not exist.");

                // adds current to the start of the list
                var newResult = new List<PriceList> { current };
                newResult.AddRange(result);
                return newResult;
            }
        }


        // gets list of price lists with price list items
        public List<PriceList> GetPriceLists(PriceListIncludes includes = PriceListIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.PriceLists.AddIncludes(includes).Active().OrderBy(x => x.Name).ToList();
                result = result.Select(x => PriceListRemoveInactive.RemoveInactiveForPriceList(x, includes)).ToList();
                return result;
            }            
        }     


        // gets price list item by id
        public PriceList GetPriceListById(long id, PriceListIncludes includes = PriceListIncludes.None)
        {
            using (var context = new MainEntities())
            {               
                var result = context.PriceLists.AddIncludes(includes).Active().FirstOrDefault(x => x.PriceListId == id);
                result = PriceListRemoveInactive.RemoveInactiveForPriceList(result, includes);
                return result;
            }
        }

        // insert price list
        public long InsertPriceList(PriceList priceList)
        {
            using (var context = new MainEntities())
            {
                var result = context.PriceLists.Add(priceList);
                context.SaveChanges();
                return result.PriceListId;
            }
        }

    
        // update price list
        public void UpdatePriceList(PriceList priceList, PriceListOperationOption options = PriceListOperationOption.None)
        {
            using (var context = new MainEntities())
            {
                // updates price list
                var toUpdate = context.PriceLists.AddIncludes(PriceListIncludes.PriceListItems).FirstOrDefault(x => x.PriceListId == priceList.PriceListId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Price list with id {priceList.PriceListId}.");

                if (toUpdate.IsApprovded || toUpdate.IsDiscarded)
                    throw new InvalidOperationException($"Price list with id {priceList.PriceListId} is already approved or discarded and cannot be changed.");

                toUpdate.Name = priceList.Name;
                toUpdate.CurrencyId = priceList.CurrencyId;
                if (!options.HasFlag(PriceListOperationOption.KeepStatus))
                {
                    toUpdate.IsApprovded = priceList.IsApprovded;
                    toUpdate.Approved = priceList.Approved;
                    toUpdate.IsDiscarded = priceList.IsDiscarded;
                    toUpdate.Discarded = priceList.Discarded;
                }               
                if (!options.HasFlag(PriceListOperationOption.KeepOwnerId))
                    toUpdate.OwnerId = priceList.OwnerId;
                if (options.HasFlag(PriceListOperationOption.CheckPriceListType))
                    if (toUpdate.PriceListTypeId != priceList.PriceListTypeId)
                        throw new SecurityException($"Current PriceList type {priceList.PriceListTypeId} is inconsistent with database PriceList {toUpdate.PriceListId} type {toUpdate.PriceListTypeId}.");

                // updates price list items
                var priceListItems = context.PriceListItems.Active().Where(x => x.PriceListId == priceList.PriceListId);
                var updateResolver = new SetUpdateResolver<PriceListItem, long>(
                    x => x.PriceListItemId, (origItem, newItem) => { origItem.Price = newItem.Price; });
                var strategy = updateResolver.ResolveStrategy(priceListItems, priceList.PriceListItems);
                context.PriceListItems.AddRange(strategy.ToAdd);
                context.PriceListItems.RemoveRange(strategy.ToDelete);

                // saves data
                context.SaveChanges();
            }
        }

      
        // delete price list
        public void DeletePriceList(long priceListId)
        {
            using (var context = new MainEntities())
            {
                var toDelete = context.PriceLists.FirstOrDefault(x => x.PriceListId == priceListId);
                if (toDelete == null) return;
                context.PriceLists.Remove(toDelete);
                context.SaveChanges();
            }
        }        

        // approves price list by id
        public void ApprovePriceList(long priceListId)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.PriceLists.Active().FirstOrDefault(x => x.PriceListId == priceListId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Price list with id {priceListId}.");
                if (toUpdate.IsApprovded || toUpdate.IsDiscarded)
                    throw new InvalidOperationException($"Price list with id {priceListId} is already approved or discarded.");

                toUpdate.IsApprovded = true;
                toUpdate.Approved = DateTime.Now;
                context.SaveChanges();
            }
                
        }

        // discard price list by id
        public void DiscardPriceList(long priceListId)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.PriceLists.Active().FirstOrDefault(x => x.PriceListId == priceListId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Price list with id {priceListId}.");
                if (!toUpdate.IsApprovded || toUpdate.IsDiscarded)
                    throw new InvalidOperationException($"Price list with id {priceListId} is not approved or is already discarded.");

                toUpdate.IsDiscarded = true;
                toUpdate.Discarded = DateTime.Now;
                context.SaveChanges();
            }
        }


        // gets price list items by price list Id
        public List<PriceListItem> GetPriceListItemsByPriceListId(long priceListId)
        {
            using (var context = new MainEntities())
            {
                var result = context.PriceListItems.Active().Where(x => x.PriceListId == priceListId).ToList();
                return result;
            }
        }
    }
}
