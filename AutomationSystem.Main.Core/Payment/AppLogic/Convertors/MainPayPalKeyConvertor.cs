using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Payment.AppLogic.Models;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Payment.AppLogic.Models;
using AutomationSystem.Shared.Contract.Payment.Data.Model;

namespace AutomationSystem.Main.Core.Payment.AppLogic.Convertors
{

    /// <summary>
    /// Converts PayPalKey related objects
    /// </summary>
    public class MainPayPalKeyConvertor : IMainPayPalKeyConvertor
    {

        // converts MainPayPalKeyFilter to PayPalKeyFilter
        public PayPalKeyFilter ConvertToPayPalKeyFilter(MainPayPalKeyFilter filter, ProfileFilter profileFilter)
        {
            filter = filter ?? new MainPayPalKeyFilter();
            var result = new PayPalKeyFilter
            {
                UserGroupTypeId = UserGroupTypeEnum.MainProfile,
                UserGroupId = filter.ProfileId,
                UserGroupIds = profileFilter.ProfileIds
            };
            return result;
        }

        // converts PayPalKeyListItem to MainPayPalKeyListItem
        public MainPayPalKeyListItem ConvertToMainPayPalKeyListItem(PayPalKeyListItem payPalKey, Dictionary<long, Profile> profileMap)
        {
            var result = new MainPayPalKeyListItem
            {
                PayPalKeyId = payPalKey.PayPalKeyId,
                Name = payPalKey.Name,
                Environment = payPalKey.Environment,
                Active = payPalKey.Active,
                CurrencyCode = payPalKey.CurrencyCode
            };

            // fills profile properties
            if (profileMap.TryGetValue(CheckAndGetProfileId(payPalKey.UserGroupTypeId, payPalKey.UserGroupId), out var profile))
            {
                result.Profile = profile.Name;
            }

            return result;
        }

        // converts PayPalKeyForEdit to MainPayPalKeyForEdit
        public MainPayPalKeyForEdit ConvertToMainPayPalKeyForEdit(PayPalKeyForEdit forEdit)
        {
            var result = new MainPayPalKeyForEdit
            {
                Currencies = forEdit.Currencies,
                Form = ConverToMainPayPalKeyForm(forEdit.Form),
                CanDelete = forEdit.CanDelete
            };
            return result;
        }

        // converts PayPalKeyForm to MainPayPalKeyForm
        public MainPayPalKeyForm ConverToMainPayPalKeyForm(PayPalKeyForm form)
        {
            var result = new MainPayPalKeyForm
            {
                PayPalKeyId = form.PayPalKeyId,
                ProfileId = CheckAndGetProfileId(form.UserGroupTypeId, form.UserGroupId),
                Name = form.Name,
                BraintreeToken = form.BraintreeToken,
                Environment = form.Environment,
                Active = form.Active,
                CurrencyId = form.CurrencyId
            };
            return result;
        }

        // converts MainPayPalKeyForm to PayPalKeyForm
        public PayPalKeyForm ConverToPayPalKeyForm(MainPayPalKeyForm form)
        {
            var result = new PayPalKeyForm
            {
                PayPalKeyId = form.PayPalKeyId,
                UserGroupTypeId = UserGroupTypeEnum.MainProfile,
                UserGroupId = form.ProfileId,
                Name = form.Name,
                BraintreeToken = form.BraintreeToken,
                Environment = form.Environment,
                Active = form.Active,
                CurrencyId = form.CurrencyId
            };
            return result;
        }


        #region private methods

        // checks user group and gets profile id
        public long CheckAndGetProfileId(UserGroupTypeEnum? typeId, long? entityId)
        {
            if (typeId != UserGroupTypeEnum.MainProfile || !entityId.HasValue)
                throw new ArgumentException($"User group '{typeId}-{entityId}' does not refer to Profile.");
            return entityId.Value;
        }

        #endregion

    }

}
