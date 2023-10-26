using System.Collections.Generic;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Profiles.AppLogic
{

    /// <summary>
    /// Provides profile administration
    /// </summary>
    public interface IProfileAdministration
    {
        // gets profiles
        List<ProfileListItem> GetProfiles();

        // gets profile detail
        ProfileDetail GetProfileDetail(long profileId);

        // gets new profile form
        ProfileForm GetNewProfileForm();

        // gets profile form by id
        ProfileForm GetProfileFormById(long profileId);

        // gets profile form by form and validation
        ProfileForm GetProfileFormByFormAndValidation(ProfileForm form, ProfileValidationResult validation);

        // validates profile form
        ProfileValidationResult ValidateProfileForm(ProfileForm form);

        // saves profile
        long SaveProfile(ProfileForm form, out bool updateIdentityClaims);

        // deletes profile
        void DeleteProfile(long profileId);
        
    }


}
