using AutomationSystem.Base.Contract.Integration;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.ConferenceAccounts.System
{
    /// <summary>
    /// Converts conference account objects
    /// </summary>
    public class ConferenceAccountConvertor : IConferenceAccountConvertor
    {

        // converts conference account to conference account info
        public ConferenceAccountInfo ConvertToConferenceAccountInfo(ConferenceAccount conferenceAccount)
        {
            var result = new ConferenceAccountInfo
            {
                ConferenceAccountId = conferenceAccount.ConferenceAccountId,
                Name = conferenceAccount.Name,
                ConferenceAccountTypeId = conferenceAccount.ConferenceAccountTypeId,
                AccountSettingsId = conferenceAccount.AccountSettingsId,
                Active = conferenceAccount.Active,
                UserGroupTypeId = conferenceAccount.UserGroupTypeId,
                UserGroupId = conferenceAccount.UserGroupId,
            };
            return result;
        }

        // converts conference account info to conference account
        public ConferenceAccount ConvertToConferenceAccount(ConferenceAccountInfo conferenceAccountInfo)
        {
            var result = new ConferenceAccount
            {
                ConferenceAccountId = conferenceAccountInfo.ConferenceAccountId,
                Name = conferenceAccountInfo.Name,
                ConferenceAccountTypeId = conferenceAccountInfo.ConferenceAccountTypeId,
                AccountSettingsId = conferenceAccountInfo.AccountSettingsId,
                Active = conferenceAccountInfo.Active,
                UserGroupTypeId = conferenceAccountInfo.UserGroupTypeId,
                UserGroupId = conferenceAccountInfo.UserGroupId,
            };
            return result;
        }

    }

}
