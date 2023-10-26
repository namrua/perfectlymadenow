using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.BatchUploads.System.BatchDataFetchers;
using AutomationSystem.Shared.Contract.ExcelConnector.Integration;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic.BatchDataFetchers
{

    /// <summary>
    /// Batch file fetcher for excel
    /// </summary>
    public class ExcelFormerStudentsFetcher : ExcelBlockDataFetcher, IFormerStudentBatchFileDataFetcher
    {
        public override BatchUploadTypeEnum BatchUploadTypeId => BatchUploadTypeEnum.FormerStudentExcel;

        public ExcelFormerStudentsFetcher(IExcelConnectorFactory excelConnectorFactory) : base(excelConnectorFactory, 4, 11, 10)
        {
        }

    }

}
