using AutomationSystem.Base.Contract.Integration;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.ConferenceAccounts.System
{    

    /// <summary>
    /// Converts conference accounts objects
    /// </summary>
    public interface IConferenceAccountConvertor
    {

        // converts conference account to conference account info
        ConferenceAccountInfo ConvertToConferenceAccountInfo(ConferenceAccount conferenceAccount);

        // converts conference account info to conference account
        ConferenceAccount ConvertToConferenceAccount(ConferenceAccountInfo conferenceAccountInfo);

    }

}
