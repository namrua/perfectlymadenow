using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using System.Linq;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.Data.Extensions
{
    public static class DistanceClassTemplateActiveExtensions
    {
        public static IQueryable<DistanceClassTemplatePerson> ActiveInActiveDistanceClassTemplate(this IQueryable<DistanceClassTemplatePerson> query)
        {
            return query.Active().Where(x => !x.DistanceClassTemplate.Deleted);
        }
    }
}
