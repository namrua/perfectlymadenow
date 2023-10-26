using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.Crf
{
    /// <summary>
    /// Class numbers info
    /// </summary>
    public class CrfClassNumbers
    {
        [SheetField("NewStudents")]
        public int NewStudents { get; set; }

        [SheetField("ReviewStudents")]
        public int ReviewStudents { get; set; }

        [SheetField("ApprovedGuests")]
        public int ApprovedGuests { get; set; }

        [SheetField("ApprovedStaff")]
        public int ApprovedStaff { get; set; }

        [SheetField("TotalInRoom")]
        public int TotalRoom { get; set; }
    }
}
