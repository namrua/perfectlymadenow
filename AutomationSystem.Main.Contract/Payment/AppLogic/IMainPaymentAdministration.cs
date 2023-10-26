using AutomationSystem.Main.Contract.Payment.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Payment.AppLogic
{

    /// <summary>
    /// Service for main payment administration
    /// </summary>
    public interface IMainPaymentAdministration
    {

        // gets list of paypalKeys by filter
        MainPayPalKeyListPageModel GetMainPayPalKeyListPageModel(MainPayPalKeyFilter filter, bool search);

        // gets new paypal for edit
        MainPayPalKeyForEdit GetNewPayPalKeyForEdit(long profileId);

        // gets paypal for edit by paypalkey id
        MainPayPalKeyForEdit GetPayPalKeyForEditById(long payPalKeyId);

        // gets paypal for edit by form
        MainPayPalKeyForEdit GetPayPalKeyForEditByForm(MainPayPalKeyForm form);

        // saves paypalKey
        long SavePayPalKey(MainPayPalKeyForm form);

        // delete paypal key
        void DeletePayPalKey(long payPalKeyId);

    }

}
