using System.Collections.Generic;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.ManualReview;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;

namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// Page model for selection of former students for reviewing
    /// </summary>
    public class FormerStudentSelectionPageModel
    {
        public string DetailView { get; set; }
        public long? SelectedFormerStudentId { get; set; }
        public ClassPublicDetail Class { get; set; }
        public BaseRegistrationDetail Registration { get; set; }
        public List<FormerStudentForReviewListItem> FormerStudents { get; set; }
        public RegistrationLastClassForm FormLastClass { get; set; }

        // constructor
        public FormerStudentSelectionPageModel()
        {
            Class = new ClassPublicDetail();
            Registration = new BaseRegistrationDetail();
            FormerStudents = new List<FormerStudentForReviewListItem>();
            FormLastClass = new RegistrationLastClassForm();
        }
    }
}