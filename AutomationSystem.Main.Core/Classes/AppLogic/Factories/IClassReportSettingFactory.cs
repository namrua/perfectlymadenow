using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Reports;

namespace AutomationSystem.Main.Core.Classes.AppLogic.Factories
{
    /// <summary>
    /// Creates ClassReportSetting objects
    /// </summary>
    public interface IClassReportSettingFactory
    {
        ClassReportSettingForEdit CreateClassReportSettingForEdit(long profileId);
    }
}
