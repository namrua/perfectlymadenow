using System.Collections.Generic;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.Accounts.Data
{
    /// <summary>
    /// Provides account database layer
    /// </summary>
    public interface IAccountDatabaseLayer
    {

        // gets list of webex accounts by ids
        List<Account> GetAccountsByIds(List<long> ids);

        // gets webEx account by id
        Account GetAccountById(long id);

        // insert webEx account
        long InsertAccount(Account account);

        // update webEx account
        void UpdateAccount(Account model);

        // delete webEx account
        void DeleteAccount(long id);

        // determines whether the account can be deleted
        bool CanDeleteAccount(long accountId);

    }
}
