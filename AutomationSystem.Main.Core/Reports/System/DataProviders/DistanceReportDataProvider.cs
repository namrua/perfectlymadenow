using System.Linq;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Reports.System.Models.DistanceCrf;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Reports.System.DataProviders
{
    /// <summary>
    /// Provides models for distance reports
    /// </summary>
    public class DistanceReportDataProvider : IDistanceReportDataProvider
    {
        private readonly IDistanceClassDataProvider data;

        public DistanceReportDataProvider(IDistanceClassDataProvider data)
        {
            this.data = data;
        }

        public DistanceCrfReport GetDistanceCrfReportModel()
        {
            var result = new DistanceCrfReport
            {
                Header = GetDistanceCrfHeader(),
                Counts = GetDistanceCrfCounts(),
                Students = data.RegistrationsWithClasses.Select(ConvertToDistanceCrfStudent).ToList()
            };
            return result;
        }

        #region private methods

        private DistanceCrfHeader GetDistanceCrfHeader()
        {
            var coordinator = data.DistanceCoordinator;
            var result = new DistanceCrfHeader
            {
                Coordinator = MainTextHelper.GetFullName(coordinator.Address.FirstName, coordinator.Address.LastName),
                CoordinatorNo = coordinator.CoordinatorNumber ?? 0,
                StartDate = MainTextHelper.GetFullDate(data.Parameters.FromDate),
                EndDate = MainTextHelper.GetFullDate(data.Parameters.ToDate)
            };
            return result;
        }

        private DistanceCrfCounts GetDistanceCrfCounts()
        {
            var result = new DistanceCrfCounts
            {
                Absentee = data.RegistrationsWithClasses.Count
            };
            return result;
        }

        private DistanceCrfStudent ConvertToDistanceCrfStudent(ClassRegistration registration)
        {
            var payment = registration.ClassRegistrationPayment;
            var result = new DistanceCrfStudent
            {
                // class
                ClassDate = MainTextHelper.GetFullDate(registration.Class.EventStart),
                Location = registration.Class.Location,

                // registration
                RegistrantLastName = registration.RegistrantAddress.LastName,
                RegistrantFirstName = registration.RegistrantAddress.FirstName,
                RegistrantAddress = MainTextHelper.GetCompleteAddress(registration.RegistrantAddress),
                RegistrantEmail = registration.RegistrantEmail,

                ParticipantLastName = AddressConvertor.ToLogicString(registration.StudentAddress.LastName),
                ParticipantFirstName = registration.StudentAddress.FirstName,
                ParticipantAddress = MainTextHelper.GetCompleteAddress(registration.StudentAddress),
                ParticipantCountry = registration.StudentAddress.Country.Description,

                RegistrationDate = MainTextHelper.GetFullDate(registration.Approved),

                // payment
                CheckNumber = payment.CheckNumber,
                TransactionNumber = payment.TransactionNumber,
                PayPalFee = payment.PayPalFee,
                TotalPayPal = payment.TotalPayPal,
                NetPayPal = payment.TotalPayPal - payment.PayPalFee,
                TotalCheck = payment.TotalCheck,
                TotalCash = payment.TotalCash,
                TotalCreditCard = payment.TotalCreditCard,
                TotalRevenue = CommonFinancialBusinessLogic.GetTotalRevenue(payment),
            };
            return result;
        }

        #endregion
    }
}
