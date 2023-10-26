using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.DistanceProfiles.AppLogic;
using AutomationSystem.Main.Contract.DistanceProfiles.AppLogic.Models;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Core.DistanceProfiles.AppLogic.Models.Events;
using AutomationSystem.Main.Core.DistanceProfiles.Data;
using AutomationSystem.Main.Core.DistanceProfiles.Data.Models;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Payment.Data;
using CorabeuControl.Components;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationSystem.Main.Core.DistanceProfiles.AppLogic
{
    public class DistanceProfileAdministration : IDistanceProfileAdministration
    {
        private readonly IPaymentDatabaseLayer paymentDb;
        private readonly IMainMapper mainMapper;
        private readonly IDistanceProfileDatabaseLayer distanceProfileDb;
        private readonly IProfileDatabaseLayer profileDb;
        private readonly IDistanceProfileFactory factory;
        private readonly IEventDispatcher eventDispatcher;

        public DistanceProfileAdministration(
            IPaymentDatabaseLayer paymentDb,
            IMainMapper mainMapper,
            IDistanceProfileDatabaseLayer distanceProfileDb,
            IProfileDatabaseLayer profileDb,
            IDistanceProfileFactory factory,
            IEventDispatcher eventDispatcher)
        {
            this.paymentDb = paymentDb;
            this.mainMapper = mainMapper;
            this.distanceProfileDb = distanceProfileDb;
            this.profileDb = profileDb;
            this.factory = factory;
            this.eventDispatcher = eventDispatcher;
        }

        // gets distance profile page model
        public DistanceProfilePageModel GetDistanceProfilePageModel()
        {
            var distanceProfiles = distanceProfileDb.GetDistanceProfilesByFilter(
                null,
                DistanceProfileIncludes.Profile
                | DistanceProfileIncludes.PriceList
                | DistanceProfileIncludes.DistanceCoordinatorAddress);

            var filter = new ProfileFilter
            {
                ExcludeProfileIds = distanceProfiles.Select(x => x.ProfileId).ToList()
            };
            var profiles = profileDb.GetProfilesByFilter(filter);

            var model = new DistanceProfilePageModel
            {
                Items = MapToDistanceProfileListItems(distanceProfiles),
                Profiles = profiles.Select(x => DropDownItem.Item(x.ProfileId, x.Name)).ToList()
            };
            
            return model;
        }

        // gets distance profile detail by distance profile id
        public DistanceProfileDetail GetDistanceProfileDetailById(long distanceProfileId)
        {
            var distanceProfile = GetDistanceProfileById(
                distanceProfileId,
                DistanceProfileIncludes.Profile
                | DistanceProfileIncludes.PriceList
                | DistanceProfileIncludes.DistanceCoordinatorAddress);

            var model = MapToDistanceProfileDetail(distanceProfile);

            return model;
        }

        // gets new distance profile for edit
        public DistanceProfileForEdit GetNewDistanceProfileForEdit(long profileId)
        {
            if (profileDb.GetProfileById(profileId) == null)
            {
                throw new ArgumentException($"There is no profile with id {profileId}.");
            }

            var result = factory.CreateDistanceProfileForEdit(profileId);
            result.Form.ProfileId = profileId;
            result.Form.DistanceCoordinatorId = result.Persons.GetDefaultPersonId(PersonRoleTypeEnum.DistanceDoordinator);
            return result;
        }

        // gets distance profile for edit by distance profile id
        public DistanceProfileForEdit GetDistanceProfileForEditById(long distanceProfileId)
        {
            var distanceProfile = GetDistanceProfileById(distanceProfileId);

            var forEdit = factory.CreateDistanceProfileForEdit(distanceProfile.ProfileId, distanceProfile.PriceListId, distanceProfile.PayPalKeyId);
            forEdit.Form = mainMapper.Map<DistanceProfileForm>(distanceProfile);
            return forEdit;
        }

        // gets distance profile for edit by distance profile form
        public DistanceProfileForEdit GetDistanceProfileForEditByForm(DistanceProfileForm form)
        {
            var forEdit = factory.CreateDistanceProfileForEdit(form.ProfileId, form.CurrentPriceListId, form.CurrentPayPalKeyId);
            forEdit.Form = form;
            return forEdit;
        }

        // activate distance profile
        public void ActivateDistanceProfile(long distanceProfileId)
        {
            eventDispatcher.Dispatch(new DistanceProfileStatusChangedEvent(distanceProfileId, true));
            distanceProfileDb.SetDistanceProfileAsActive(distanceProfileId);
        }

        // deactivate distance profile
        public void DeactivateDistanceProfile(long distanceProfileId)
        {
            eventDispatcher.Dispatch(new DistanceProfileStatusChangedEvent(distanceProfileId, false));
            distanceProfileDb.SetDistanceProfileAsDeactive(distanceProfileId);
        }

        // saves distance profile
        public long SaveDistanceProfile(DistanceProfileForm form)
        {
            var distanceProfile = mainMapper.Map<DistanceProfile>(form);
            var distanceProfileId = form.DistanceProfileId;
            if (distanceProfileId == 0)
            {
                distanceProfileId = distanceProfileDb.InsertDistanceProfile(distanceProfile);
            }
            else
            {
                distanceProfileDb.UpdateDistanceProfile(distanceProfile);
            }

            return distanceProfileId;
        }

        // deletes distance profile
        public void DeleteDistanceProfile(long distanceProfileId)
        {
            distanceProfileDb.DeleteDistanceProfile(distanceProfileId);
        }

        #region private methods
        private DistanceProfile GetDistanceProfileById(long distanceProfileId, DistanceProfileIncludes includes = DistanceProfileIncludes.None)
        {
            var result = distanceProfileDb.GetDistanceProfileById(distanceProfileId, includes);

            if (result == null)
            {
                throw new ArgumentException($"There is no distance profile with id {distanceProfileId}.");
            }

            return result;
        }

        private List<DistanceProfileListItem> MapToDistanceProfileListItems(List<DistanceProfile> distanceProfiles)
        {
            var result = new List<DistanceProfileListItem>();
            var payPalKeyIds = distanceProfiles.Select(x => x.PayPalKeyId).ToList();
            var payPalKeysMap = paymentDb.GetPayPalKeysByIds(payPalKeyIds).ToDictionary(x => x.PayPalKeyId);
            foreach (var distanceProfile in distanceProfiles)
            {
                var item = mainMapper.Map<DistanceProfileListItem>(distanceProfile);
                if (!payPalKeysMap.TryGetValue(distanceProfile.PayPalKeyId, out var payPalKey))
                {
                    throw new ArgumentException($"There is no PayPal key with id {distanceProfile.PayPalKeyId}.");
                }

                item.PayPalKey = payPalKey.Name;
                result.Add(item);
            }
            return result;
        }

        private DistanceProfileDetail MapToDistanceProfileDetail(DistanceProfile profile)
        {
            var payPalKey = paymentDb.GetPayPalKeyById(profile.PayPalKeyId);
            if (payPalKey == null)
            {
                throw new ArgumentException($"There is no PayPal key with id {profile.PayPalKeyId}.");
            }

            var result = mainMapper.Map<DistanceProfileDetail>(profile);
            result.PayPalKey = payPalKey.Name;
            return result;
        }
        #endregion
    }
}
