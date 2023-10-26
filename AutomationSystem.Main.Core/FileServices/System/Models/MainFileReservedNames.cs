using System.Collections.Generic;
using System.Linq;

namespace AutomationSystem.Main.Core.FileServices.System.Models
{
    /// <summary>
    /// Main file reserved names
    /// </summary>
    public static class MainFileReservedNames
    {

        // certificates
        public const string Certificate = "Certificate";
        public const string MultiCertificate = "MultiCertificate";

        // reports
        public const string FinancialCrfClass = "FinancialCRF";
        public const string FinancialCrfLecture = "CRFLecture";
        public const string FinancialWwa = "FinancialWWA";
        public const string GuestInstructorStatement = "GuestInstructorStatement";
        public const string FoiRoyaltyForm = "FoiRoyaltyForm";
        public const string FaClosingStatement = "FaClosingStatement";
        public const string RevenueDepositForm = "RevenueDepositForm";
        public const string CountriesReport = "CountriesReport";


        #region static macros

        // categories of file names
        public static HashSet<string> certificateExactMatch = new HashSet<string> { Certificate, MultiCertificate };
        public static HashSet<string> reportExactMatch = new HashSet<string> { FinancialCrfClass, FinancialCrfLecture, FinancialWwa,
            GuestInstructorStatement, FoiRoyaltyForm, FaClosingStatement, RevenueDepositForm, CountriesReport };

        public static string[] certificateStartsWith = { $"{Certificate}-" };


        // determines whether code is certificate related
        public static bool IsCertificateCode(string code)
        {
            if (certificateExactMatch.Contains(code))
                return true;
            var result = certificateStartsWith.Any(code.StartsWith);
            return result;
        }

        // determines whether code is report code
        public static bool IsReportCode(string code)
        {
            var result = reportExactMatch.Contains(code);
            return result;
        }

        #endregion

    }

}
