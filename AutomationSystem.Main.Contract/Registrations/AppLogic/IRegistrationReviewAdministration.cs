using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.ManualReview;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic
{
    public interface IRegistrationReviewAdministration
    {
        RegistrationManualReviewPageModel GetRegistrationManualReviewPageModel(long registrationId);
    }
}
