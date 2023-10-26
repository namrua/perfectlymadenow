using AutomationSystem.Main.Core.Reports.System.Models.CountryReport;
using AutomationSystem.Main.Core.Reports.System.Models.Crf;
using AutomationSystem.Main.Core.Reports.System.Models.WwaCrf;

namespace AutomationSystem.Main.Core.Reports.System.DataProviders
{
    /// <summary>
    /// Common reports data provider
    /// </summary>
    public interface ICommonReportDataProvider
    {
        CrfReport GetCrfReportModel();

        WwaCrfReport GetWwaReportModel();             

        CountriesReportModel GetCountriesReport();

        string GetRegistrationListText();
    }
}