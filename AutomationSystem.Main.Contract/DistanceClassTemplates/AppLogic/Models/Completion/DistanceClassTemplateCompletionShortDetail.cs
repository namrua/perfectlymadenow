using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using System.ComponentModel;

namespace AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Completion
{
    public class DistanceClassTemplateCompletionShortDetail
    {
        [DisplayName("ID")]
        public long DistanceClassTemplateId { get; set; }

        [DisplayName("Template state")]
        public DistanceClassTemplateState TemplateState { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }
    }
}
