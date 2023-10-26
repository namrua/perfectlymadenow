using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Payment.AppLogic.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Payment.AppLogic
{
    /// <summary>
    /// Converts PayPalKey related objects
    /// </summary>
    public class PayPalKeyConvertor : IPayPalKeyConvertor
    {
        public Lazy<Dictionary<CurrencyEnum, IEnumItem>> currencyMap;

        public PayPalKeyConvertor(IEnumDatabaseLayer enumDb)
        {
            currencyMap = new Lazy<Dictionary<CurrencyEnum, IEnumItem>>(() => 
                enumDb.GetItemsByFilter(EnumTypeEnum.Currency).ToDictionary(x => (CurrencyEnum) x.Id));
        }

        // creates paypal key for edit
        public PayPalKeyForEdit InitializePayPalKeyForEdit()
        {
            var result = new PayPalKeyForEdit
            {
                Currencies = currencyMap.Value.Values.ToList()
            };
            return result;
        }

        // converts PayPalKey to PayPalKeyDetail
        public PayPalKeyListItem ConvertToPayPalKeyListDetail(PayPalKey payPalKey)
        {
            var result = new PayPalKeyListItem
            {
                PayPalKeyId = payPalKey.PayPalKeyId,
                Name = payPalKey.Name,
                Environment = FirstCharUpper(payPalKey.Environment),
                Active = payPalKey.Active,
                UserGroupTypeId = payPalKey.UserGroupTypeId,
                UserGroupId = payPalKey.UserGroupId,
                CurrencyCode = currencyMap.Value[payPalKey.CurrencyId].Name
            };
            return result;
        }


        // converts paypal db model to form
        public PayPalKeyForm ConvertoToPayPalKeyForm(PayPalKey payPalKey)
        {
            var result = new PayPalKeyForm();
            result.PayPalKeyId = payPalKey.PayPalKeyId;
            result.Name = payPalKey.Name;
            result.BraintreeToken = payPalKey.BraintreeToken;
            result.Environment = payPalKey.Environment;
            result.Active = payPalKey.Active;
            result.UserGroupTypeId = payPalKey.UserGroupTypeId;
            result.UserGroupId = payPalKey.UserGroupId;
            result.CurrencyId = payPalKey.CurrencyId;
            return result;
        }

        // converts PayPalKeyForm form to PayPalKey
        public PayPalKey ConvertToPayPalKey(PayPalKeyForm form)
        {
            var result = new PayPalKey();
            result.PayPalKeyId = form.PayPalKeyId;
            result.Name = form.Name;
            result.BraintreeToken = form.BraintreeToken;
            result.Environment = form.Environment;
            result.Active = form.Active;
            result.UserGroupTypeId = form.UserGroupTypeId;
            result.UserGroupId = form.UserGroupId;
            result.CurrencyId = form.CurrencyId;
            return result;
        }


        #region private fields

        // convert to string with upper first char
        private string FirstCharUpper(string value)
        {
            string result = null;
            if (!string.IsNullOrEmpty(value))
                result = value[0].ToString().ToUpper() + value.Substring(1);
            return result;
        }

        #endregion

    }

}
