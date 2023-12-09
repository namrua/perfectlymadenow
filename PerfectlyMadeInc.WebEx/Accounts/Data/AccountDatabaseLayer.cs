using System;
using System.Collections.Generic;
using System.Linq;
using PerfectlyMadeInc.WebEx.Model;
using PerfectlyMadeInc.WebEx.Model.Queries;

namespace PerfectlyMadeInc.WebEx.Accounts.Data
{

    /// <summary>
    /// Provides webex account data layer
    /// </summary>
    public class AccountDatabaseLayer : IAccountDatabaseLayer
    {

        // gets list of webEx accounts by ids
        public List<Account> GetAccountsByIds(List<long> ids)
        {
            using (var context = new WebExEntities())
            {
                var result = context.Accounts.Active().Where(x => ids.Contains(x.AccountId)).ToList();
                return result;
            }
        }


        // gets webEx account by id
        public Account GetAccountById(long id)
        {
            using (var context = new WebExEntities())
            {
                var result = context.Accounts.Active().FirstOrDefault(x => x.AccountId == id);
                if (result == null)
                    throw new ArgumentException($"There is no WebEx account with id {id}.");
                return result;
            }
        }

        public UserAccount GetUserAccountById(long id)
        {
            using (var context = new WebExEntities())
            {
                var result = context.UserAccounts.FirstOrDefault(x => x.AccountId == id);
                if (result == null)
                    throw new ArgumentException($"There is no WebEx account with id {id}.");
                return result;
            }
        }


        // insert webEx account
        public long InsertAccount(Account model)
        {
            using (var context = new WebExEntities())
            {
                context.Accounts.Add(model);
                context.SaveChanges();
                return model.AccountId;
            }
        }

        // update webEx account
        public void UpdateAccount(Account account)
        {
            using (var context = new WebExEntities())
            {
                // to update
                var toUpdate = context.Accounts.FirstOrDefault(x => x.AccountId == account.AccountId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no WebEx account with id {account.AccountId}.");

                toUpdate.SiteName = account.SiteName;
                toUpdate.Login = account.Login;
                toUpdate.Password = account.Password;
                toUpdate.ServiceUrl = account.ServiceUrl;

                // save changes
                context.SaveChanges();
            }
        }


        // delete webEx account
        public void DeleteAccount(long id)
        {
            using (var context = new WebExEntities())
            {
                var toDelete = context.Accounts.FirstOrDefault(x => x.AccountId == id);
                if (toDelete == null) return;
                context.Accounts.Remove(toDelete);
                context.SaveChanges();
            }
        }


        // determines whether the account can be deleted
        public bool CanDeleteAccount(long accountId)
        {
            using (var context = new WebExEntities())
            {
                if (context.Programs.Active().Any(x => x.AccountId == accountId))
                    return false;

                return true;
            }
        }

    }

}
