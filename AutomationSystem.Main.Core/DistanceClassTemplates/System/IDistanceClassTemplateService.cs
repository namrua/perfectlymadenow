using AutomationSystem.Main.Core.DistanceClassTemplates.System.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.System
{
    public interface IDistanceClassTemplateService
    {
        void PopulateDistanceClassesForDistanceProfile(long distanceProfileId);

        void PopulateDistanceClassesForDistanceTemplate(long distanceClassTemplateId);

        void PropagateChangesToDistanceClasses(DistanceClassTemplate distanceTemplate);

        DistanceClassTemplateCompletionResult CompleteDistanceClassTemplate(long distanceTemplateId, string certificateRootPath);
    }
}
