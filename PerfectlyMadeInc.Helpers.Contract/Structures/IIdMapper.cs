using System.Collections.Generic;

namespace PerfectlyMadeInc.Helpers.Contract.Structures
{
    /// <summary>
    /// Provides mapping of object by its IDs
    /// </summary>
    /// <typeparam name="TId">Type of ID</typeparam>
    /// <typeparam name="TObject">Type of object</typeparam>
    public interface IIdMapper<TId, TObject>
    {

        // All items
        List<TObject> Items { get; }

        // All items in the dictionary
        Dictionary<TId, TObject> Map { get; }

        // Tries to get item, if there is no item, returns default object (or empty object)
        TObject TryGetItem(TId id);

        // Gets item, if there is no item, throws KeyNotFoundException
        TObject GetItem(TId id);

    }
}
