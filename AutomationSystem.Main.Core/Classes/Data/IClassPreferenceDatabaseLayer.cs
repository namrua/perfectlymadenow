using AutomationSystem.Main.Model;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.Classes.Data
{
    public interface IClassPreferenceDatabaseLayer
    {
        // updates ClassPreference of the Profile
        void UpdateClassPreference(long profileId, ClassPreference classPreference, bool updateHeaderPictureId);

        // updates ClassPreference's expenses
        void UpdateClassPreferenceExpenses(long profileId, List<ClassPreferenceExpense> profileExpenses);

        // detemrmines whether person can be deleted
        bool CanDeletePerson(long personId);
    }
}
