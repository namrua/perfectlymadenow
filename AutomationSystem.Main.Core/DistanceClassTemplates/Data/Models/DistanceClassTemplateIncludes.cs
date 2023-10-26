using System;
namespace AutomationSystem.Main.Core.DistanceClassTemplates.Data.Models
{
    [Flags]
    public enum DistanceClassTemplateIncludes
    {
        None = 0,
        ClassType = 1 << 0,
        DistanceClassTemplatePersons = 1 << 1,
        DistanceClassTemplateClasses = 1 << 2
    }
}
