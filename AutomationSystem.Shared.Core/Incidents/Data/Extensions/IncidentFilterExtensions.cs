using System.Linq;
using AutomationSystem.Shared.Contract.Incidents.AppLogic.Models;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;

namespace AutomationSystem.Shared.Core.Incidents.Data.Extensions
{
    /// <summary>
    /// Extensions of filters 
    /// TODO: PLEASE EVERYTIME INCLUDE .Active() query
    /// </summary>
    public static class IncidentFilterExtensions
    {
        // selects Incident entities by filter, includes restriction on Active items
        public static IQueryable<Incident> Filter(this IQueryable<Incident> query, IncidentFilter filter)
        {
            query = query.Active().Where(x => !x.IsHidden);

            if (filter == null)
            {
                query = query.Where(x => x.ParentIncidentId == null);
            }
            else
            {
                query = query.Where(x => x.ParentIncidentId == filter.ParentIncidentId);
                if (filter.ExcludeReported)
                    query = query.Where(x => !x.IsReported);
                if (!filter.IncludeResolved)
                    query = query.Where(x => !x.IsResolved);
                if (filter.EntityType.HasValue)
                    query = query.Where(x => x.EntityTypeId == filter.EntityType);
                if (filter.EntityId.HasValue)
                    query = query.Where(x => x.EntityId == filter.EntityId);
            }

            return query;
        }
    }
}
