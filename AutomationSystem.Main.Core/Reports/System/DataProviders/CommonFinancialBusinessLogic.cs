using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Reports.System.DataProviders
{
    /// <summary>
    /// Defines common financial business logic procedures
    /// </summary>
    public static class CommonFinancialBusinessLogic
    {
        public static decimal GetTotalRevenue(ClassRegistrationPayment payment)
        {
            var result = (payment.TotalCash ?? 0) + (payment.TotalCheck ?? 0) + (payment.TotalCreditCard ?? 0) + (payment.TotalPayPal ?? 0);
            return result;
        }
    }
}
