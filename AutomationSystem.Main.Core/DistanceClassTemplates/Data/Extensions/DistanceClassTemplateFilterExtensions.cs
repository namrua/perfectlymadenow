using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using System.Linq;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.Data.Extensions
{
    public static class DistanceClassTemplateFilterExtensions
    {
        // select DistanceClassTemplates by filter
        public static IQueryable<DistanceClassTemplate> Filter(this IQueryable<DistanceClassTemplate> query, DistanceClassTemplateFilter filter)
        {
            query = query.Active();
            if (filter == null)
            {
                return query;
            }

            if (filter.TemplateState.HasValue)
            {
                query = query.FilterState(filter.TemplateState.Value);
            }
            else
            {
                query = query.Where(x => !x.IsCompleted);
            }

            if (filter.ExcludeIds != null)
            {
                query = query.Where(x => !filter.ExcludeIds.Contains(x.DistanceClassTemplateId));
            }

            if (filter.FromRegistrationEnd.HasValue)
            {
                query = query.Where(x => filter.FromRegistrationEnd <= x.RegistrationEnd);
            }

            if (filter.ToAutomationCompleteTime.HasValue)
            {
                query = query.Where(x => x.AutomationCompleteTime.HasValue && x.AutomationCompleteTime < filter.ToAutomationCompleteTime);
            }

            return query;
        }


        // filters distance class template state
        public static IQueryable<DistanceClassTemplate> FilterState(this IQueryable<DistanceClassTemplate> query, DistanceClassTemplateState templateState)
        {
            switch (templateState)
            {
                case DistanceClassTemplateState.New:
                    query = query.Where(x => !x.IsApproved && !x.IsCompleted);
                    break;
                case DistanceClassTemplateState.Approved:
                    query = query.Where(x => !x.IsCompleted && x.IsApproved);
                    break;
                case DistanceClassTemplateState.Completed:
                    query = query.Where(x => x.IsCompleted);
                    break;
            }

            return query;
        }

        
    }
}
