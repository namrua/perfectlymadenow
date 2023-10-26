using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.Crf
{
    public class CrfEventLocationInfo
    {
        [SheetField("EventName")]
        public string EventName { get; set; }

        [SheetField("EventLocation")]
        public string EventLocation { get; set; }
    }
}
