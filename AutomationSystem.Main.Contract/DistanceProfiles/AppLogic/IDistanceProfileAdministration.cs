using AutomationSystem.Main.Contract.DistanceProfiles.AppLogic.Models;

namespace AutomationSystem.Main.Contract.DistanceProfiles.AppLogic
{
    public interface IDistanceProfileAdministration
    {
        // gets distance profile page model
        DistanceProfilePageModel GetDistanceProfilePageModel();

        // gets distance profile detail by distance profile id
        DistanceProfileDetail GetDistanceProfileDetailById(long distanceProfileId);

        // gets new distance profile for edit
        DistanceProfileForEdit GetNewDistanceProfileForEdit(long profileId);

        // gets distance profile for edit by distance profile id
        DistanceProfileForEdit GetDistanceProfileForEditById(long distanceProfileId);

        // gets distance profile for edit by distance profile form
        DistanceProfileForEdit GetDistanceProfileForEditByForm(DistanceProfileForm form);

        // activate distance profile
        void ActivateDistanceProfile(long distanceProfileId);

        // deactivate distance profile
        void DeactivateDistanceProfile(long distanceProfileId);

        // saves distance profile 
        long SaveDistanceProfile(DistanceProfileForm form);

        // deletes distance profile
        void DeleteDistanceProfile(long distanceProfileId);
    }
}
