using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Payments;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.AppLogic.Factories
{
    public interface IRegistrationPaymentFactory
    {
        RegistrationPaymentForEdit CreateRegistrationPaymentForEditByClassCurrency(Currency currency);
    }
}
