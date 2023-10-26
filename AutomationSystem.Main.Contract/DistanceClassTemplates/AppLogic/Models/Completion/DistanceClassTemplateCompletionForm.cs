using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Completion
{
    public class DistanceClassTemplateCompletionForm
    {
        [HiddenInput]
        public long DistanceClassTemplateId { get; set; }

        [DisplayName("Time of automation completion")]
        public DateTime? AutomationCompleteTime { get; set; }
    }
}
