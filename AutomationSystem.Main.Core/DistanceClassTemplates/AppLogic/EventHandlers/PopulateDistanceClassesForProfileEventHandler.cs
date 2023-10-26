using AutomationSystem.Main.Core.DistanceClassTemplates.System;
using AutomationSystem.Main.Core.DistanceProfiles.AppLogic.Models.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.EventHandlers
{
    public class PopulateDistanceClassesForProfileEventHandler : IEventHandler<DistanceProfileStatusChangedEvent>
    {
        private readonly IDistanceClassTemplateService service;

        public PopulateDistanceClassesForProfileEventHandler(IDistanceClassTemplateService service)
        {
            this.service = service;
        }

        public Result HandleEvent(DistanceProfileStatusChangedEvent evnt)
        {
            if (!evnt.IsActive)
            {
                return Result.Skipped($"Distance profile with id {evnt.DistanceProfileId} is deactivated.");
            }

            service.PopulateDistanceClassesForDistanceProfile(evnt.DistanceProfileId);
            return Result.Success($"Distance classes was created for distance profile with id {evnt.DistanceProfileId}.");
        }
    }
}
