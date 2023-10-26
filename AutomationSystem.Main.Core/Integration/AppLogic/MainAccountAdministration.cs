using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Integration.AppLogic;
using AutomationSystem.Main.Contract.Integration.AppLogic.Models.Accounts;
using AutomationSystem.Main.Core.Integration.AppLogic.Convertors;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.System.Extensions;
using CorabeuControl.Components;
using PerfectlyMadeInc.WebEx.Contract.Accounts;

namespace AutomationSystem.Main.Core.Integration.AppLogic
{
    /// <summary>
    /// Main account administration
    /// </summary>
    public class MainAccountAdministration : IMainAccountAdministration
    {
        private readonly IAccountAdministration accountAdministration;
        private readonly IProfileDatabaseLayer profileDb;
        private readonly IIdentityResolver identityResolver;
        private readonly IMainAccountConvertor accountConvertor;


        // constructor
        public MainAccountAdministration(
            IAccountAdministration accountAdministration,
            IProfileDatabaseLayer profileDb,
            IIdentityResolver identityResolver,
            IMainAccountConvertor accountConvertor)
        {
            this.accountAdministration = accountAdministration;
            this.profileDb = profileDb;
            this.identityResolver = identityResolver;
            this.accountConvertor = accountConvertor;
        }


        // gets list of webex accounts by filter
        public MainAccountListPageModel GetMainAccountListPageModel(MainAccountFilter filter, bool search)
        {
            var result = new MainAccountListPageModel(filter);
            result.WasSearched = search;

            // loads profiles
            var profileFilter = identityResolver.GetGrantedProfilesForEntitle(Entitle.WebExAccounts);
            var profiles = profileDb.GetProfilesByFilter(profileFilter);
            result.Profiles = profiles.Select(x => DropDownItem.Item(x.ProfileId, x.Name)).ToList();

            // executes searching
            if (search)
            {
                var profileMap = profiles.ToDictionary(x => x.ProfileId);
                var origFilter = accountConvertor.ConvertToAccountFilter(filter, profileFilter);
                var accounts = accountAdministration.GetAccounts(origFilter);
                result.Items = accounts.Select(x => accountConvertor.ConverToMainAccountListItem(x, profileMap))
                    .ToList();
            }

            return result;
        }

        // gets new account form
        public MainAccountForm GetNewAccountForm(long profileId)
        {
            var origForm = accountAdministration.GetNewAccountForm(UserGroupTypeEnum.MainProfile, profileId);
            var result = accountConvertor.ConvertToMainAccountForm(origForm);
            return result;
        }

        // gets webex account form by accountId
        public MainAccountForm GetAccountFormById(long accountId)
        {
            var origForm = accountAdministration.GetAccountFormById(accountId);
            var result = accountConvertor.ConvertToMainAccountForm(origForm);
            return result;
        }

        // save account
        public long SaveAccount(MainAccountForm form)
        {
            var origForm = accountConvertor.ConvertTAccountForm(form);
            var result = accountAdministration.SaveAccount(origForm);
            return result;
        }

        // delete account by accountId
        public void DeleteAccount(long accountId)
        {
            accountAdministration.DeleteAccount(accountId);
        }
    }
}