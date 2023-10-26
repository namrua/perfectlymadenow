using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Integration.AppLogic.Models.Accounts;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.WebEx.Contract.Accounts.Models;

namespace AutomationSystem.Main.Core.Integration.AppLogic.Convertors
{
    /// <summary>
    /// Converts Account related objects
    /// </summary>
    public class MainAccountConvertor : IMainAccountConvertor
    {

        // converts MainAccountFilter to AccountFilter
        public AccountFilter ConvertToAccountFilter(MainAccountFilter filter, ProfileFilter profileFilter)
        {
            filter = filter ?? new MainAccountFilter();
            var result = new AccountFilter
            {
                UserGroupTypeId = UserGroupTypeEnum.MainProfile,
                UserGroupId = filter.ProfileId,
                UserGroupIds = profileFilter.ProfileIds
            };
            return result;
        }

        // converts AccountListItem to MainAccountListItem
        public MainAccountListItem ConverToMainAccountListItem(AccountListItem item, Dictionary<long, Profile> profileMap)
        {
            var result = new MainAccountListItem
            {
                ConferenceAccountId = item.ConferenceAccountId,
                AccountId = item.AccountId,
                Name = item.Name,
                SiteName = item.SiteName,
                Login = item.Login,
                Active = item.Active,
            };

            // fills profile properties
            if (profileMap.TryGetValue(CheckAndGetProfileId(item.UserGroupTypeId, item.UserGroupId), out var profile))
            {
                result.Profile = profile.Name;
            }

            return result;
        }

        // converts AccountForm to MainAccountForm
        public MainAccountForm ConvertToMainAccountForm(AccountForm form)
        {
            var result = new MainAccountForm
            {
                ConferenceAccountId = form.ConferenceAccountId,
                AccountId = form.AccountId,
                ProfileId = CheckAndGetProfileId(form.UserGroupTypeId, form.UserGroupId),
                Name = form.Name,
                SiteName = form.SiteName,
                Login = form.Login,
                Password = form.Password,
                ServiceUrl = form.ServiceUrl,
                Active = form.Active,
                CanDelete = form.CanDelete
            };
            return result;
        }

        // converts MainAccountForm to AccountForm
        public AccountForm ConvertTAccountForm(MainAccountForm form)
        {
            var result = new AccountForm
            {
                ConferenceAccountId = form.ConferenceAccountId,
                AccountId = form.AccountId,
                UserGroupTypeId = UserGroupTypeEnum.MainProfile,
                UserGroupId = form.ProfileId,
                Name = form.Name,
                SiteName = form.SiteName,
                Login = form.Login,
                Password = form.Password,
                ServiceUrl = form.ServiceUrl,
                Active = form.Active,
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
