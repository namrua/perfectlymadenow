using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.Models;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic
{
    public interface IDistanceClassTemplateOperationChecker
    {
        bool IsOperationAllowed(DistanceClassTemplateOperation operation, DistanceClassTemplateState state);

        void CheckOperation(DistanceClassTemplateOperation distanceTemplateOperation, DistanceClassTemplateState state);
    }
}
