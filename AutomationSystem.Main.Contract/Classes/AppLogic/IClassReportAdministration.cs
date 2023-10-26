using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Reports;

namespace AutomationSystem.Main.Contract.Classes.AppLogic
{
    public interface IClassReportAdministration
    {
        #region report settings

        ClassReportSettingForEdit GetClassReportSettingForEditByClassId(long classId);

        ClassReportSettingForEdit GetClassReportSettingForEditByForm(ClassReportSettingForm form);

        void SaveClassReportSetting(ClassReportSettingForm form);

        #endregion

        #region reports

        ClassReportsPageModel GetClassReportsPageModel(long classId);

        long GenerateFinancialForms(long classId);

        ClassRecipientSelectionForEdit GetClassRecipientSelectionForEdit(long classId, ClassCommunicationType type);

        void SendMessageToRecipients(ClassRecipientSelectionForm form);

        void SendRegistrationListToMasterCoordinator(string rootPath, long classId);

        #endregion
    }
}
