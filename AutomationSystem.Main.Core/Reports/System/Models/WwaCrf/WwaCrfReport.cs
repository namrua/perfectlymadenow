using System.Collections.Generic;
using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.WwaCrf
{
    /// <summary>
    /// WWA report model
    /// </summary>
    public class WwaCrfReport
    {
        // groups 
        [SheetGroup("EventInfo")]
        public WwaCrfEventInfo EventInfo { get; set; }

        // tables
        [SheetTable("Students")]
        public List<WwaCrfPersonsInfo> Students { get; set; }

        // canceled
        [SheetTable("CanceledStudents")]
        public List<WwaCrfPersonsInfo> CanceledStudents { get; set; }

        // constructor
        public WwaCrfReport()
        {
            EventInfo = new WwaCrfEventInfo();
            Students = new List<WwaCrfPersonsInfo>();
            CanceledStudents = new List<WwaCrfPersonsInfo>();
        }
    }
}
