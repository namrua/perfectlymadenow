using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.FinancialForms
{
    /// <summary>
    /// General info for templates
    /// </summary>
    public class GeneralInfo
    {        

        [SheetField("EventName")]
        public string EventName { get; set; }

        [SheetField("EventDates")]
        public string EventDates { get; set; }       
        
        [SheetField("Coordinator")]
        public string Coordinator { get; set; }

        [SheetField("Instructors")]
        public string Instructors { get; set; }

        [SheetField("GuestInstructor")]
        public string GuestInstructor { get; set; }

        [SheetField("Address")]
        public string Address { get; set; }

        [SheetField("Phone")]
        public string Phone { get; set; }

        [SheetField("Email")]
        public string Email { get; set; }

        [SheetField("EventLocation")]
        public string EventLocation { get; set; }

        [SheetField("EventSite")]
        public string EventSite { get; set; }      

        [SheetField("ReportDate")]
        public string ReportDate { get; set; }  

    }

}
