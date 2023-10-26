using System.Collections.Generic;
using System.IO;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic;
using AutomationSystem.Shared.Contract.ExcelConnector.Integration;

namespace AutomationSystem.Main.Core.BatchUploads.System.BatchDataFetchers
{
    public abstract class ExcelBlockDataFetcher : IBatchFileDataFetcher
    {
        protected readonly IExcelConnectorFactory excelConnectorFactory;
        private readonly int positionX;
        private readonly int positionY;
        private readonly int columnCount;

        protected ExcelBlockDataFetcher(
            IExcelConnectorFactory excelConnectorFactory,
            int positionX,
            int positionY,
            int columnCount)
        {
            this.excelConnectorFactory = excelConnectorFactory;
            this.positionX = positionX;
            this.positionY = positionY;
            this.columnCount = columnCount;
        }

        public abstract BatchUploadTypeEnum BatchUploadTypeId { get; }

        public FileTypeEnum BatchFileTypeId => FileTypeEnum.Excel;

        public List<string[]> FetchData(Stream stream)
        {
            var excelConnector = excelConnectorFactory.CreateExcelConnector();
            excelConnector.Initialize(stream);
            var result = excelConnector.GetBlockValues(positionX, positionY, columnCount);
            return result;
        }
    }
}
