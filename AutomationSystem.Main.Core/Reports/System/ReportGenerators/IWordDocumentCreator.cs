using AutomationSystem.Main.Core.Reports.System.Models.CountryReport;

namespace AutomationSystem.Main.Core.Reports.System.ReportGenerators
{
    /// <summary>
    /// Creates word documents
    /// </summary>
    public interface IWordDocumentCreator
    {
        byte[] GetCountriesReport(CountriesReportModel countriesReport);
    }
}
