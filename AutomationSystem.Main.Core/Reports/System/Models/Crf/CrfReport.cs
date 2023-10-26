using System.Collections.Generic;
using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.Crf
{
    /// <summary>
    /// CRF report info
    /// </summary>
    public class CrfReport
    {
        // titles
        [SheetGroup("TableTitles")]
        public CrfTableTitles TableTitles { get; set; }

        // header informations 
        [SheetGroup("EventInfo")]
        public CrfEventInfo EventInfo { get; set; }

        [SheetGroup("LocationInfo")]
        public CrfLocationInfo LocationInfo { get; set; }

        [SheetTable("EventsLocationInfo")]
        public List<CrfEventLocationInfo> EventsLocationInfo { get; set; }

        [SheetGroup("ClassNumbers")]
        public CrfClassNumbers ClassNumbers { get; set; }

        [SheetGroup("FinancialTotals")]
        public CrfFinancialTotals FinancialTotals { get; set; }

        // content
        [SheetTable("ApprovedGuests")]
        public List<CrfPersonInfo> ApprovedGuest { get; set; }

        [SheetTable("Instructors")]
        public List<CrfPersonInfo> Instructors { get; set; }

        [SheetTable("ApprovedStaff")]
        public List<CrfPersonInfo> ApprovedStaff { get; set; }

        [SheetTable("PaidStudents")]
        public List<CrfPersonInfo> PaidStudents { get; set; }

        [SheetTable("CanceledStudents")]
        public List<CrfPersonInfo> CanceledStudents { get; set; }

        // constructor
        public CrfReport()
        {
            EventInfo = new CrfEventInfo();
            LocationInfo = new CrfLocationInfo();
            ClassNumbers = new CrfClassNumbers();
            FinancialTotals = new CrfFinancialTotals();
            ApprovedGuest = new List<CrfPersonInfo>();
            ApprovedStaff = new List<CrfPersonInfo>();
            Instructors = new List<CrfPersonInfo>();
            PaidStudents = new List<CrfPersonInfo>();
            CanceledStudents = new List<CrfPersonInfo>();
            EventsLocationInfo = new List<CrfEventLocationInfo>();
        }

    }
}
