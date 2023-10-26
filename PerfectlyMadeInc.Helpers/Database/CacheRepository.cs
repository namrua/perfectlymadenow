using System.Collections.Generic;
using PerfectlyMadeInc.Helpers.Contract.Database;

namespace PerfectlyMadeInc.Helpers.Database
{
    /// <summary>
    /// cache repository
    /// </summary>
    public static class CacheRepository
    {

        // private fields
        private static readonly List<ICacheExpirator> expirators = new List<ICacheExpirator>();

        // register expirator
        public static void RegisterExpirator(ICacheExpirator expirator)
        {
            expirators.Add(expirator);
        }

        // expira all caches
        public static void ExpireAllCaches()
        {
            foreach (var expirator in expirators)
                expirator.SetAsExpired();
        }

    }

}
