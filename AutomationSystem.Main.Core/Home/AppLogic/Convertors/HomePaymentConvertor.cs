using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Home.AppLogic.Models;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Incidents.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Contract.Payment.Integration.Models;
using AutomationSystem.Shared.Model;
using Newtonsoft.Json;
using PerfectlyMadeInc.Helpers.Database;

namespace AutomationSystem.Main.Core.Home.AppLogic.Convertors
{

    /// <summary>
    /// Converts home payment types
    /// </summary>
    public class HomePaymentConvertor : IHomePaymentConvertor
    {
        private readonly IIncidentLogger incidentLogger;
        private readonly IRegistrationTypeResolver registrationTypeResolver;

        public HomePaymentConvertor(IIncidentLogger incidentLogger, IRegistrationTypeResolver registrationTypeResolver)
        {
            this.incidentLogger = incidentLogger;
            this.registrationTypeResolver = registrationTypeResolver;
        }

        // converts payment execution summary to PaymentResult
        public PaymentResult ConvertToPaymentResult(PaymentExecutionSummary paymentSummary)
        {
            var result = new PaymentResult
            {
                IsSuccess = paymentSummary.IsSuccess,
                Message = paymentSummary.Message
            };
            return result;
        }

        // convert to class registration payment
        public ClassRegistrationPayment ConvertToClassRegistrationPayment(PaymentExecutionSummary paymentSummary, long? payPalRecordId)
        {
            var result = new ClassRegistrationPayment
            {
                TransactionNumber = paymentSummary.TransactionId,
                PayPalFee = paymentSummary.PaypalFee,
                TotalPayPal = paymentSummary.Amount,
                PayPalRecordId = payPalRecordId
            };
            return result;
        }


        // converts to PayPal record
        public PayPalRecord ConvertToPayPalRecord(long registrationId, PaymentExecutionSummary paymentSummary, string jsonPayloadDetail)
        {
            PayloadDetails payloadDetails = null;
            try
            {               
                payloadDetails = JsonConvert.DeserializeObject<PayloadDetails>(jsonPayloadDetail);
            }
            catch (Exception e)
            {
                var incident = IncidentForLog.New(IncidentTypeEnum.PayPalError, $"Invalid payload details.", $"{jsonPayloadDetail}\n\n{e}");
                incident.Entity(EntityTypeEnum.MainClassRegistration, registrationId);
                incidentLogger.LogIncident(incident);
            }

            // assembes result
            payloadDetails = payloadDetails ?? new PayloadDetails();
            var shippmentAddress = payloadDetails.ShippingAddress ?? new PayloadShippingAddress();
            var result = new PayPalRecord
            {
                Amount = paymentSummary.Amount,
                Fee = paymentSummary.PaypalFee,
                FeeText = DatabaseHelper.TrimNVarchar(paymentSummary.PaypalFeeText, false, 16),
                PayPalId = paymentSummary.PaypalId ?? "------",

                CountryCode = DatabaseHelper.TrimNVarchar(payloadDetails.CountryCode, false, 8),
                Email = DatabaseHelper.TrimNVarchar(payloadDetails.Email, false, 128),
                FirstName = DatabaseHelper.TrimNVarchar(payloadDetails.FirstName, false, 64),
                LastName = DatabaseHelper.TrimNVarchar(payloadDetails.LastName, false, 64),
                PayerId = DatabaseHelper.TrimNVarchar(payloadDetails.PayerId, false, 64),

                SaRecipientName = DatabaseHelper.TrimNVarchar(shippmentAddress.RecipientName, false, 128),
                SaLine1 = DatabaseHelper.TrimNVarchar(shippmentAddress.Line1, false, 64),
                SaLine2 = DatabaseHelper.TrimNVarchar(shippmentAddress.Line2, false, 64),
                SaCity = DatabaseHelper.TrimNVarchar(shippmentAddress.City, false, 64),
                SaState = DatabaseHelper.TrimNVarchar(shippmentAddress.State, false, 64),
                SaPostalCode = DatabaseHelper.TrimNVarchar(shippmentAddress.PostalCode, false, 16),
                SaCountryCode = DatabaseHelper.TrimNVarchar(shippmentAddress.CountryCode, false, 8),
                TransactionId = DatabaseHelper.TrimNVarchar(paymentSummary.TransactionId, false, 32)
            };
            return result;
        }


        // gets payment list item info
        public PaymentListItemInfo GetPaymentListItemInfo(decimal price, RegistrationTypeEnum registrationTypeId, ClassCategoryEnum classCategoryId, DateTime classTime)
        {
            var result = new PaymentListItemInfo();
            result.Price = price;
            var registrationTypeCode = registrationTypeResolver.GetRegistrationTypeCode(registrationTypeId, classCategoryId);           
            result.PayPalId = $"{registrationTypeCode} {MainTextHelper.GetBriefDate(classTime)}";
            result.ItemName = result.PayPalId;
            return result;
        }

    }

}
