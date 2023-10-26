using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.Models.Events;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data.Models;
using AutomationSystem.Main.Core.DistanceClassTemplates.System;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.EventHandlers
{
    public class ChangesPropagatedToDistanceClassesEventHandler : IEventHandler<DistanceClassTemplateChangedEvent>
    {
        private readonly IDistanceClassTemplateService service;
        private readonly IDistanceClassTemplateDatabaseLayer templateDb;
        private readonly IDistanceClassTemplateHelper helper;

        public ChangesPropagatedToDistanceClassesEventHandler(
            IDistanceClassTemplateService service,
            IDistanceClassTemplateDatabaseLayer templateDb,
            IDistanceClassTemplateHelper helper)
        {
            this.service = service;
            this.templateDb = templateDb;
            this.helper = helper;
        }

        public Result HandleEvent(DistanceClassTemplateChangedEvent evnt)
        {
            var template = templateDb.GetDistanceClassTemplateById(evnt.DistanceClassTemplateId, DistanceClassTemplateIncludes.DistanceClassTemplatePersons);
            if (helper.GetDistanceClassTemplateState(template) != DistanceClassTemplateState.Approved)
            {
                return Result.Skipped($"Distance class template with id {evnt.DistanceClassTemplateId} is not approved.");
            }

            service.PropagateChangesToDistanceClasses(template);
            return Result.Success("Distance classes has been updated.");
        }
    }
}
