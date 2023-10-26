using System;
using AutoMapper;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Payments;
using AutomationSystem.Main.Core.Registrations.AppLogic.MappingProfiles;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Model;
using Xunit;

namespace AutomationSystem.Main.Tests.Registrations.AppLogic.MappingProfiles
{
    public class RegistrationPaymentProfileTests
    {
        private readonly ClassRegistrationPayment _classRegistrationPayment;

        public RegistrationPaymentProfileTests()
        {
            _classRegistrationPayment = new ClassRegistrationPayment
            {
                ClassRegistrationPaymentId = 2,
                CheckNumber = "CheckNumber",
                TransactionNumber = "TransactionNumber",
                PayPalFee = 1.0M,
                TotalPayPal = 1.1M,
                TotalCheck = 1.2M,
                TotalCash = 1.3M,
                TotalCreditCard = 1.4M,
                IsPaidPmi = false,
                IsAbsentee = true,
            };
        }

        #region CreateMap<ClassRegistrationPayment, RegistrationPaymentDetail>() tests
        [Fact]
        public void Map_ClassRegistrationPayment_ReturnsRegistrationPaymentDetail()
        {
            // arrange
            var mapper = CreateMapper();

            // act
            var detail = mapper.Map<RegistrationPaymentDetail>(_classRegistrationPayment);

            // assert
            Assert.Equal(2, detail.ClassRegistrationPaymentId);
            Assert.Equal("CheckNumber", detail.CheckNumber);
            Assert.Equal("TransactionNumber", detail.TransactionNumber);
            Assert.Equal(1.0M, detail.PayPalFee);
            Assert.Equal(1.1M, detail.TotalPayPal);
            Assert.Equal(1.2M, detail.TotalCheck);
            Assert.Equal(1.3M, detail.TotalCash);
            Assert.Equal(1.4M, detail.TotalCreditCard);
            Assert.False(detail.IsPaidPmi);
            Assert.True(detail.IsAbsentee);
        }
        #endregion

        #region CreateMap<ClassRegistrationPayment, RegistrationPaymentForm>() tests
        [Fact]
        public void Map_ClassRegistrationPayment_ReturnsRegistrationPaymentForm()
        {
            // arrange
            var mapper = CreateMapper();

            // act
            var form = mapper.Map<RegistrationPaymentForm>(_classRegistrationPayment);

            // assert
            Assert.Equal(0, form.ClassRegistrationId);
            Assert.Equal("CheckNumber", form.CheckNumber);
            Assert.Equal("TransactionNumber", form.TransactionNumber);
            Assert.Equal(1.0M, form.PayPalFee);
            Assert.Equal(1.1M, form.TotalPayPal);
            Assert.Equal(1.2M, form.TotalCheck);
            Assert.Equal(1.3M, form.TotalCash);
            Assert.Equal(1.4M, form.TotalCreditCard);
            Assert.False(form.IsPaidPmi);
            Assert.True(form.IsAbsentee);
        }
        #endregion

        #region CreateMap<ClassRegistration, RegistrationPaymentForm>() tests
        [Fact]
        public void Map_ClassRegistrationPaymentIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var registration = new ClassRegistration
            {
                ClassRegistrationPayment = null
            };
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<RegistrationPaymentForm>(registration));
        }

        [Fact]
        public void Map_ClassRegistration_ReturnsRegistrationPaymentForm()
        {
            // arrange
            var registration = new ClassRegistration
            {
                ClassRegistrationId = 1,
                ClassRegistrationPayment = _classRegistrationPayment
            };
            var mapper = CreateMapper();

            // act
            var form = mapper.Map<RegistrationPaymentForm>(registration);

            // assert
            Assert.Equal(1, form.ClassRegistrationId);
            Assert.Equal("CheckNumber", form.CheckNumber);
            Assert.Equal("TransactionNumber", form.TransactionNumber);
            Assert.Equal(1.0M, form.PayPalFee);
            Assert.Equal(1.1M, form.TotalPayPal);
            Assert.Equal(1.2M, form.TotalCheck);
            Assert.Equal(1.3M, form.TotalCash);
            Assert.Equal(1.4M, form.TotalCreditCard);
            Assert.False(form.IsPaidPmi);
            Assert.True(form.IsAbsentee);
        }
        #endregion
        
        #region CreateMap<RegistrationPaymentForm, ClassRegistrationPayment>() tests
        [Fact]
        public void Map_RegistrationPaymentForm_ReturnsClassRegistrationPayment()
        {
            //arrange
            var registrationPaymentForm = new RegistrationPaymentForm
            {
                CheckNumber = "CheckNumber",
                TransactionNumber = "TransactionNumber",
                PayPalFee = 1.0M,
                TotalPayPal = 1.1M,
                TotalCheck = 1.2M,
                TotalCash = 1.3M,
                TotalCreditCard = 1.4M,
                IsPaidPmi = false,
                IsAbsentee = true,
            };
            var mapper = CreateMapper();
            
            //act
            var classRegistrationPayment = mapper.Map<ClassRegistrationPayment>(registrationPaymentForm);

            //assert
            Assert.Equal("CheckNumber", classRegistrationPayment.CheckNumber);
            Assert.Equal("TransactionNumber", classRegistrationPayment.TransactionNumber);
            Assert.Equal(1.0M, classRegistrationPayment.PayPalFee);
            Assert.Equal(1.1M, classRegistrationPayment.TotalPayPal);
            Assert.Equal(1.2M, classRegistrationPayment.TotalCheck);
            Assert.Equal(1.3M, classRegistrationPayment.TotalCash);
            Assert.Equal(1.4M, classRegistrationPayment.TotalCreditCard);
            Assert.False(classRegistrationPayment.IsPaidPmi);
            Assert.True(classRegistrationPayment.IsAbsentee);
        }
        #endregion

        #region CreateMap<PayPalRecord, PayloadAddressDetail>() tests
        [Fact]
        public void Map_PayPalRecord_ReturnsPayloadAddressDetail()
        {
            // arrange
            var record = CreatePayPalRecord();
            var mapper = CreateMapper();

            // act
            var detail = mapper.Map<PayloadAddressDetail>(record);

            // assert
            Assert.Equal("SaRecipientName", detail.RecipientName);
            Assert.Equal("SaCity", detail.City);
            Assert.Equal("SaCountryCode", detail.CountryCode);
            Assert.Equal("SaPostalCode", detail.PostalCode);
            Assert.Equal("SaState", detail.State);
            Assert.Equal("SaCity, SaState SaPostalCode", detail.FullCity);
            Assert.Equal("SaLine1", detail.Line1);
            Assert.Equal("SaLine2", detail.Line2);
            Assert.Equal("SaLine1, SaLine2", detail.FullStreet);
        }
        #endregion

        #region CreateMap<PayPalRecord, PayPalRecordDetail>() tests
        [Fact]
        public void Map_PayPalRecord_ReturnsPayPalRecordDetail()
        {
            // arrange
            var record = CreatePayPalRecord();
            var mapper = CreateMapper();

            // act
            var detail = mapper.Map<PayPalRecordDetail>(record);

            // assert
            Assert.Equal("PayPalId", detail.PayPalId);
            Assert.Equal(1.0M, detail.Total);
            Assert.Equal(1.1M, detail.Fee);
            Assert.Equal("PayerId", detail.PayerId);
            Assert.Equal("FirstName", detail.FirstName);
            Assert.Equal("LastName", detail.LastName);
            Assert.Equal("Email", detail.Email);
            Assert.Equal("CountryCode", detail.CountryCode);
            Assert.Equal("FirstName LastName", detail.FullName);
            Assert.Equal("Transaction number", detail.TransactionId);
        }
        #endregion

        #region private methods

        private Mapper CreateMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new RegistrationPaymentProfile());
            });
            return new Mapper(mapperConfiguration);
        }

        private PayPalRecord CreatePayPalRecord()
        {
            return new PayPalRecord
            {
                PayPalId = "PayPalId",
                Amount = 1.0M,
                Fee = 1.1M,
                PayerId = "PayerId",
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                CountryCode = "CountryCode",
                SaRecipientName = "SaRecipientName",
                SaLine1 = "SaLine1",
                SaLine2 = "SaLine2",
                SaCity = "SaCity",
                SaState = "SaState",
                SaPostalCode = "SaPostalCode",
                SaCountryCode = "SaCountryCode",
                TransactionId = "Transaction number"
            };
        }
        #endregion
    }
}
