using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Extensions;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Base.Contract.Integration;
using PerfectlyMadeInc.WebEx.Accounts.AppLogic;
using PerfectlyMadeInc.WebEx.Accounts.Data;
using PerfectlyMadeInc.WebEx.Contract.Accounts;
using PerfectlyMadeInc.WebEx.Contract.Accounts.Models;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.Accounts
{
    /// <summary>
    /// Account administration
    /// </summary>
    public class AccountAdministration : IAccountAdministration
    {

        private readonly IAccountDatabaseLayer accountDb;
        private readonly IConferenceAccountService conferenceAccountService;
        private readonly IIdentityResolver identityResolver;

        private readonly IAccountConvertor accountConvertor;


        // constructor
        public AccountAdministration(IAccountDatabaseLayer accountDb, IConferenceAccountService conferenceAccountService,
            IIdentityResolver identityResolver)
        {
            this.accountDb = accountDb;
            this.conferenceAccountService = conferenceAccountService;
            this.identityResolver = identityResolver;

            accountConvertor = new AccountConvertor();
        }


        // gets list of webex accounts
        public List<AccountListItem> GetAccounts(AccountFilter filter = null)
        {
            // gets conference accounts and webex accounts
            var confAccountFilter = accountConvertor.ConvertToConferenceAccountFilter(filter);
            var conAccounts = conferenceAccountService.GetConferenceAccountsByFilter(confAccountFilter);

            // todo: #BICH - batch item check

            var accountIds = conAccounts.Select(x => x.AccountSettingsId).ToList();
            var accounts = accountDb.GetAccountsByIds(accountIds);

            // join accounts to result
            var result = conAccounts.Join(accounts, x => x.AccountSettingsId, y => y.AccountId,
                accountConvertor.ConvertToAccountListItem).OrderBy(x => x.Name).ToList();
            return result;
        }


        // gets new account form
        public AccountForm GetNewAccountForm(UserGroupTypeEnum? userGroupTypeId, long? userGroupId)
        {
            // checks access rights
            identityResolver.CheckEntitleForUserGroup(Entitle.WebExAccounts, userGroupTypeId, userGroupId);

            // creates new AccountForm
            var result = new AccountForm();
            result.Active = true;
            result.UserGroupTypeId = userGroupTypeId;
            result.UserGroupId = userGroupId;
            result.CanDelete = false;
            return result;
        }


        // gets webex account form by accountId
        public AccountForm GetAccountFormById(long accountId)
        {
            var account = GetAccountById(accountId);
            var conAccount = conferenceAccountService.GetConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum.WebEx, accountId);
            identityResolver.CheckEntitleForConferenceAccountInfo(Entitle.WebExAccounts, conAccount);
            var result = accountConvertor.ConvertToAccountForm(conAccount, account);
            result.CanDelete = CanDeleteAccount(accountId);
            return result;
        }


        // save account
        public long SaveAccount(AccountForm form)
        {
            var account = accountConvertor.ConvertToAccount(form);
            var conAccount = accountConvertor.ConvertToConferenceAccountInfo(form);
            var result = account.AccountId;
            if (result == 0)
            {
                identityResolver.CheckEntitleForConferenceAccountInfo(Entitle.WebExAccounts, conAccount);
                result = accountDb.InsertAccount(account);
                conAccount.AccountSettingsId = result;                              
            }
            else
            {
                // check identities and account-conference account pairing consistency
                var toCheck = conferenceAccountService.GetConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum.WebEx, form.AccountId);
                if (toCheck.ConferenceAccountId != form.ConferenceAccountId)
                    throw new SecurityException(
                        $"Conference account with id {toCheck.ConferenceAccountId} is inconsistent with form's conference account id {form.ConferenceAccountId}.");
                identityResolver.CheckEntitleForConferenceAccountInfo(Entitle.WebExAccounts, toCheck);

                // saves account
                accountDb.UpdateAccount(account);
            }
            conferenceAccountService.SaveConferenceAccount(conAccount, identityResolver.GetOwnerId());
            return result;
        }


        // delete conference account by accountId
        public void DeleteAccount(long accountId)
        {
            var toCheck = conferenceAccountService.GetConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum.WebEx, accountId);
            identityResolver.CheckEntitleForConferenceAccountInfo(Entitle.WebExAccounts, toCheck);

            if (!CanDeleteAccount(accountId))
                throw new InvalidOperationException($"WebEx account with id {accountId} has some programs assigned and cannot be deleted.");

            accountDb.DeleteAccount(accountId);
            conferenceAccountService.DeleteConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum.WebEx, accountId);
        }


        #region private methods

        // gets account by id
        private Account GetAccountById(long accountId)
        {
            var account = accountDb.GetAccountById(accountId);
            if (account == null)
                throw new ArgumentException($"There is no WebEx account with id {accountId}.");
            return account;
        }


        // determines whether the account can be deleted
        private bool CanDeleteAccount(long accountId)
        {
            var result = accountDb.CanDeleteAccount(accountId);
            return result;
        }

        #endregion

    }

}
