using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.Reports.System.Models.ReportService;
using AutomationSystem.Shared.Contract.Files.System.Models;

namespace AutomationSystem.Main.Core.Reports.System.ReportGenerators
{
    /// <summary>
    /// Base Word report generator
    /// </summary>
    public abstract class BaseWordReportGenerator : IClassReportGenerator
    {
        private const string wordFileExtension = ".docx";

        protected readonly IWordDocumentCreator wordDocumentCreator;

        protected BaseWordReportGenerator(ClassReportType reportType, string reservedCode, string fileNameBase)
        {
            TypeInfo = new ClassReportTypeInfo
            {
                ReportType = reportType,
                ReservedCode = reservedCode,
                FileNameBase = fileNameBase,
                FileExtension = wordFileExtension,
                FileTypeId = FileTypeEnum.Word,
                MimeType = FileMimeType.Word
            };

            wordDocumentCreator = new WordDocumentCreator();
        }

        public ClassReportTypeInfo TypeInfo { get; }

        public abstract byte[] GenerateReport(IClassReportComponents components, string rootPath, string reportFolder);
    }
}
