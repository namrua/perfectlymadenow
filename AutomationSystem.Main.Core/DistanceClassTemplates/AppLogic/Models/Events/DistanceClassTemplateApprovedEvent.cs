using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.Models.Events
{
    public class DistanceClassTemplateApprovedEvent : BaseEvent
    {
        public long DistanceClassTemplateId { get; }

        public DistanceClassTemplateApprovedEvent(long distanceClassTemplateId)
        {
            DistanceClassTemplateId = distanceClassTemplateId;
        }

        public override string ToString()
        {
            return $"DistanceClassTemplateId: {DistanceClassTemplateId}";
        }
    }
}
