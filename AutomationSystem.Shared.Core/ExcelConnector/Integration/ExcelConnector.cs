using System.Collections.Generic;
using System.IO;
using AutomationSystem.Shared.Contract.ExcelConnector.Integration;
using ClosedXML.Excel;

namespace AutomationSystem.Shared.Core.ExcelConnector.Integration
{
    /// <summary>
    /// Manages operations with excel
    /// </summary>
    public class ExcelConnector : IExcelConnector
    {

        // private fields
        private IXLWorkbook xlWorkbook;
        private IXLWorksheet xlWorksheet;
       

        // initialize excel file
        public void Initialize(Stream stream)
        {
            xlWorkbook = new XLWorkbook(stream);
            xlWorksheet = xlWorkbook.Worksheet(1);
        }

        // Gets cell value from sheet
        public string GetCellValue(int positionX, int positionY)
        {
            string result = null;
            if (xlWorksheet != null)
                result = xlWorksheet.Cell(positionY, positionX).Value.ToString();
            return result;
        }


        // gets block values from sheet
        public List<string[]> GetBlockValues(int positionX, int positionY, int columnCount)
        {
            var result = new List<string[]>();
            if (xlWorksheet == null)
                return result;

            var currentPosition = positionY;
            var isEmpty = false;
            while (!isEmpty)
            {
                var line = ReadLine(positionX, currentPosition++, columnCount);
                if (CheckForLastLine(line))
                    isEmpty = true;
                else
                    result.Add(line.ToArray());
            }
            return result;
        }
       


        #region private methods
              

        // read one line
        private List<string> ReadLine(int positionX, int positionY, int columnCount)
        {
            var line = new List<string>();
            var endPosition = positionX + columnCount - 1;
            for (var i = positionX; i <= endPosition; i++)
            {
                var cellValue = GetCellValue(i, positionY);
                line.Add(cellValue);
            }            
            return line;
        }

        // checks if line is empty
        private bool CheckForLastLine(List<string> list)
        {
            var result = list.TrueForAll(string.IsNullOrWhiteSpace);
            return result;
        }

        #endregion

    }

}
