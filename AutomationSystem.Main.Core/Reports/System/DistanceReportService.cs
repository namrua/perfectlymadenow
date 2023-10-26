using System.IO;
using AutomationSystem.Main.Core.Reports.System.DataProviders;
using AutomationSystem.Main.Core.Reports.System.Models.DistanceReportService;
using AutomationSystem.Main.Core.Reports.System.SheetUtilityCellTextProcessors;
using AutomationSystem.Shared.Contract.Files.System.Models;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using SheetUtility;

namespace AutomationSystem.Main.Core.Reports.System
{
    /// <summary>
    /// Provides report services for distance classes
    /// </summary>
    public class DistanceReportService : IDistanceReportService
    {
        // todo: separate to dedicated const classes
        private const string underscoreCellTextProcessor = "UnderscoreCellTextProcessor";
        private const string distanceCrfDefinition = "DistanceCrfReport.xml";
        private const string distanceCrfFileName = "IZI LLC WWA CRF - Distance Coordinator";
        private const string excelFileExtension = ".xlsx";                                              // todo: refactor and unify with BaseExcelReportGenerator

        private readonly IDistanceReportDataProviderFactory distanceReportServiceFactory;

        public DistanceReportService(IDistanceReportDataProviderFactory distanceReportServiceFactory)
        {
            this.distanceReportServiceFactory = distanceReportServiceFactory;
        }

        public FileForDownload GenerateWwaCrfReport(string rootPath, DistanceCrfReportParameters parameters)
        {
            // initializes data provider
            var provider = distanceReportServiceFactory.CreateDistanceReportDataProvider(parameters);

            // gets report model and generates report
            var model = provider.GetDistanceCrfReportModel();
            var writer = GetSheetCreator(rootPath, LocalisationInfo.DefaultCurrencyCode, distanceCrfDefinition);
            var content = writer.CreateSheet(model);

            // assembles file to download
            var result = new FileForDownload
            {
                Content = content,
                FileName = distanceCrfFileName + excelFileExtension,
                MimeType = FileMimeType.Excel
            };
            return result;
        }

        #region private methods

        // todo: refactor and unify with BaseExcelReportGenerator
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

        #endregion
    }
}
