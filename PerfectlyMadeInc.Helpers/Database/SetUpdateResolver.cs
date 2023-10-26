using System;
using System.Collections.Generic;
using System.Linq;
using PerfectlyMadeInc.Helpers.Contract.Database;
using PerfectlyMadeInc.Helpers.Contract.Database.Models;

namespace PerfectlyMadeInc.Helpers.Database
{
    /// <summary>
    /// Resolves set update strategy
    /// </summary>
    public class SetUpdateResolver<TObject, TId> : ISetUpdateResolver<TObject>
    {

        // private fields
        private readonly Func<TObject, TId> idSelector;
        private readonly Action<TObject, TObject> updateAction;

        // constructor
        public SetUpdateResolver(Func<TObject, TId> idSelector, Action<TObject, TObject> updateAction)
        {
            this.idSelector = idSelector;
            this.updateAction = updateAction;
        }


        // resolves strategy to update set
        public SetUpdateResolverStrategy<TObject> ResolveStrategy(IEnumerable<TObject> currentItems, IEnumerable<TObject> newItems)
        {
            var result = new SetUpdateResolverStrategy<TObject>();
            var currentMap = currentItems.ToDictionary(x => idSelector(x), y => y);

            foreach (var newItem in newItems)
            {
                TObject itemToUpdate;
                if (currentMap.TryGetValue(idSelector(newItem), out itemToUpdate))
                    updateAction(itemToUpdate, newItem);
                else                
                    result.ToAdd.Add(newItem);                
            }
            var newSet = new HashSet<TId>(newItems.Select(x => idSelector(x)));
            result.ToDelete = currentItems.Where(x => !newSet.Contains(idSelector(x))).ToList();
            return result;
        }       
                
    }

}
