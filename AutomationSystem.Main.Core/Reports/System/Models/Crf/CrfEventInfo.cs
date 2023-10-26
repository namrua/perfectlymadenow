using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.Crf
{
    /// <summary>
    /// Event info
    /// </summary>
    public class CrfEventInfo
    {

        [SheetField("EventName")]
        public string EventName { get; set; }

        [SheetField("EventLocation")]
        public string EventLocation { get; set; }

        [SheetField("EventDate")]
        public string EventDate { get; set; }

        [SheetField("Coordinators")]
        public string Coordinators { get; set; }

        [SheetField("InstructorsFirstCell")]
        public string InstructorsFirstCell { get; set; }

        [SheetField("InstructorsSecondCell")]
        public string InstructorsSecondCell { get; set; }

        [SheetField("InstructorsThirdCell")]
        public string InstructorsThirdCell { get; set; }
    }
}
