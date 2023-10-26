using System.Data.Entity.Infrastructure;
using PerfectlyMadeInc.WebEx.IntegrationStates.Data.Models;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.IntegrationStates.Data.Queries
{
    public static class IntegrationStateIncludeExtensions
    {
        // adds includes for IntegrationState
        public static DbQuery<IntegrationState> AddIncludes(this DbQuery<IntegrationState> query, IntegrationStateIncludes includes)
        {
            if (includes.HasFlag(IntegrationStateIncludes.Event))
                query = query.Include("Event");
            return query;
        }
    }
}
