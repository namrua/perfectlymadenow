using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.ManualReview;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;

namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// Registration confirmation page model
    /// </summary>
    public class RegistrationConfirmationPageModel
    {
        public string DetailView { get; set; }      
        public ClassPublicDetail Class { get; set; }
        public BaseRegistrationDetail Registration { get; set; }
        public FormerStudentForReviewListItem FormerStudent { get; set; }
        public RegistrationLastClassDetail LastClass { get; set; }                          // NULLABLE
        public HomeWorkflowState WorkflowState { get; set; }

        // constructor
        public RegistrationConfirmationPageModel()
        {
            Class = new ClassPublicDetail();
            Registration = new BaseRegistrationDetail();
            FormerStudent = new FormerStudentForReviewListItem();
            WorkflowState = new HomeWorkflowState();
        }
    }
}