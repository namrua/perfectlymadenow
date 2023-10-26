using System.Collections.Generic;

namespace AutomationSystem.Main.Core.Reports.System.Models.CountryReport
{
    /// <summary>
    /// Encapsulates statistics informations about countries
    /// </summary>
    public class CountriesReportModel
    {
        public string ClassTitle { get; set; }
        public List<CountryReportItem> Countries { get; set; } = new List<CountryReportItem>();
    }
}
