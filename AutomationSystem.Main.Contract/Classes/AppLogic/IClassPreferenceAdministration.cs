using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances;
using System.IO;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Preferences;

namespace AutomationSystem.Main.Contract.Classes.AppLogic
{
    public interface IClassPreferenceAdministration
    {

        // gets class preference detail
        ClassPreferenceDetail GetClassPreferenceDetail(long profileId);


        // gets class preference for edit by profile id
        ClassPreferenceForEdit GetClassPreferenceForEditByProfileId(long profileId);

        // gets class preference for edit by form
        ClassPreferenceForEdit GetClassPreferenceForEditByForm(ClassPreferenceForm form);

        // saves class preferences
        void SaveClassPreference(ClassPreferenceForm form, Stream headerPictureContent, string headerPictureName);


        // gets expense layout for edit by profile id
        ExpensesLayoutForEdit GetExpenseLayoutForEditByProfileId(long profileId);

        // gets expense layout for edit by form
        ExpensesLayoutForEdit GetExpenseLayoutForEditByForm(ExpensesLayoutForm form);

        // saves expense layout
        void SaveExpenseLayout(ExpensesLayoutForm form);

    }
}
