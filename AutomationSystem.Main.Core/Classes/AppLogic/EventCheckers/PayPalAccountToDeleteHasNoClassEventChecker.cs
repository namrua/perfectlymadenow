using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Shared.Contract.Payment.AppLogic.Models.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Main.Core.Classes.AppLogic.EventCheckers
{
    public class PayPalAccountToDeleteHasNoClassEventChecker : IEventChecker<PayPalAccountDeletingEvent>
    {
        private readonly IClassDatabaseLayer classDb;

        public PayPalAccountToDeleteHasNoClassEventChecker(IClassDatabaseLayer classDb)
        {
            this.classDb = classDb;
        }

        public bool CheckEvent(PayPalAccountDeletingEvent evnt)
        {
            var result = !classDb.PayPalKeyOnAnyClass(evnt.PayPalKeyId);
            return result;
        }
    }
}
