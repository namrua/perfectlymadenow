using System.Collections.Generic;
using AutomationSystem.Main.Contract.Payment.AppLogic.Models;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Payment.AppLogic.Models;
using AutomationSystem.Shared.Contract.Payment.Data.Model;

namespace AutomationSystem.Main.Core.Payment.AppLogic.Convertors
{
    /// <summary>
    /// Converts PayPalKey related objects
    /// </summary>
    public interface IMainPayPalKeyConvertor
    {

        // converts MainPayPalKeyFilter to PayPalKeyFilter
        PayPalKeyFilter ConvertToPayPalKeyFilter(MainPayPalKeyFilter filter, ProfileFilter profileFilter);

        // converts PayPalKeyListItem to MainPayPalKeyListItem
        MainPayPalKeyListItem ConvertToMainPayPalKeyListItem(PayPalKeyListItem payPalKey, Dictionary<long, Profile> profileMap);

        // converts PayPalKeyForEdit to MainPayPalKeyForEdit
        MainPayPalKeyForEdit ConvertToMainPayPalKeyForEdit(PayPalKeyForEdit forEdit);

        // converts PayPalKeyForm to MainPayPalKeyForm
        MainPayPalKeyForm ConverToMainPayPalKeyForm(PayPalKeyForm form);

        // converts MainPayPalKeyForm to PayPalKeyForm
        PayPalKeyForm ConverToPayPalKeyForm(MainPayPalKeyForm form);

    }

}
