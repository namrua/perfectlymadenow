using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.Registrations.AppLogic;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Payments;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Core.Registrations.AppLogic.Factories;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Payment.Data;
using System;

namespace AutomationSystem.Main.Core.Registrations.AppLogic
{
    public class RegistrationPaymentAdministration : IRegistrationPaymentAdministration
    {
        private readonly IIdentityResolver identityResolver;
        private readonly IRegistrationPaymentFactory registrationPaymentFactory;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IMainMapper mainMapper;
        private readonly IPaymentDatabaseLayer paymentDb;
        private readonly IClassOperationChecker classOperationChecker;

        public RegistrationPaymentAdministration(
            IIdentityResolver identityResolver,
            IRegistrationDatabaseLayer registrationDb,
            IRegistrationPaymentFactory registrationPaymentFactory,
            IMainMapper mainMapper,
            IPaymentDatabaseLayer paymentDb,
            IClassOperationChecker classOperationChecker)
        {
            this.identityResolver = identityResolver;
            this.registrationDb = registrationDb;
            this.registrationPaymentFactory = registrationPaymentFactory;
            this.mainMapper = mainMapper;
            this.paymentDb = paymentDb;
            this.classOperationChecker = classOperationChecker;
        }

        public RegistrationPaymentDetailPageModel GetRegistrationPaymentByRegistrationId(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.ClassRegistrationPayment | ClassRegistrationIncludes.ClassCurrency);
            identityResolver.CheckEntitleForClass(registration.Class);

            var payPalRecord = paymentDb.GetPayPalRecordById(registration.ClassRegistrationPayment.PayPalRecordId ?? 0);
            var result = new RegistrationPaymentDetailPageModel
            {
                ClassId = registration.ClassId,
                ClassRegistrationId = registrationId,
                CurrencyCode = registration.Class.Currency.Name,
                ClassState = ClassConvertor.GetClassState(registration.Class),
                Detail = mainMapper.Map<RegistrationPaymentDetail>(registration.ClassRegistrationPayment),
                PayPalRecord = payPalRecord == null ? null : mainMapper.Map<PayPalRecordDetail>(payPalRecord)
            };
            result.CanEdit = classOperationChecker.IsOperationAllowed(ClassOperation.EditPayment, result.ClassState);

            return result;
        }
        
        public RegistrationPaymentForEdit GetRegistrationPaymentForEditById(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.ClassRegistrationPayment | ClassRegistrationIncludes.ClassCurrency);
            identityResolver.CheckEntitleForClass(registration.Class);

            var result = registrationPaymentFactory.CreateRegistrationPaymentForEditByClassCurrency(registration.Class.Currency);
            result.Form = mainMapper.Map<RegistrationPaymentForm>(registration);
            return result;
        }
        
        public RegistrationPaymentForEdit GetRegistrationPaymentForEditByForm(RegistrationPaymentForm form)
        {
            var registration = GetRegistrationById(form.ClassRegistrationId, ClassRegistrationIncludes.ClassCurrency);
            identityResolver.CheckEntitleForClass(registration.Class);
            var result = registrationPaymentFactory.CreateRegistrationPaymentForEditByClassCurrency(registration.Class.Currency);
            result.Form = form;
            return result;
        }
        
        public void SaveRegistrationPayment(RegistrationPaymentForm form)
        {
            var toCheck = GetRegistrationById(form.ClassRegistrationId, ClassRegistrationIncludes.Class);
            identityResolver.CheckEntitleForClass(toCheck.Class);

            var dbPayment = mainMapper.Map<ClassRegistrationPayment>(form);
            registrationDb.UpdateClassRegistrationPayment(form.ClassRegistrationId, dbPayment);
        }

        #region private methods
        private ClassRegistration GetRegistrationById(long registrationId, ClassRegistrationIncludes includes = ClassRegistrationIncludes.None)
        {
            var result = registrationDb.GetClassRegistrationById(registrationId, includes);
            if (result == null)
                throw new ArgumentException($"There is no Class registration with id {registrationId}.");
            return result;
        }
        #endregion
    }
}
