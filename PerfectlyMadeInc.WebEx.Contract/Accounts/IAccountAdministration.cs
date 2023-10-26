using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.WebEx.Contract.Accounts.Models;

namespace PerfectlyMadeInc.WebEx.Contract.Accounts
{
    /// <summary>
    /// Account administration
    /// </summary>
    public interface IAccountAdministration
    {

        // gets list of webex accounts
        List<AccountListItem> GetAccounts(AccountFilter filter = null);

        // gets new account form
        AccountForm GetNewAccountForm(UserGroupTypeEnum? userGroupTypeId, long? userGroupId);

        // gets webex account form by accountId
        AccountForm GetAccountFormById(long accountId);

        // save account
        long SaveAccount(AccountForm form);

        // delete account by accountId
        void DeleteAccount(long accountId);

    }
}
