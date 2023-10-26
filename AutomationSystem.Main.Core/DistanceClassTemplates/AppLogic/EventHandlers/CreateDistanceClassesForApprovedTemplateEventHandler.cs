using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.Models.Events;
using AutomationSystem.Main.Core.DistanceClassTemplates.System;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.EventHandlers
{
    public class CreateDistanceClassesForApprovedTemplateEventHandler : IEventHandler<DistanceClassTemplateApprovedEvent>
    {
        private readonly IDistanceClassTemplateService service;

        public CreateDistanceClassesForApprovedTemplateEventHandler(IDistanceClassTemplateService service)
        {
            this.service = service;
        }

        public Result HandleEvent(DistanceClassTemplateApprovedEvent evnt)
        {
            service.PopulateDistanceClassesForDistanceTemplate(evnt.DistanceClassTemplateId);
            return Result.Success($"Distance classes were created for distance template with id {evnt.DistanceClassTemplateId}.");
        }
    }
}
