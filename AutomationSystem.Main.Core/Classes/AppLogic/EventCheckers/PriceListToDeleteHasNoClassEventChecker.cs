using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.PriceLists.AppLogic.Models.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Main.Core.Classes.AppLogic.EventCheckers
{
    public class PriceListToDeleteHasNoClassEventChecker : IEventChecker<PriceListDeletingEvent>
    {
        public IClassDatabaseLayer classDb;

        public PriceListToDeleteHasNoClassEventChecker(IClassDatabaseLayer classDb)
        {
            this.classDb = classDb;
        }

        public bool CheckEvent(PriceListDeletingEvent evnt)
        {
            var result = !classDb.PriceListOnAnyClass(evnt.PriceListId);
            return result;
        }
    }
}
