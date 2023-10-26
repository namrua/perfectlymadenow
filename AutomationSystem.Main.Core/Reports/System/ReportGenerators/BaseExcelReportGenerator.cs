using System.IO;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.Reports.System.Models.ReportService;
using AutomationSystem.Main.Core.Reports.System.SheetUtilityCellTextProcessors;
using AutomationSystem.Shared.Contract.Files.System.Models;
using SheetUtility;

namespace AutomationSystem.Main.Core.Reports.System.ReportGenerators
{
    /// <summary>
    /// Base excel report generator
    /// </summary>
    public abstract class BaseExcelReportGenerator : IClassReportGenerator
    {
        private const string underscoreCellTextProcessor = "UnderscoreCellTextProcessor";
        private const string excelFileExtension = ".xlsx";

        protected BaseExcelReportGenerator(ClassReportType reportType, string reservedCode, string fileNameBase)
        {
            TypeInfo = new ClassReportTypeInfo
            {
                ReportType = reportType,
                ReservedCode = reservedCode,
                FileNameBase = fileNameBase,
                FileExtension = excelFileExtension,
                FileTypeId = FileTypeEnum.Excel,
                MimeType = FileMimeType.Excel

            };
        }
        
        public ClassReportTypeInfo TypeInfo { get; }

        public abstract byte[] GenerateReport(IClassReportComponents components, string rootPath, string reportFolder);

        protected SheetCreator<byte[]> GetSheetCreator(string rootPath, string reportFolder, string definitionName)
        {
            // paths
            var reportPath = Path.Combine(rootPath, reportFolder);
            var definitionUri = Path.Combine(rootPath, reportFolder, "Definitions", definitionName);
            var pathToWorkingDir = Path.Combine(rootPath, "_Temporary");
            var pathXsd = Path.Combine(rootPath, "XmlDefinitionSchema.xsd");

            // initialization
            var reflectionDataLoader = new ReflectionDataLoader();
            var xmlDefinitionLoader = new XmlDefinitionLoader(definitionUri, pathXsd, reportPath);
            var memoryPersistor = new FileToByteArrayPersistor(new ExcelPersistor(pathToWorkingDir));
            var processorManager = new CellTextProcessorManager();
            processorManager.Subscribe(underscoreCellTextProcessor, new UnderscoreCellTextProcessor());

            var writer = new SheetCreator<byte[]>(reflectionDataLoader, xmlDefinitionLoader, memoryPersistor, processorManager);
            return writer;
        }
    }
}
