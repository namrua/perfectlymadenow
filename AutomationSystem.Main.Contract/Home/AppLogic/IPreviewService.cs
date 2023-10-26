using AutomationSystem.Main.Contract.Home.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Home.AppLogic
{
    /// <summary>
    /// Provides data for previewing purposes
    /// </summary>
    public interface IPreviewService
    {

        // gets preview style page model by back URL and classId
        PreviewStylePageModel GetPreviewStylePageModelForClass(string backUrl, long classId);

        // gets preview style page model by back URL for profile
        PreviewStylePageModel GetPreviewStylePageModelForProfile(string backUrl, long profileId);

    }

}
