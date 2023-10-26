using AutomationSystem.Main.Contract.Integration.AppLogic.Models.Accounts;

namespace AutomationSystem.Main.Contract.Integration.AppLogic
{
    /// <summary>
    /// Main account administration
    /// </summary>
    public interface IMainAccountAdministration
    {
        // gets list of webex accounts by filter
        MainAccountListPageModel GetMainAccountListPageModel(MainAccountFilter filter, bool search);

        // gets new account form
        MainAccountForm GetNewAccountForm(long profileId);

        // gets webex account form by accountId
        MainAccountForm GetAccountFormById(long accountId);

        // save account
        long SaveAccount(MainAccountForm form);

        // delete account by accountId
        void DeleteAccount(long accountId);

    }

}
