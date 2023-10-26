using System.Data.Entity.Infrastructure;
using PerfectlyMadeInc.WebEx.Model;
using PerfectlyMadeInc.WebEx.Programs.Data.Models;

namespace PerfectlyMadeInc.WebEx.Programs.Data.Queries
{
    public static class ProgramIncludeExtensions
    {   
        // adds includes for Program
        public static DbQuery<Program> AddIncludes(this DbQuery<Program> query, ProgramIncludes includes)
        {
            if (includes.HasFlag(ProgramIncludes.Events))
                query = query.Include("Events");
            return query;
        }
    }

}
