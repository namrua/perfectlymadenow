using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Payment.AppLogic.Extensions
{
    /// <summary>
    /// Identity resolver extensions
    /// </summary>
    public static class PaymentIdentityResolverExtensions
    {

        // checks entitle for PayPal key
        public static void CheckEntitleForPayPalKey(this IIdentityResolver identityResolver, Entitle entitle, PayPalKey payPalKey)
        {
            identityResolver.CheckEntitleForUserGroup(entitle, payPalKey.UserGroupTypeId, payPalKey.UserGroupId, 
                entityType: "PayPalKey", entityId: payPalKey.PayPalKeyId);
        }

    }

}
