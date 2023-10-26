using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.Data
{
    public class DistanceClassTemplateClassDatabaseLayer : IDistanceClassTemplateClassDatabaseLayer
    {
        public List<long> GetFilledDistanceTemplateIdsForDistanceProfileId(long distanceProfileId, DateTime fromRegistrationEnd)
        {
            using (var context = new MainEntities())
            {
                var result = context.DistanceClassTemplateClasses.Active()
                    .Where(x => x.DistanceProfileId == distanceProfileId && fromRegistrationEnd <= x.DistanceClassTemplate.RegistrationEnd)
                    .Select(x => x.DistanceClassTemplateId).ToList();
                return result;
            }
        }

        public List<long> GetFilledDistanceProfileIdsForDistanceClassTemplateId(long distanceClassTemplateId)
        {
            using (var context = new MainEntities())
            {
                var result = context.DistanceClassTemplateClasses.Active()
                    .Where(x => x.DistanceClassTemplateId == distanceClassTemplateId)
                    .Select(x => x.DistanceProfileId).ToList();
                return result;
            }
        }

        public List<long> GetFilledClassIdsByDistanceClassTemplateId(long distanceClassTemplateId)
        {
            using (var context = new MainEntities())
            {
                var result = context.DistanceClassTemplateClasses.Active()
                    .Where(x => x.DistanceClassTemplateId == distanceClassTemplateId && !x.Class.Deleted)
                    .Select(x => x.ClassId).ToList();
                return result;
            }
        }

        public void InsertDistanceClassTemplateClass(long distanceTemplateId, long distanceProfileId, long classId)
        {
            using (var context = new MainEntities())
            {
                var distanceClassTemplateClass = new DistanceClassTemplateClass
                {
                    DistanceClassTemplateId = distanceTemplateId,
                    DistanceProfileId = distanceProfileId,
                    ClassId = classId
                };
                context.DistanceClassTemplateClasses.Add(distanceClassTemplateClass);
                context.SaveChanges();
            }
        }
    }
}
