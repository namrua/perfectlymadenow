using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Reports.AppLogic.Models
{
    /// <summary>
    /// Class report item
    /// </summary>
    public class ClassReportItem
    {
        public ClassReportType ReportType { get; set; }
        public string Name { get; set; }
        public FileTypeEnum FileTypeId { get; set; }
    }
}