using AutomationSystem.Main.Contract.DistanceProfiles.AppLogic.Models;

namespace AutomationSystem.Main.Core.DistanceProfiles.AppLogic
{
    public interface IDistanceProfileFactory
    {
        DistanceProfileForEdit CreateDistanceProfileForEdit(long profileId, long? currentPriceListId = null, long? currentPayPalKeyId = null);
        
    }
}
