using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Payment.Data;
using AutomationSystem.Shared.Contract.Payment.Data.Model;
using AutomationSystem.Shared.Core.Payment.Data.Extensions;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;

namespace AutomationSystem.Shared.Core.Payment.Data
{
    // Provides payment database layer
    public class PaymentDatabaseLayer: IPaymentDatabaseLayer
    {

        // gets active paypal keys
        public List<PayPalKey> GetActivePayPalKeys(long? currentId, PayPalKeyFilter filter = null)
        {
            using (var context = new CoreEntities())
            {
                var result = context.PayPalKeys.Filter(filter).ToList();
                if (!currentId.HasValue || result.Any(x => x.PayPalKeyId == currentId)) return result;

                // adds current if not listed - !!! Active() is ignored !!!
                var current = context.PayPalKeys.FirstOrDefault(x => x.PayPalKeyId == currentId);
                if (current == null)
                {
                    throw new InvalidOperationException($"Current PayPalKey with id {currentId} does not exist.");
                }

                // adds current to the start of the list
                var newResult = new List<PayPalKey> { current };
                newResult.AddRange(result);
                return newResult;
            }
        }

        // get list of paypalkeys
        public List<PayPalKey> GetPayPalKeys(PayPalKeyFilter filter = null)
        {
            using (var context = new CoreEntities())
            {
                var result = context.PayPalKeys.Filter(filter).OrderBy(x => x.Name).ToList();
                return result;
            }
        }

        // gets paypal key by id
        public PayPalKey GetPayPalKeyById(long id)
        {
            using (var context = new CoreEntities())
            {
                var result = context.PayPalKeys.Active().FirstOrDefault(x => x.PayPalKeyId == id);
                if (result == null)
                {
                    throw new ArgumentException($"There is no PayPalKey with id {id}.");
                }

                return result;
            }
        }

        // gets paypal keys by ids
        public List<PayPalKey> GetPayPalKeysByIds(List<long> ids)
        {
            using (var context = new CoreEntities())
            {
                var result = context.PayPalKeys.Active().Where(x => ids.Contains(x.PayPalKeyId)).ToList();
                return result;
            }
        }

        // checks if paypal key is lined to user group
        public bool AnyPayPalKeyOnUserGroup(long userGroupId, UserGroupTypeEnum userGroupTypeId)
        {
            using (var context = new CoreEntities())
            {
                var result = context.PayPalKeys.Active().Any(x => x.UserGroupId == userGroupId && x.UserGroupTypeId == userGroupTypeId);
                return result;
            }
        }


        // insert paypal key
        public long InsertPayPalKey(PayPalKey model)
        {
            using (var context = new CoreEntities())
            {
                context.PayPalKeys.Add(model);
                context.SaveChanges();
                return model.PayPalKeyId;
            }
        }

        // update paypal key
        public void UpdatePayPalKey(PayPalKey model)
        {
            using (var context = new CoreEntities())
            {
                // to update
                var toUpdate = context.PayPalKeys.FirstOrDefault(x => x.PayPalKeyId == model.PayPalKeyId);
                if (toUpdate == null)
                   throw new ArgumentException($"There is no PayPalKey with id {model.PayPalKeyId}.");

                toUpdate.Name = model.Name;
                toUpdate.BraintreeToken = model.BraintreeToken;
                toUpdate.Environment = model.Environment;                                
                toUpdate.Active = model.Active;
                toUpdate.CurrencyId = model.CurrencyId;

                // save changes
                context.SaveChanges();                
            }
        }

        // delete paypal key
        public void DeletePayPalKey(long paypalKeyId)
        {
            using (var context = new CoreEntities())
            {
                var toDelete = context.PayPalKeys.FirstOrDefault(x => x.PayPalKeyId == paypalKeyId);
                if (toDelete == null) return;
                context.PayPalKeys.Remove(toDelete);
                context.SaveChanges();
            }
        }

        // gets paypal record by id
        public PayPalRecord GetPayPalRecordById(long payPalRecordId)
        {
            using (var context = new CoreEntities())
            {
                var result = context.PayPalRecords.Active().FirstOrDefault(x => x.PayPalRecordId == payPalRecordId);
                return result;
            }
        }

        // insert paypal record
        public long InsertPayPalRecord(PayPalRecord payPalRecord)
        {
            using (var context = new CoreEntities())
            {
                context.PayPalRecords.Add(payPalRecord);
                context.SaveChanges();
                return payPalRecord.PayPalRecordId;
            }
        }

    }

}
