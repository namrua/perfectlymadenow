using AutomationSystem.Base.Contract.Integration;
using PerfectlyMadeInc.WebEx.Contract.Accounts.Models;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.Accounts.AppLogic
{
    /// <summary>
    /// Converts accounts objects
    /// </summary>
    public interface IAccountConvertor
    {

        // converts AccountFilter to ConferenceAccountFilter
        ConferenceAccountFilter ConvertToConferenceAccountFilter(AccountFilter filter);

        // converts Account and ConferenceAccountInfo to AccountListItem     
        AccountListItem ConvertToAccountListItem(ConferenceAccountInfo conferenceAccount, Account account);

        // converts Account and ConferenceAccountInfo to account form
        AccountForm ConvertToAccountForm(ConferenceAccountInfo conferenceAccount, Account account);

        // converts account form to Account
        Account ConvertToAccount(AccountForm form);

        // converts account form to ConferenceAccountInfo
        ConferenceAccountInfo ConvertToConferenceAccountInfo(AccountForm form);

    }
}
