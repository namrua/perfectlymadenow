using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using AutomationSystem.Shared.Contract.Payment.AppLogic;
using AutomationSystem.Shared.Contract.Payment.AppLogic.Models;
using AutomationSystem.Shared.Contract.Payment.AppLogic.Models.Events;
using AutomationSystem.Shared.Contract.Payment.Data;
using AutomationSystem.Shared.Contract.Payment.Data.Model;
using AutomationSystem.Shared.Core.Payment.AppLogic.Extensions;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Shared.Core.Payment.AppLogic
{
    /// <summary>
    /// Service for payment administration
    /// </summary>
    public class PaymentAdministration : IPaymentAdministration
    {

        private readonly IPaymentDatabaseLayer paymentDb;
        private readonly IIdentityResolver identityResolver;
        private readonly IEventDispatcher eventDispatcher;
        private readonly IPayPalKeyConvertor payPalConvertor;

        // constructor
        public PaymentAdministration(
            IPaymentDatabaseLayer paymentDb,
            IIdentityResolver identityResolver,
            IEventDispatcher eventDispatcher,
            IPayPalKeyConvertor payPalConvertor)
        {
            this.paymentDb = paymentDb;
            this.identityResolver = identityResolver;
            this.eventDispatcher = eventDispatcher;
            this.payPalConvertor = payPalConvertor;
        }

        // gets list of paypalKeys by filter
        public List<PayPalKeyListItem> GetPayPalKeyListItems(PayPalKeyFilter filter = null)
        {
            var payPalKeys = paymentDb.GetPayPalKeys(filter);
            var result = payPalKeys.Select(payPalConvertor.ConvertToPayPalKeyListDetail).ToList();
            // todo: #BICH - batch item check
            return result;
        }

        // get new paypal for edit
        public PayPalKeyForEdit GetNewPayPalKeyForEdit(UserGroupTypeEnum? userGroupTypeId, long? userGroupId)
        {
            // checks access rights
            identityResolver.CheckEntitleForUserGroup(Entitle.CorePayPalKeys, userGroupTypeId, userGroupId);

            // creates new PayPalKeyForm
            var form = new PayPalKeyForm();
            form.Environment = "production";
            form.Active = true;
            form.UserGroupTypeId = userGroupTypeId;
            form.UserGroupId = userGroupId;
            form.CurrencyId = LocalisationInfo.DefaultCurrency;

            // creates PayPalKeyForEdit
            var result = payPalConvertor.InitializePayPalKeyForEdit();
            result.Form = form;
            return result;
        }

        // get paypal key for edit by paypalkey id
        public PayPalKeyForEdit GetPayPalKeyForEditById(long payPalKeyId)
        {
            var payPalKey = GetPayPalKeyById(payPalKeyId);
            identityResolver.CheckEntitleForPayPalKey(Entitle.CorePayPalKeys, payPalKey);

            // creates PayPalKeyForEdit
            var result = payPalConvertor.InitializePayPalKeyForEdit();
            result.Form = payPalConvertor.ConvertoToPayPalKeyForm(payPalKey);
            result.CanDelete = CanDeletePayPalKey(payPalKeyId);
            return result;
        }

        // get paypal key for edit by form
        public PayPalKeyForEdit GetPayPalKeyForEditByForm(PayPalKeyForm form)
        {
            // creates PayPalKeyForEdit
            var result = payPalConvertor.InitializePayPalKeyForEdit();
            result.Form = form;
            result.CanDelete = CanDeletePayPalKey(form.PayPalKeyId);
            return result;
        }

        // saves paypalKey
        public long SavePayPalKey(PayPalKeyForm form)
        {
            var dbPayPalKey = payPalConvertor.ConvertToPayPalKey(form);
            var result = form.PayPalKeyId;
            if (result == 0)
            {
                identityResolver.CheckEntitleForPayPalKey(Entitle.CorePayPalKeys, dbPayPalKey);
                dbPayPalKey.OwnerId = identityResolver.GetOwnerId();
                result = paymentDb.InsertPayPalKey(dbPayPalKey);
            }
            else
            {
                var toCheck = GetPayPalKeyById(form.PayPalKeyId);
                identityResolver.CheckEntitleForPayPalKey(Entitle.CorePayPalKeys, toCheck);
                paymentDb.UpdatePayPalKey(dbPayPalKey);
            }

            return result;
        }

        // delete paypal key
        public void DeletePayPalKey(long payPalKeyId)
        {
            var toCheck = GetPayPalKeyById(payPalKeyId);
            identityResolver.CheckEntitleForPayPalKey(Entitle.CorePayPalKeys, toCheck);
            if (!CanDeletePayPalKey(payPalKeyId))
            {
                throw new InvalidOperationException($"PayPal key with id {payPalKeyId} was assigned to some class and cannot be deleted.");
            }

            paymentDb.DeletePayPalKey(payPalKeyId);
        }


        #region private methods

        // gets PayPalKey by id
        private PayPalKey GetPayPalKeyById(long payPalKeyId)
        {
            var result = paymentDb.GetPayPalKeyById(payPalKeyId);
            if (result == null)
                throw new ArgumentException($"There is no PayPalKey with id {payPalKeyId}.");
            return result;
        }

        private bool CanDeletePayPalKey(long payPalKeyId)
        {
            var result = eventDispatcher.Check(new PayPalAccountDeletingEvent(payPalKeyId));
            return result;
        }

        #endregion

    }

}
