using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Integration;
using AutomationSystem.Shared.Core.ConferenceAccounts.Data.Extensions;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;

namespace AutomationSystem.Shared.Core.ConferenceAccounts.Data
{
    /// <summary>
    /// Provides conference account database layer
    /// </summary>
    public class ConferenceAccountDatabaseLayer : IConferenceAccountDatabaseLayer
    {               

        // gets list of conference accounts
        public List<ConferenceAccount> GetConferrenceAccountsByFilter(ConferenceAccountFilter filter, bool includeDeleted = false)
        {
            using (var context = new CoreEntities())
            {
                var result = context.ConferenceAccounts.Include("ConferenceAccountType").Filter(filter, includeDeleted).ToList();
                return result;
            }
        }


        // gets conference account by type and settings id       
        public ConferenceAccount GetConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum typeId, long accountSettingsId, bool includeDeleted = false)
        {
            using (var context = new CoreEntities())
            {
                IQueryable<ConferenceAccount> query = context.ConferenceAccounts;
                if (!includeDeleted)
                    query = query.Active();
                var result = query.FirstOrDefault(x => x.ConferenceAccountTypeId == typeId && x.AccountSettingsId == accountSettingsId);                
                return result;
            }
        }


        // checks if conference account is linked to user group
        public bool AnyConferenceAccountOnUserGroup(long userGroupId, UserGroupTypeEnum userGroupTypeId)
        {
            using (var context = new CoreEntities())
            {
                var result = context.ConferenceAccounts.Active().Any(x => x.UserGroupId == userGroupId && x.UserGroupTypeId == userGroupTypeId);
                return result;
            }
        }

        // insert conference account
        public long InsertConferenceAccount(ConferenceAccount model)
        {
            using (var context = new CoreEntities())
            {
                context.ConferenceAccounts.Add(model);
                context.SaveChanges();
                return model.ConferenceAccountId;
            }
        }
       

        // update conference account
        public void UpdateConferenceAccount(ConferenceAccount model)
        {
            using (var context = new CoreEntities())
            {
                // to update
                var toUpdate = context.ConferenceAccounts.FirstOrDefault(x => x.ConferenceAccountId == model.ConferenceAccountId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Conference account with id {model.ConferenceAccountId}.");

                toUpdate.Name = model.Name;                
                toUpdate.Active = model.Active;

                // save changes
                context.SaveChanges();
            }
        }


        // delete conference account by type and settings id
        public void DeleteConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum typeId, long accountSettingsId)
        {
            using (var context = new CoreEntities())
            {
                var toDelete = context.ConferenceAccounts
                    .FirstOrDefault(x => x.ConferenceAccountTypeId == typeId && x.AccountSettingsId == accountSettingsId);
                if (toDelete == null) return;
                context.ConferenceAccounts.Remove(toDelete);
                context.SaveChanges();
            }
        }
       
    }

}
