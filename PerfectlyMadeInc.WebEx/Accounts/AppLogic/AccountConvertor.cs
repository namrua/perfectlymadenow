using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Integration;
using PerfectlyMadeInc.WebEx.Contract.Accounts.Models;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.Accounts.AppLogic
{

    /// <summary>
    /// Converts accounts objects
    /// </summary>
    public class AccountConvertor : IAccountConvertor
    {

        // converts AccountFilter to ConferenceAccountFilter
        public ConferenceAccountFilter ConvertToConferenceAccountFilter(AccountFilter filter)
        {
            filter = filter ?? new AccountFilter();
            var result = new ConferenceAccountFilter
            {
                ConferenceAccountTypeId = ConferenceAccountTypeEnum.WebEx,
                UserGroupTypeId = filter.UserGroupTypeId,
                UserGroupId = filter.UserGroupId,
                UserGroupIds = filter.UserGroupIds
            };
            return result;
        }


        // converts Account and ConferenceAccountInfo to AccountListItem     
        public AccountListItem ConvertToAccountListItem(ConferenceAccountInfo conferenceAccount, Account account)
        {
            var result = new AccountListItem
            {
                Active = conferenceAccount.Active,
                ConferenceAccountId = conferenceAccount.ConferenceAccountId,
                Name = conferenceAccount.Name,                
                AccountId = account.AccountId,
                SiteName = account.SiteName,
                Login = account.Login,
                UserGroupTypeId = conferenceAccount.UserGroupTypeId,
                UserGroupId = conferenceAccount.UserGroupId
            };

            return result;
        }

        // converts Account and ConferenceAccountInfo to account form
        public AccountForm ConvertToAccountForm(ConferenceAccountInfo conferenceAccount, Account account)
        {
            var result = new AccountForm
            {
                ConferenceAccountId = conferenceAccount.ConferenceAccountId,
                Name = conferenceAccount.Name,
                Active = conferenceAccount.Active,
                AccountId = account.AccountId,
                SiteName = account.SiteName,
                Login = account.Login,
                Password = account.Password,
                ServiceUrl = account.ServiceUrl,
                UserGroupTypeId = conferenceAccount.UserGroupTypeId,
                UserGroupId = conferenceAccount.UserGroupId
            };

            return result;
        }


        // converts account form to Account
        public Account ConvertToAccount(AccountForm form)
        {
            var result = new Account
            {
                AccountId = form.AccountId,
                SiteName = form.SiteName,
                Login = form.Login,
                Password = form.Password,
                ServiceUrl = form.ServiceUrl
            };            
            return result;
        }


        // converts account form to ConferenceAccountInfo
        public ConferenceAccountInfo ConvertToConferenceAccountInfo(AccountForm form)
        {
            var result = new ConferenceAccountInfo
            {
                ConferenceAccountId = form.ConferenceAccountId,
                Name = form.Name,
                Active = form.Active,
                ConferenceAccountTypeId = ConferenceAccountTypeEnum.WebEx,
                AccountSettingsId = form.AccountId,
                UserGroupTypeId = form.UserGroupTypeId,
                UserGroupId = form.UserGroupId
            };

            return result;
        }

    }

}
