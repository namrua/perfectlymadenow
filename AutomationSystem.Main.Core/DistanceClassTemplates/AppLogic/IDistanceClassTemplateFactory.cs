using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic
{
    public interface IDistanceClassTemplateFactory
    {
        DistanceClassTemplateForEdit CreateDistanceClassTemplateForEdit();
    }
}
