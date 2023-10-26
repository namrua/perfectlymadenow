using AutomationSystem.Main.Core.DistanceClassTemplates.Data;
using AutomationSystem.Main.Core.Persons.AppLogic.Models.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.EventCheckers
{
    public class PersonToDeleteHasNoDistanceClassTemplate : IEventChecker<PersonDeletingEvent>
    {
        public IDistanceClassTemplateDatabaseLayer templateDb;

        public PersonToDeleteHasNoDistanceClassTemplate(IDistanceClassTemplateDatabaseLayer templateDb)
        {
            this.templateDb = templateDb;
        }

        public bool CheckEvent(PersonDeletingEvent evnt)
        {
            var result = !templateDb.PersonOnAnyDistanceClassTemplate(evnt.PersonId);
            return result;
        }
    }
}
