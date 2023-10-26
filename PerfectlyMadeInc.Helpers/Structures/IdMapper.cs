using System;
using System.Collections.Generic;
using System.Linq;
using PerfectlyMadeInc.Helpers.Contract.Structures;

namespace PerfectlyMadeInc.Helpers.Structures
{
    /// <summary>
    /// Provides mapping of object by its IDs
    /// </summary>
    /// <typeparam name="TId">Type of ID</typeparam>
    /// <typeparam name="TObject">Type of object</typeparam>
    public class IdMapper<TId, TObject> : IIdMapper<TId, TObject>
    {

        private readonly Func<TObject, TId> idSelector;
        private readonly Func<TId, TObject> emptyObjectCreator;
        private readonly List<TObject> items;
        private readonly Dictionary<TId, TObject> map;

        
        // constructor
        public IdMapper(IEnumerable<TObject> items, Func<TObject, TId> idSelector,
            Func<TId, TObject> emptyObjectCreator = null)
        {
            this.idSelector = idSelector;
            this.emptyObjectCreator = emptyObjectCreator ?? (x => default(TObject));
            this.items = items.ToList();

            map = this.items.ToDictionary(idSelector, y => y);
        } 


        // All items
        public List<TObject> Items => items;

        // All items in the dictionary
        public Dictionary<TId, TObject> Map => map;

        // Tries to get item, if there is no item, returns default object (or empty object)
        public TObject TryGetItem(TId id)
        {
            if (!map.TryGetValue(id, out var result))
                result = emptyObjectCreator(id);
            return result;
        }

        // Gets item, if there is no item, throws KeyNotFoundException
        public TObject GetItem(TId id)
        {
            var result = map[id];
            return result;
        }
       
    }
    
}
