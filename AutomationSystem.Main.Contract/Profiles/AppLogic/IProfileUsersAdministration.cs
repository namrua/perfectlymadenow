using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Profiles.AppLogic
{
    public interface IProfileUsersAdministration
    {
        // gets profile users page model
        ProfileUsersPageModel GetProfileUsersPageModel(long profileId);

        // adds user to profile
        void AddUserToProfile(long profileId, int userId);

        // removes user from profile
        void RemoveUserFromProfile(long profileId, int userId);

    }
}
