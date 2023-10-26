using System.Text.RegularExpressions;
using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.SheetUtilityCellTextProcessors
{
    /// <summary>
    /// Underscore cell text processor
    /// </summary>
    public class UnderscoreCellTextProcessor : ICellTextProcessor
    {
        public object GetProcessedText(object data, string templateText)
        {
            string pattern = "_+";
            var dataWithSpace = $" {data.ToString()}";
            var result = Regex.Replace(templateText, pattern, dataWithSpace);

            return result;
        }
    }
}
