using AutomationSystem.Main.Core.DistanceProfiles.Data;
using AutomationSystem.Shared.Contract.Payment.AppLogic.Models.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Main.Core.DistanceProfiles.AppLogic.EventCheckers
{
    public class PayPalAccountToDeleteHasNoDistanceProfileEventChecker : IEventChecker<PayPalAccountDeletingEvent>
    {
        public IDistanceProfileDatabaseLayer distanceProfileDb;
        
        public PayPalAccountToDeleteHasNoDistanceProfileEventChecker(IDistanceProfileDatabaseLayer distanceProfileDb)
        {
            this.distanceProfileDb = distanceProfileDb;
        }

        public bool CheckEvent(PayPalAccountDeletingEvent evnt)
        {
            var result = !distanceProfileDb.PayPalKeyOnAnyDistanceProfile(evnt.PayPalKeyId);
            return result;
        }
    }
}
