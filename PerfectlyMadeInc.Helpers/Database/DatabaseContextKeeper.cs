using System.Data.Entity;
using PerfectlyMadeInc.Helpers.Contract.Database;

namespace PerfectlyMadeInc.Helpers.Database
{
    /// <summary>
    /// Keeps context for direct entity modification
    /// </summary>   
    public class DatabaseContextKeeper<TResult> : IDatabaseContextKeeper<TResult>
    {

        private readonly DbContext context;


        // constructor
        public DatabaseContextKeeper(DbContext context, TResult result)
        {
            this.context = context;
            Result = result;
        }

        // Result of query
        public TResult Result { get; set; }

        // Saves changes
        public void SaveChanges()
        {
            context.SaveChanges();
        }

        // disposes context
        public void Dispose()
        {
            context.Dispose();
        }

    }

}
