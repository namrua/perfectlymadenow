using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Payments;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.AppLogic.Factories
{
    /// <summary>
    /// Creates Registration payment objects
    /// </summary>
    public class RegistrationPaymentFactory : IRegistrationPaymentFactory
    {
        
        public RegistrationPaymentForEdit CreateRegistrationPaymentForEditByClassCurrency(Currency currency)
        {
            var result = new RegistrationPaymentForEdit
            {
                CurrencyCode = currency.Name
            };
            return result;
        }
    }
}
