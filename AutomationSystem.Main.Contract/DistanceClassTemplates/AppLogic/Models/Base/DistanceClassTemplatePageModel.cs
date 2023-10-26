using System.Collections.Generic;

namespace AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base
{
    public class DistanceClassTemplatePageModel
    {
        public DistanceClassTemplateFilter Filter { get; set; }
        public List<DistanceClassTemplateListItem> Items { get; set; } = new List<DistanceClassTemplateListItem>();
        public List<DistanceClassTemplateState> TemplateStates { get; set; } = new List<DistanceClassTemplateState>();
        public bool WasSearched { get; set; }

        public DistanceClassTemplatePageModel(DistanceClassTemplateFilter filter = null)
        {
            Filter = filter ?? new DistanceClassTemplateFilter();
        }
    }
}
