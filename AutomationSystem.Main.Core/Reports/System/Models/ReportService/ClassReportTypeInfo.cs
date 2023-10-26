using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;

namespace AutomationSystem.Main.Core.Reports.System.Models.ReportService
{
    /// <summary>
    /// Encapsulates class report type information
    /// </summary>
    public class ClassReportTypeInfo
    {
        public ClassReportType ReportType { get; set; }
        public string ReservedCode { get; set; }                // reserved code of report - used for unique identification of file in DB     
        public string FileNameBase { get; set; }
        public string FileExtension { get; set; }
        public FileTypeEnum FileTypeId { get; set; }
        public string MimeType { get; set; }
    }
}