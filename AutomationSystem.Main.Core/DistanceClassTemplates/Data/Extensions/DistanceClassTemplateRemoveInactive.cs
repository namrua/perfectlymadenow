using AutomationSystem.Main.Core.DistanceClassTemplates.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using System.Linq;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.Data.Extensions
{
    public static class DistanceClassTemplateRemoveInactive
    {
        #region remove inactive
        public static DistanceClassTemplate RemoveInactiveForDistanceClassTemplate(DistanceClassTemplate entity, DistanceClassTemplateIncludes includes)
        {
            if (entity == null)
            {
                return null;
            }

            if (includes.HasFlag(DistanceClassTemplateIncludes.DistanceClassTemplatePersons))
            {
                entity.DistanceClassTemplatePersons = entity.DistanceClassTemplatePersons.AsQueryable().Active().ToList();
            }

            if (includes.HasFlag(DistanceClassTemplateIncludes.DistanceClassTemplateClasses))
            {
                entity.DistanceClassTemplateClasses = entity.DistanceClassTemplateClasses.AsQueryable().Active().ToList();
            }

            return entity;
        }
        #endregion
    }
}
