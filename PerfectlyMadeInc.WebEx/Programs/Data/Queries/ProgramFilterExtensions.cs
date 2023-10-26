using System.Linq;
using PerfectlyMadeInc.WebEx.Contract.Programs.Models;
using PerfectlyMadeInc.WebEx.Model;
using PerfectlyMadeInc.WebEx.Model.Queries;

namespace PerfectlyMadeInc.WebEx.Programs.Data.Queries
{

    /// <summary>
    /// Extensions of filters 
    /// TODO: PLEASE EVERYTIME INCLUDE .Active() query
    /// </summary>
    public static class ProgramFilterExtensions
    {        

        // selects Programs by filter, includ active
        public static IQueryable<Program> Filter(this IQueryable<Program> query, ProgramFilter filter)
        {
            query = query.Active();
            if (filter == null) return query;           

            if (!filter.IncludeUsed)
                query = query.Where(x => x.EntityId == null || x.EntityTypeId == null);
            if (filter.AllowedAccountsIds != null)
                query = query.Where(x => filter.AllowedAccountsIds.Contains(x.AccountId));

            return query;

        }

    }

}
