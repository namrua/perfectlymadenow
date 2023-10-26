namespace AutomationSystem.Main.Contract.Reports.AppLogic.Models
{
    /// <summary>
    /// Report constants
    /// </summary>
    public static class ReportConstants
    {
        // paths
        public const string ReportRootPath = "~/App_Data/Reports";
        public const string ReportRootPathForJob = @"App_Data\Reports";

        // file names - class
        public const string CrfClass = "IZI CRF CLASS";
        public const string CrfLecture = "IZI CRF LECTURE";
        public const string CrfWwaClass = "IZI CRF WWA CLASS";

        public const string GuestInstructorClosingStatement = "IZI LLC Guest Instructor Closing Statement";
        public const string FoiRoyaltyForm = "FOI Royalty Form - US & Canada";
        public const string FaClosingStatement = "IZI LLC FA Closing Statement - USA";       

        public const string CountriesReport = "Countries Report";

        // email parameters
        public const string RegistrationListParameter = "RegistrationList";
    }
}