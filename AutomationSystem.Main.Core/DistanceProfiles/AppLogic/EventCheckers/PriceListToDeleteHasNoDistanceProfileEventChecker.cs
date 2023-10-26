using AutomationSystem.Main.Core.DistanceProfiles.Data;
using AutomationSystem.Main.Core.PriceLists.AppLogic.Models.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events;
namespace AutomationSystem.Main.Core.DistanceProfiles.AppLogic.EventCheckers
{
    public class PriceListToDeleteHasNoDistanceProfileEventChecker : IEventChecker<PriceListDeletingEvent>
    {
        public IDistanceProfileDatabaseLayer distanceProfileDb;

        public PriceListToDeleteHasNoDistanceProfileEventChecker(IDistanceProfileDatabaseLayer distanceProfileDb)
        {
            this.distanceProfileDb = distanceProfileDb;
        }

        public bool CheckEvent(PriceListDeletingEvent evnt)
        {
            var result = !distanceProfileDb.PriceListOnAnyDistanceProfile(evnt.PriceListId);
            return result;
        }
    }
}
