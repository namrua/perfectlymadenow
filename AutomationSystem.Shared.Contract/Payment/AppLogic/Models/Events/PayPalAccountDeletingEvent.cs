using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace AutomationSystem.Shared.Contract.Payment.AppLogic.Models.Events
{
    public class PayPalAccountDeletingEvent : BaseEvent
    {
        public long PayPalKeyId { get; }

        public PayPalAccountDeletingEvent(long payPalKeyId)
        {
            PayPalKeyId = payPalKeyId;
        }

        public override string ToString()
        {
            return $"PayPalKeyId: {PayPalKeyId}";
        }
    }
}
