using System.Linq;
using PerfectlyMadeInc.WebEx.Model;
using PerfectlyMadeInc.WebEx.Model.Queries;
using PerfectlyMadeInc.WebEx.Programs.Data.Models;

namespace PerfectlyMadeInc.WebEx.Programs.Data.Queries
{
    public static class ProgramRemoveInactive
    {
        // removes inactive includes for Program
        public static Program RemoveInactiveForPrograms(Program entity, ProgramIncludes includes)
        {
            if (entity == null)
                return null;
            if (includes.HasFlag(ProgramIncludes.Events))
                entity.Events = entity.Events.AsQueryable().Active().ToList();
            return entity;
        }
    }
}
