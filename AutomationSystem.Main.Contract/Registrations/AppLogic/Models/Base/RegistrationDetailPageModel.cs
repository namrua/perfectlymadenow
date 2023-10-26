using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base
{
    /// <summary>
    /// Registration detail page model
    /// </summary>
    public class RegistrationDetailPageModel
    {

        public RegistrationListItem Registration { get; set; }
        public ClassShortDetail Class { get; set; }
        public EmailTemplateListItem RegistrationCancelTemplate { get; set; }

        public RegistrationFullState FullState => RegistrationFullState.New(Class.ClassState,
            Registration.RegistrationState, Registration.ApprovementTypeId, Registration.IsReviewed);
        public bool CanApprove { get; set; }
        public bool CanCancel { get; set; }
        public bool CanDelete { get; set; }

        // constructor
        public RegistrationDetailPageModel()
        {
            Registration = new RegistrationListItem();
            Class = new ClassShortDetail();             
        }

    }

}
