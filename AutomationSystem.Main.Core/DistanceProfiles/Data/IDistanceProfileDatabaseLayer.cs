using AutomationSystem.Main.Core.DistanceProfiles.Data.Models;
using AutomationSystem.Main.Model;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.DistanceProfiles.Data
{
    public interface IDistanceProfileDatabaseLayer
    {
        // gets list of distance profiles
        List<DistanceProfile> GetDistanceProfilesByFilter(DistanceProfileFilter filter, DistanceProfileIncludes includes = DistanceProfileIncludes.None);

        // gets distance profile by id
        DistanceProfile GetDistanceProfileById(long id, DistanceProfileIncludes includes = DistanceProfileIncludes.None);

        bool PersonOnAnyDistanceProfile(long personId);

        bool PriceListOnAnyDistanceProfile(long priceListId);

        bool PayPalKeyOnAnyDistanceProfile(long payPalKeyId);

        // check if distance profile is linked to profile
        bool AnyDistanceProfileOnProfile(long profileId);

        // saves distance profile
        long InsertDistanceProfile(DistanceProfile distanceProfile);

        // updates distance profile
        void UpdateDistanceProfile(DistanceProfile distanceProfile);
        
        // deletes distance profile
        void DeleteDistanceProfile(long id);

        // sets distance profile as active
        void SetDistanceProfileAsActive(long id);

        // sets distance profile as deactive
        void SetDistanceProfileAsDeactive(long id);
    }
}
