using System;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.System.Models
{
    public class DistanceClassTemplateCompletionResult
    {
        public long DistanceClassTemplateId { get; set; }
        public List<long> SkippedClasses { get; set; }  = new List<long>();
        public List<long> CompletedClasses { get; set; }  = new List<long>();

        public bool IsSuccess { get; set; }
        public Exception Exception { get; set; }
        public long? CorruptedClassId { get; set; }

        public DistanceClassTemplateCompletionResult(long distanceTemplateId)
        {
            DistanceClassTemplateId = distanceTemplateId;
        }
    }
}
