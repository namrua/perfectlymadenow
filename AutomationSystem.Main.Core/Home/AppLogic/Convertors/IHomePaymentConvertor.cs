using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Payment.Integration.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Main.Core.Home.AppLogic.Convertors
{
    /// <summary>
    /// Converts home payment types
    /// </summary>
    public interface IHomePaymentConvertor
    {
        // converts payment execution summary to PaymentResult
        PaymentResult ConvertToPaymentResult(PaymentExecutionSummary paymentSummary);

        // convert to class registration payment
        ClassRegistrationPayment ConvertToClassRegistrationPayment(PaymentExecutionSummary paymentSummary, long? payPalRecordId);

        // converts to PayPal record
        PayPalRecord ConvertToPayPalRecord(long registrationId, PaymentExecutionSummary paymentSummary, string jsonPayloadDetail);

        // gets payment list item info
        PaymentListItemInfo GetPaymentListItemInfo(decimal price, RegistrationTypeEnum registrationTypeId, ClassCategoryEnum classCategoryId, DateTime classTime);
    }
}