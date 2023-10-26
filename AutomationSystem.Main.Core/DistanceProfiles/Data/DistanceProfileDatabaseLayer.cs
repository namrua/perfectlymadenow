using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Main.Core.DistanceProfiles.Data.Extensions;
using AutomationSystem.Main.Core.DistanceProfiles.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;

namespace AutomationSystem.Main.Core.DistanceProfiles.Data
{
    public class DistanceProfileDatabaseLayer : IDistanceProfileDatabaseLayer
    {
        public List<DistanceProfile> GetDistanceProfilesByFilter(DistanceProfileFilter filter, DistanceProfileIncludes includes = DistanceProfileIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.DistanceProfiles.AddIncludes(includes).Filter(filter).ToList();
                return result;
            }
        }

        public DistanceProfile GetDistanceProfileById(long id, DistanceProfileIncludes includes = DistanceProfileIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.DistanceProfiles.AddIncludes(includes).Active().FirstOrDefault(x => x.DistanceProfileId == id);
                return result;
            }
        }
        
        public bool PersonOnAnyDistanceProfile(long personId)
        {
            using (var context = new MainEntities())
            {
                var result = context.DistanceProfiles.Active().Any(x => x.DistanceCoordinatorId == personId);
                return result;
            }
        }

        public bool PriceListOnAnyDistanceProfile(long priceListId)
        {
            using (var context = new MainEntities())
            {
                var result = context.DistanceProfiles.Active().Any(x => x.PriceListId == priceListId);
                return result;
            }
        }

        public bool PayPalKeyOnAnyDistanceProfile(long payPalKeyId)
        {
            using (var context = new MainEntities())
            {
                var result = context.DistanceProfiles.Active().Any(x => x.PayPalKeyId == payPalKeyId);
                return result;
            }
        }

        public bool AnyDistanceProfileOnProfile(long profileId)
        {
            using (var context = new MainEntities())
            {
                var result = context.DistanceProfiles.Active().Any(x => x.ProfileId == profileId);
                return result;
            }
        }

        public long InsertDistanceProfile(DistanceProfile distanceProfile)
        {
            using (var context = new MainEntities())
            {
                context.DistanceProfiles.Add(distanceProfile);
                context.SaveChanges();
                return distanceProfile.DistanceProfileId;
            }
        }

        public void UpdateDistanceProfile(DistanceProfile distanceProfile)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.DistanceProfiles.FirstOrDefault(x => x.DistanceProfileId == distanceProfile.DistanceProfileId);
                if (toUpdate == null)
                {
                    throw new ArgumentException($"There is no distance profile with id {distanceProfile.DistanceProfileId}.");
                }

                toUpdate.DistanceCoordinatorId = distanceProfile.DistanceCoordinatorId;
                toUpdate.PayPalKeyId = distanceProfile.PayPalKeyId;
                toUpdate.PriceListId = distanceProfile.PriceListId;

                context.SaveChanges();
            }
        } 

        public void DeleteDistanceProfile(long id)
        {
            using (var context = new MainEntities())
            {
                var toDelete = context.DistanceProfiles.Active().FirstOrDefault(x => x.DistanceProfileId == id);
                if (toDelete == null)
                {
                    return;
                }

                context.DistanceProfiles.Remove(toDelete);
                context.SaveChanges();
            }
        }

        public void SetDistanceProfileAsActive(long id)
        {
            using (var context = new MainEntities())
            {
                var profile = context.DistanceProfiles.Active().FirstOrDefault(x => x.DistanceProfileId == id);
                if (profile == null)
                {
                    throw new ArgumentException($"There is no distance profile with id {id}.");
                }

                if (profile.IsActive)
                {
                    throw new InvalidOperationException($"Profile with id {id} is already activated.");
                }

                profile.IsActive = true;
                profile.Activated = DateTime.Now;
                context.SaveChanges();
            }
        }

        public void SetDistanceProfileAsDeactive(long id)
        {
            using (var context = new MainEntities())
            {
                var profile = context.DistanceProfiles.Active().FirstOrDefault(x => x.DistanceProfileId == id);
                if (profile == null)
                {
                    throw new ArgumentException($"There is no distance profile with id {id}.");
                }

                if (!profile.IsActive)
                {
                    throw new InvalidOperationException($"Profile with id {id} is already deactivated.");
                }

                profile.IsActive = false;
                profile.Activated = null;
                context.SaveChanges();
            }
        }
    }
}
