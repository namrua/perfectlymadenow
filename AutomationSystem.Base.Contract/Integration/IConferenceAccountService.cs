using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Base.Contract.Integration
{
    /// <summary>
    /// Service for conference accounts
    /// </summary>
    public interface IConferenceAccountService
    {

        // gets list of conference accounts by filter
        List<ConferenceAccountInfo> GetConferenceAccountsByFilter(ConferenceAccountFilter filter, bool includeDeleted = false);

        // gets conference account by type and account settings Id
        ConferenceAccountInfo GetConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum typeId, long accountSettingsId, bool includeDeleted = false);


        // save conference account
        long SaveConferenceAccount(ConferenceAccountInfo conferenceAccount, int ownerId);

        // delete conference account by type and account settings Id
        void DeleteConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum typeId, long accountSettingsId);

    }

}
