using AutomationSystem.Shared.Contract.Payment.AppLogic.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Payment.AppLogic
{
    /// <summary>
    /// Converts PayPalKey related objects
    /// </summary>
    public interface IPayPalKeyConvertor
    {
        // creates paypal key for edit
        PayPalKeyForEdit InitializePayPalKeyForEdit();

        // converts PayPalKey to PayPalKeyDetail
        PayPalKeyListItem ConvertToPayPalKeyListDetail(PayPalKey payPalKey);

        // converts paypal db model to form
        PayPalKeyForm ConvertoToPayPalKeyForm(PayPalKey payPalKey);

        // converts PayPalKeyForm form to PayPalKey
        PayPalKey ConvertToPayPalKey(PayPalKeyForm form);

    }
}