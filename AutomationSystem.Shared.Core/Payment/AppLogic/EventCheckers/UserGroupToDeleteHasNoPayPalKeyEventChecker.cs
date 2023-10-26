using AutomationSystem.Base.Contract.Identities.Models.Events;
using AutomationSystem.Shared.Contract.Payment.Data;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Shared.Core.Payment.AppLogic.EventCheckers
{
    public class UserGroupToDeleteHasNoPayPalKeyEventChecker : IEventChecker<UserGroupDeletingEvent>
    {
        public IPaymentDatabaseLayer paymentDb;

        public UserGroupToDeleteHasNoPayPalKeyEventChecker(IPaymentDatabaseLayer paymentDb)
        {
            this.paymentDb = paymentDb;
        }

        public bool CheckEvent(UserGroupDeletingEvent evnt)
        {
            var result = !paymentDb.AnyPayPalKeyOnUserGroup(evnt.UserGroupId, evnt.UserGroupTypeId);
            return result;
        }
    }
}
