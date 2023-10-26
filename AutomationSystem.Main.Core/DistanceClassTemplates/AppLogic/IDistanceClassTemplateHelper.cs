using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using AutomationSystem.Main.Model;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic
{
    public interface IDistanceClassTemplateHelper
    {
        HashSet<long> GetDistanceClassTemplatePersonIds(DistanceClassTemplate template);

        HashSet<long> GetDistanceClassTemplatePersonsIds(List<DistanceClassTemplate> templates);

        List<string> GetDistanceClassTemplateInstructorsWithGuestInstructor(DistanceClassTemplate template, IPersonHelper personHelper);

        List<string> GetDistanceClassTemplateInstructors(DistanceClassTemplate template, IPersonHelper personHelper);

        DistanceClassTemplateState GetDistanceClassTemplateState(DistanceClassTemplate template);
    }
}
