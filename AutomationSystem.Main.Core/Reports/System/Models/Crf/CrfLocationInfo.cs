using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.Crf
{
    /// <summary>
    /// Location info
    /// </summary>
    public class CrfLocationInfo
    {
        [SheetField("VenueName")]
        public string VenueName { get; set; }

        [SheetField("ContactNames")]
        public string ContactNames { get; set; }

        [SheetField("Address")]
        public string Address { get; set; }

        [SheetField("ContactPhone")]
        public string ContactPhone { get; set; }
    }
}
