using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Profiles.AppLogic
{
    public interface IProfileEmailAdministration
    {
        EmailTypeSummary GetEmailTypeSummaryByProfileId(long profileId);

    }
}
