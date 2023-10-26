using System;
using System.Collections.Generic;
using CorabeuControl.ModelMetadata;
using System.ComponentModel;
using System.Web.Mvc;

namespace AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base
{
    [Bind(Include = "TemplateState")]
    public class DistanceClassTemplateFilter
    {
        [DisplayName("Template state")]
        [PickInputOptions(NoItemText = "Open templates", Placeholder = "Open templates")]
        public DistanceClassTemplateState? TemplateState { get; set; }

        public List<long> ExcludeIds { get; set; } 

        public DateTime? FromRegistrationEnd { get; set; }
        public DateTime? ToAutomationCompleteTime { get; set; }
    }
}
