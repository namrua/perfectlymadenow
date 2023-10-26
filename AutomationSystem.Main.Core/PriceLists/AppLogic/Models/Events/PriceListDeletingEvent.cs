using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace AutomationSystem.Main.Core.PriceLists.AppLogic.Models.Events
{
    public class PriceListDeletingEvent : BaseEvent
    {
        public long PriceListId { get; }

        public PriceListDeletingEvent(long priceListId)
        {
            PriceListId = priceListId;
        }

        public override string ToString()
        {
            return $"PriceListId: {PriceListId}";
        }
    }
}
