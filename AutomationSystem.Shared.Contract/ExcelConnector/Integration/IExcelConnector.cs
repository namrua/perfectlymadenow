using System.Collections.Generic;
using System.IO;

namespace AutomationSystem.Shared.Contract.ExcelConnector.Integration
{

    /// <summary>
    /// Provides connection to excel files
    /// </summary>
    public interface IExcelConnector
    {

        // initialize excel file
        void Initialize(Stream stream);

        // gets cell value from sheet
        string GetCellValue(int positionX, int positionY);

        // gets block values from sheet
        List<string[]> GetBlockValues(int positionX, int positionY, int columnCount);

    }

}
