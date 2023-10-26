using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.ManualReview
{
    /// <summary>
    /// Registration manual reviewing page model
    /// </summary>
    public class RegistrationManualReviewPageModel
    {
        public bool NeedsReview { get; set; }
        public bool IsReviewed { get; set; }
        public long ClassId { get; set; }
        public long ClassRegistrationId { get; set; }
        public ClassState ClassState { get; set; }
        public ReviewFormerClassFilter ReviewFormerClassFilter { get; set; }    
        public FormerStudentDetail FormerStudentDetail { get; set; }                // nullable
        public RegistrationLastClassDetail LastClassDetail { get; set; }            // nullable
        public bool CanPickOperation { get; set; }

        // access levels
        public bool CanPick { get; set; }
    }

}
