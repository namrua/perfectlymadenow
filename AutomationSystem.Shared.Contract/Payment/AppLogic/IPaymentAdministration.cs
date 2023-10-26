using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Payment.AppLogic.Models;
using AutomationSystem.Shared.Contract.Payment.Data.Model;

namespace AutomationSystem.Shared.Contract.Payment.AppLogic
{
    /// <summary>
    /// Service for payment administration
    /// </summary>
    public interface IPaymentAdministration
    {

        // gets list of paypalKeys by filter
        List<PayPalKeyListItem> GetPayPalKeyListItems(PayPalKeyFilter filter = null);

        // get new paypal for edit
        PayPalKeyForEdit GetNewPayPalKeyForEdit(UserGroupTypeEnum? userGroupTypeId, long? userGroupId);

        // get paypal key for edit by paypalkey id
        PayPalKeyForEdit GetPayPalKeyForEditById(long payPalKeyId);         

        // get paypal key for edit by form
        PayPalKeyForEdit GetPayPalKeyForEditByForm(PayPalKeyForm form);

        // saves paypalKey
        long SavePayPalKey(PayPalKeyForm payPalKey);        

        // delete paypal key
        void DeletePayPalKey(long payPalKeyId);

    }
}