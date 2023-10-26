using AutomationSystem.Main.Core.DistanceClassTemplates.Data.Models;
using AutomationSystem.Main.Model;
using System.Data.Entity.Infrastructure;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.Data.Extensions
{
    public static class DistanceClassTemplateIncludeExtensions
    {
        public static DbQuery<DistanceClassTemplate> AddIncludes(this DbQuery<DistanceClassTemplate> query, DistanceClassTemplateIncludes includes)
        {
            if (includes.HasFlag(DistanceClassTemplateIncludes.ClassType))
            {
                query = query.Include("ClassType");
            }

            if (includes.HasFlag(DistanceClassTemplateIncludes.DistanceClassTemplatePersons))
            {
                query = query.Include("DistanceClassTemplatePersons");
            }

            if (includes.HasFlag(DistanceClassTemplateIncludes.DistanceClassTemplateClasses))
            {
                query = query.Include("DistanceClassTemplateClasses");
            }

            return query;
        }
    }
}
