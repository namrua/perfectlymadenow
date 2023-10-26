using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.BatchUploads.System.BatchDataFetchers;
using AutomationSystem.Shared.Contract.ExcelConnector.Integration;

namespace AutomationSystem.Main.Core.RegistrationUpload.AppLogic.BatchDataFetchers
{
    public class ExcelStudentRegistrationFetcher : ExcelBlockDataFetcher, IStudentRegistrationBatchDataFileFetcher
    {
        public override BatchUploadTypeEnum BatchUploadTypeId => BatchUploadTypeEnum.StudentRegistrationExcel;

        public ExcelStudentRegistrationFetcher(IExcelConnectorFactory excelConnectorFactory) : base(excelConnectorFactory, 4, 11, 10)
        {
        }
    }
}
