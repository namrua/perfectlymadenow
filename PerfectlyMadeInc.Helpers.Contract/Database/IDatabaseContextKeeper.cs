using System;

namespace PerfectlyMadeInc.Helpers.Contract.Database
{
    /// <summary>
    /// Keeps context for direct entity modification
    /// </summary>   
    public interface IDatabaseContextKeeper<TResult> : IDisposable
    {

        // Result of query
        TResult Result { get; }

        // Saves changes
        void SaveChanges();

    }

}
