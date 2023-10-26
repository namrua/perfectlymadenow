using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Integration;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.ConferenceAccounts.Data
{

    /// <summary>
    /// Provides conference account database layer
    /// </summary>
    public interface IConferenceAccountDatabaseLayer
    {       

        // gets list of conference accounts
        List<ConferenceAccount> GetConferrenceAccountsByFilter(ConferenceAccountFilter filter, bool includeDeleted = false);

        // gets conference account by type and settings id       
        ConferenceAccount GetConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum typeId, long accountSettingsId, bool includeDeleted = false);

        // checks if conference account is linked to user group
        bool AnyConferenceAccountOnUserGroup(long userGroupId, UserGroupTypeEnum userGroupTypeId);

        // insert conference account
        long InsertConferenceAccount(ConferenceAccount model);
       
        // update conference account
        void UpdateConferenceAccount(ConferenceAccount model);
       
        // delete conference account by type and settings id
        void DeleteConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum typeId, long accountSettingsId);       

    }

}
