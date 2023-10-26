using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Base.Contract.Identities.Models.Events;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.Profiles.AppLogic;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.AppLogic.Factories;
using AutomationSystem.Main.Core.Profiles.AppLogic.Models.Events;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Core.Profiles.System.Extensions;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationSystem.Main.Core.Profiles.AppLogic
{

    /// <summary>
    /// Provides profile administration
    /// </summary>
    public class ProfileAdministration : IProfileAdministration
    {
        private readonly IProfileDatabaseLayer profileDb;
        private readonly IIdentityResolver identityResolver;
        private readonly IMainMapper mainMapper;
        private readonly IClassPreferenceFactory classPreferenceFactory;
        private readonly IEventDispatcher eventDispatcher;


        // constructor
        public ProfileAdministration(IProfileDatabaseLayer profileDb,
            IIdentityResolver identityResolver,
            IMainMapper mainMapper,
            IClassPreferenceFactory classPreferenceFactory,
            IEventDispatcher eventDispatcher)
        {
            this.profileDb = profileDb;
            this.identityResolver = identityResolver;
            this.mainMapper = mainMapper;
            this.classPreferenceFactory = classPreferenceFactory;
            this.eventDispatcher = eventDispatcher;
        }


        #region profiles

        // gets profiles
        public List<ProfileListItem> GetProfiles()
        {
            var profileFilter = identityResolver.GetGrantedProfilesForEntitle(Entitle.MainProfiles);
            var profiles = profileDb.GetProfilesByFilter(profileFilter);

            // todo: #BICH Batch item check

            var result = profiles.Select(mainMapper.Map<ProfileListItem>).ToList();
            return result;
        }

        // gets profile detail
        public ProfileDetail GetProfileDetail(long profileId)
        {
            var profile = GetProfileById(profileId);
            identityResolver.CheckEntitleForProfile(Entitle.MainProfiles, profile);
            var result = mainMapper.Map<ProfileDetail>(profile);
            result.CanDelete = CanDeleteProfile(profileId);
            return result;
        }


        // gets new profile form
        public ProfileForm GetNewProfileForm()
        {
            identityResolver.CheckEntitle(Entitle.MainProfiles);
            var result = new ProfileForm
            {
                OriginMoniker = ""
            };
            return result;
        }

        // gets profile form by id
        public ProfileForm GetProfileFormById(long profileId)
        {
            var profile = GetProfileById(profileId);
            identityResolver.CheckEntitleForProfile(Entitle.MainProfiles, profile);
            var result = mainMapper.Map<ProfileForm>(profile);
            return result;
        }


        // gets profile form by form and validation
        public ProfileForm GetProfileFormByFormAndValidation(ProfileForm form, ProfileValidationResult validation)
        {
            if (validation.ForbiddenMoniker != null)
                form.ForbiddenMoniker = validation.ForbiddenMoniker;
            return form;
        }

        // validates profile form
        public ProfileValidationResult ValidateProfileForm(ProfileForm form)
        {
            // prevents DB sniffing
            identityResolver.CheckEntitle(Entitle.MainProfiles);

            var result = new ProfileValidationResult();

            // test duplicit moniker
            var profileWithMoniker = profileDb.GetProfileByMoniker(form.Moniker);
            if (profileWithMoniker != null && profileWithMoniker.ProfileId != form.ProfileId)
            {
                result.IsValid = false;
                result.ForbiddenMoniker = profileWithMoniker.Moniker;
            }

            return result;
        }


        // saves profile
        public long SaveProfile(ProfileForm form, out bool updateIdentityClaims)
        {
            var dbProfile = mainMapper.Map<Profile>(form);
            var result = dbProfile.ProfileId;
            if (dbProfile.ProfileId == 0)
            {
                identityResolver.CheckEntitle(Entitle.MainProfiles);
                dbProfile.OwnerId = identityResolver.GetOwnerId();
                dbProfile.ClassPreference = classPreferenceFactory.CreateDefaultClassPreference();
                dbProfile.ProfileUsers.Add(new ProfileUser { UserId = dbProfile.OwnerId });
                result = profileDb.InsertProfile(dbProfile);

                // determines whether created profile is accessible for current identity, if not, updateIdentityClaims is set to true
                updateIdentityClaims = !identityResolver.IsEntitleGrantedForUserGroup(Entitle.MainProfiles, UserGroupTypeEnum.MainProfile, result);
            }
            else
            {
                updateIdentityClaims = false;
                identityResolver.CheckEntitleForProfileId(Entitle.MainProfiles, result);
                profileDb.UpdateProfile(dbProfile);
            }

            return result;
        }

        // deletes profile
        public void DeleteProfile(long profileId)
        {
            identityResolver.CheckEntitleForProfileId(Entitle.MainProfiles, profileId);
            if (!CanDeleteProfile(profileId))
            {
                throw new InvalidOperationException($"Profile with id {profileId} has assigned some classes, persons or entities and cannot be deleted.");
            }

            profileDb.DeleteProfile(profileId);
        }

        #endregion

        #region private methods

        // gets profile, checks whether profile exists
        private Profile GetProfileById(long profileId, ProfileIncludes includes = ProfileIncludes.None)
        {
            var result = profileDb.GetProfileById(profileId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Profile with id {profileId}.");
            }

            return result;
        }


        // determines whether profile can be deleted
        private bool CanDeleteProfile(long profileId)
        {
            var result = eventDispatcher.Check(new ProfileDeletingEvent(profileId)) 
                && eventDispatcher.Check(new UserGroupDeletingEvent(profileId, UserGroupTypeEnum.MainProfile));
            return result;
        }

        #endregion

    }

}
