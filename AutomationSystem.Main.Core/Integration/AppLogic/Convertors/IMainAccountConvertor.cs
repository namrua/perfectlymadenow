using System.Collections.Generic;
using AutomationSystem.Main.Contract.Integration.AppLogic.Models.Accounts;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.WebEx.Contract.Accounts.Models;

namespace AutomationSystem.Main.Core.Integration.AppLogic.Convertors
{
    /// <summary>
    /// Converts Account related objects
    /// </summary>
    public interface IMainAccountConvertor
    {

        // converts MainAccountFilter to AccountFilter
        AccountFilter ConvertToAccountFilter(MainAccountFilter filter, ProfileFilter profileFilter);

        // converts AccountListItem to MainAccountListItem
        MainAccountListItem ConverToMainAccountListItem(AccountListItem item, Dictionary<long, Profile> profileMap);

        // converts AccountForm to MainAccountForm
        MainAccountForm ConvertToMainAccountForm(AccountForm form);

        // converts MainAccountForm to AccountForm
        AccountForm ConvertTAccountForm(MainAccountForm form);

    }

}
