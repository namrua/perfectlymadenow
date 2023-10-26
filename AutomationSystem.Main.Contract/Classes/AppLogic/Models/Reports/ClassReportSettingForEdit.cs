using AutomationSystem.Main.Contract.Persons.AppLogic;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Reports
{
    /// <summary>
    /// Class report for edit
    /// </summary>
    public class ClassReportSettingForEdit
    {
        public ClassReportSettingForm Form { get; set; } = new ClassReportSettingForm();
        
        public IPersonHelper PersonHelper { get; set; } = new EmptyPersonHelper();
    }
}
