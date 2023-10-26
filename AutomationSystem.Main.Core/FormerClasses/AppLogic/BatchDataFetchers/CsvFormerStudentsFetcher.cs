using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudentsBatchUploads;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic.BatchDataFetchers
{
    /// <summary>
    /// Fetch former student data from CVS batch file
    /// </summary>
    public class CsvFormerStudentsFetcher : IFormerStudentBatchFileDataFetcher
    {
        // gets batch file type id
        public FileTypeEnum BatchFileTypeId => FileTypeEnum.Csv;

        public BatchUploadTypeEnum BatchUploadTypeId => BatchUploadTypeEnum.FormerStudentCSV;

        // fetch former student data
        public List<string[]> FetchData(Stream stream)
        {
            var result = new List<string[]>();
            using (var sr = new StreamReader(stream))
            {
                var regex = new Regex(@"((\"".*?\"")|([^,]*))(?=,?)");
                var firstLine = true;
                string line;
                var lineNumber = -1;
                while ((line = sr.ReadLine()) != null)
                {
                    lineNumber++;
                    // skip first line
                    if (firstLine || string.IsNullOrWhiteSpace(line))
                    {
                        firstLine = false;
                        continue;
                    }

                    // gets items
                    var items = new List<string>();
                    foreach (var match in regex.Matches(line))
                    {                        
                        var item = ((Match)match).Value;
                        if (string.IsNullOrEmpty(item))
                            continue;
                        if (item.Length >= 2 && item[0] == '\"')
                            item = item.Substring(1, item.Length - 2);
                        items.Add(item);
                    }
                   
                    // adds result line
                    var itemsForResult = ConvertToResult(items, lineNumber);
                    result.Add(itemsForResult);
                }
                return result;
            }
        }


        // converts items to result array format
        private string[] ConvertToResult(List<string> items, int lineNumber)
        {
            // map items to result
            if (items.Count < CsvFormerStudentBatchColumn.MinCount)
                throw new ArgumentException($"Line {lineNumber} does not have enought items.");

            var result = new string[FormerStudentBatchColumn.RowLength];
            result[FormerStudentBatchColumn.FirstName] = items[CsvFormerStudentBatchColumn.FirstName];
            result[FormerStudentBatchColumn.LastName] = items[CsvFormerStudentBatchColumn.LastName];
            result[FormerStudentBatchColumn.Street] = items[CsvFormerStudentBatchColumn.Street];
            result[FormerStudentBatchColumn.Street2] = items[CsvFormerStudentBatchColumn.Street2];
            result[FormerStudentBatchColumn.City] = items[CsvFormerStudentBatchColumn.City];
            result[FormerStudentBatchColumn.State] = items[CsvFormerStudentBatchColumn.State];
            result[FormerStudentBatchColumn.ZipCode] = items[CsvFormerStudentBatchColumn.ZipCode];
            result[FormerStudentBatchColumn.Country] = items[CsvFormerStudentBatchColumn.Country];
            result[FormerStudentBatchColumn.Email] = items[CsvFormerStudentBatchColumn.Email];

            return result;
        }
    }
}
