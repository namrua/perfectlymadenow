using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.WwaCrf
{
    /// <summary>
    /// WWA report event info
    /// </summary>
    public class WwaCrfEventInfo
    {
        [SheetField("EventName")]
        public string EventName { get; set; }

        [SheetField("EventLocation")]
        public string EventLocation { get; set; }

        [SheetField("EventDate")]
        public string EventDate { get; set; }

        [SheetField("ClassCoordinator")]
        public string ClassCoordinator { get; set; }

        [SheetField("CoordinatorId")]
        public string CoordinatorId { get; set; }

        [SheetField("Instructors")]
        public string Instructors { get; set; }

        [SheetField("TotalAbsentee")]
        public int TotalAbsentee { get; set; }

    }
}
