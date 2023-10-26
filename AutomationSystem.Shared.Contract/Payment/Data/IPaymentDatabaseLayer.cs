using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Payment.Data.Model;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.Payment.Data
{
    /// <summary>
    /// Provides payment database layer
    /// </summary>
    public interface IPaymentDatabaseLayer
    {

        // gets active paypal keys
        List<PayPalKey> GetActivePayPalKeys(long? currentId, PayPalKeyFilter filter = null);

        // get list of paypalkeys
        List<PayPalKey> GetPayPalKeys(PayPalKeyFilter filter = null);

        // get paypal key by id
        PayPalKey GetPayPalKeyById(long id);

        //gets paypal keys by ids
        List<PayPalKey> GetPayPalKeysByIds(List<long> ids);


        // checks if paypal key is lined to user group
        bool AnyPayPalKeyOnUserGroup(long userGroupId, UserGroupTypeEnum userGroupTypeId);

        // insert paypal key
        long InsertPayPalKey(PayPalKey payPalKey);

        // update paypal key
        void UpdatePayPalKey(PayPalKey payPalKey);

        // delete paypal key by id
        void DeletePayPalKey(long payPalKeyId);


        // gets paypal record by id
        PayPalRecord GetPayPalRecordById(long payPalRecordId);

        // insert paypal record
        long InsertPayPalRecord(PayPalRecord payPalRecord);

    }

}
