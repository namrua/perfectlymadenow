using AutomationSystem.Main.Core.Persons.AppLogic.Models.Events;
using AutomationSystem.Main.Core.Preferences.System;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Main.Core.Preferences.AppLogic.EventCheckers
{
    public class PersonToDeleteNotUsedInPreferences : IEventChecker<PersonDeletingEvent>
    {
        private readonly IMainPreferenceProvider mainPreferenceProvider;

        public PersonToDeleteNotUsedInPreferences(IMainPreferenceProvider mainPreferenceProvider)
        {
            this.mainPreferenceProvider = mainPreferenceProvider;
        }

        public bool CheckEvent(PersonDeletingEvent evnt)
        {
            var result = mainPreferenceProvider.GetMasterLeadInstructorId() != evnt.PersonId;
            return result;
        }
    }
}
