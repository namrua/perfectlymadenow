using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Completion
{
    public class DistanceClassTemplateCompletionPageModel
    {
        [DisplayName("Automation completion time")]
        public DateTime? AutomationCompleteTime { get; set; }
        
        [DisplayName("Completed")]
        public DateTime? Completed { get; set; }

        public bool CanComplete { get; set; }

        public DistanceClassTemplateCompletionShortDetail ShortDetail { get; set; } = new DistanceClassTemplateCompletionShortDetail();

        public List<AsyncRequestDetail> GeneratingRequests { get; set; } = new List<AsyncRequestDetail>();
    }
}
