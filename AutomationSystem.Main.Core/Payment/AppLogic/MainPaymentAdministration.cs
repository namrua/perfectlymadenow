using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Payment.AppLogic;
using AutomationSystem.Main.Contract.Payment.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Payment.AppLogic.Convertors;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.System.Extensions;
using AutomationSystem.Shared.Contract.Payment.AppLogic;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Core.Payment.AppLogic
{

    /// <summary>
    /// Service for main payment administration
    /// </summary>
    public class MainPaymentAdmninistration : IMainPaymentAdministration
    {

        private readonly IPaymentAdministration paymentAdministration;
        private readonly IProfileDatabaseLayer profileDb;
        private readonly IClassDatabaseLayer classDb;
        private readonly IIdentityResolver identityResolver;
        private readonly IMainPayPalKeyConvertor payPalKeyConvertor;


        // constructor
        public MainPaymentAdmninistration(
            IPaymentAdministration paymentAdministration,
            IProfileDatabaseLayer profileDb,
            IIdentityResolver identityResolver, 
            IClassDatabaseLayer classDb,
            IMainPayPalKeyConvertor payPalKeyConvertor)
        {
            this.paymentAdministration = paymentAdministration;
            this.profileDb = profileDb;
            this.identityResolver = identityResolver;
            this.classDb = classDb;
            this.payPalKeyConvertor = payPalKeyConvertor;
        }


        // gets list of paypalKeys by filter
        public MainPayPalKeyListPageModel GetMainPayPalKeyListPageModel(MainPayPalKeyFilter filter, bool search)
        {
            var result = new MainPayPalKeyListPageModel(filter);
            result.WasSearched = search;

            // laods profiels
            var profileFilter = identityResolver.GetGrantedProfilesForEntitle(Entitle.CorePayPalKeys);
            var profiles = profileDb.GetProfilesByFilter(profileFilter);
            result.Profiles = profiles.Select(x => DropDownItem.Item(x.ProfileId, x.Name)).ToList();

            // executes searching
            if (search)
            {
                var profileMap = profiles.ToDictionary(x => x.ProfileId);
                var origFilter = payPalKeyConvertor.ConvertToPayPalKeyFilter(filter, profileFilter);
                var payPalKeys = paymentAdministration.GetPayPalKeyListItems(origFilter);
                result.Items = payPalKeys.Select(x => payPalKeyConvertor.ConvertToMainPayPalKeyListItem(x, profileMap)).ToList();
            }
            return result;
        }

        // gets new paypal for edit
        public MainPayPalKeyForEdit GetNewPayPalKeyForEdit(long profileId)
        {
            var origForEdit = paymentAdministration.GetNewPayPalKeyForEdit(UserGroupTypeEnum.MainProfile, profileId);
            var result = payPalKeyConvertor.ConvertToMainPayPalKeyForEdit(origForEdit);
            result.CanDelete = false;
            return result;
        }

        // gets paypal for edit by paypalkey id
        public MainPayPalKeyForEdit GetPayPalKeyForEditById(long payPalKeyId)
        {
            var origForEdit = paymentAdministration.GetPayPalKeyForEditById(payPalKeyId);
            var result = payPalKeyConvertor.ConvertToMainPayPalKeyForEdit(origForEdit);
            return result;
        }

        // gets paypal for edit by form
        public MainPayPalKeyForEdit GetPayPalKeyForEditByForm(MainPayPalKeyForm form)
        {
            var origForEdit = paymentAdministration.GetPayPalKeyForEditByForm(payPalKeyConvertor.ConverToPayPalKeyForm(form));
            var result = payPalKeyConvertor.ConvertToMainPayPalKeyForEdit(origForEdit);
            return result;
        }

        // saves paypalKey
        public long SavePayPalKey(MainPayPalKeyForm form)
        {
            var origForm = payPalKeyConvertor.ConverToPayPalKeyForm(form);
            var result = paymentAdministration.SavePayPalKey(origForm);
            return result;
        }

        // delete paypal key
        public void DeletePayPalKey(long payPalKeyId)
        {
            paymentAdministration.DeletePayPalKey(payPalKeyId);
        }

    }

}
