using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using AutomationSystem.Shared.Contract.Payment.Data;
using AutomationSystem.Main.Core.DistanceProfiles.AppLogic;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Payment.Data.Model;
using AutomationSystem.Shared.Model;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.PriceLists.Data;

namespace AutomationSystem.Main.Tests.DistanceProfiles.AppLogic
{
    public class DistanceProfileFactoryTests
    {
        private readonly Mock<IPaymentDatabaseLayer> paymentDbMock;
        private readonly Mock<IPriceListDatabaseLayer> priceListDbMock;
        private readonly Mock<IPersonDatabaseLayer> personDbMock;

        public DistanceProfileFactoryTests()
        {
            paymentDbMock = new Mock<IPaymentDatabaseLayer>();
            priceListDbMock = new Mock<IPriceListDatabaseLayer>();
            personDbMock = new Mock<IPersonDatabaseLayer>();
        }

        #region CreateDistanceProfileForEdit() tests
        [Fact]
        public void CreateDistanceProfileForEdit_ForPayPalKeysInDb_ReturnsPayPalKeysInResult()
        {
            // arrange
            var payPalKeys = CreatePayPalKeys();
            PayPalKeyFilter filter = null;
            paymentDbMock.Setup(e => e.GetActivePayPalKeys(It.IsAny<long?>(), It.IsAny<PayPalKeyFilter>()))
                .Callback(new Action<long?, PayPalKeyFilter>((i, f) => filter = f))
                .Returns(payPalKeys);
            priceListDbMock.Setup(e => e.GetActivePriceLists(It.IsAny<long?>(), It.IsAny<PriceListTypeEnum>(), It.IsAny<CurrencyEnum>())).Returns(new List<PriceList>());
            personDbMock.Setup(e => e.GetMinimizedPersonsByProfileId(It.IsAny<long?>())).Returns(new List<PersonMinimized>());
            var factory = CreateFactory();

            // act
            var result = factory.CreateDistanceProfileForEdit(1, currentPayPalKeyId: 3);

            // assert
            Assert.Collection(result.PayPalKeys,
                item => Assert.Equal("1", item.Id),
                item => Assert.Equal("2", item.Id));
            Assert.Equal(CurrencyEnum.USD, filter.CurrencyId);
            Assert.Equal(UserGroupTypeEnum.MainProfile, filter.UserGroupTypeId);
            Assert.Equal(1, filter.UserGroupId);
            Assert.True(filter.IsActive);
            paymentDbMock.Verify(e => e.GetActivePayPalKeys(3, It.IsAny<PayPalKeyFilter>()), Times.Once);
        }

        [Fact]
        public void CreateDistanceProfileForEdit_ForPriceListsInDb_ReturnsPriceListsInResult()
        {
            // arrange
            var priceLists = CreatePriceLists();
            paymentDbMock.Setup(e => e.GetActivePayPalKeys(It.IsAny<long?>(), It.IsAny<PayPalKeyFilter>())).Returns(new List<PayPalKey>());
            priceListDbMock.Setup(e => e.GetActivePriceLists(It.IsAny<long?>(), It.IsAny<PriceListTypeEnum>(), It.IsAny<CurrencyEnum>())).Returns(priceLists);
            personDbMock.Setup(e => e.GetMinimizedPersonsByProfileId(It.IsAny<long?>())).Returns(new List<PersonMinimized>());
            var factory = CreateFactory();

            // act
            var result = factory.CreateDistanceProfileForEdit(1, currentPriceListId: 2);

            // assert
            Assert.Collection(result.PriceLists,
                item =>
                {
                    Assert.Equal("first", item.Text);
                    Assert.Equal("1", item.Id);
                },
                item =>
                {
                    Assert.Equal("second", item.Text);
                    Assert.Equal("2", item.Id);
                });
            priceListDbMock.Verify(e => e.GetActivePriceLists(2, PriceListTypeEnum.WwaClass, CurrencyEnum.USD), Times.Once);
        }

        [Fact]
        public void CreateDistanceProfileForEdit_ForPersonsInDb_ReturnsPersonsInResult()
        {
            // arrange
            List<PersonMinimized> personMinimizeds = new List<PersonMinimized>()
            {
                new PersonMinimized
                {
                    PersonId = 11,
                    Name = "firstName lastName"
                },
                new PersonMinimized
                {
                    PersonId = 12,
                    Name = "secondFirstName secondLastName"
                }
            };
            paymentDbMock.Setup(e => e.GetActivePayPalKeys(It.IsAny<long?>(), It.IsAny<PayPalKeyFilter>())).Returns(new List<PayPalKey>());
            priceListDbMock.Setup(e => e.GetActivePriceLists(It.IsAny<long?>(), It.IsAny<PriceListTypeEnum>(), It.IsAny<CurrencyEnum>())).Returns(new List<PriceList>());
            personDbMock.Setup(e => e.GetMinimizedPersonsByProfileId(It.IsAny<long?>())).Returns(personMinimizeds);
            var factory = CreateFactory();

            // act
            var result = factory.CreateDistanceProfileForEdit(1);

            // assert
            Assert.Equal("firstName lastName", result.Persons.GetPersonNameById(11));
            Assert.Equal("secondFirstName secondLastName", result.Persons.GetPersonNameById(12));
            personDbMock.Verify(e => e.GetMinimizedPersonsByProfileId(1), Times.Once);
        }
        #endregion

        #region private methods
        private List<PayPalKey> CreatePayPalKeys()
        {
            return new List<PayPalKey>
            {
                new PayPalKey
                {
                    PayPalKeyId = 1,
                    CurrencyId = CurrencyEnum.USD,
                    UserGroupTypeId = UserGroupTypeEnum.MainProfile,
                    UserGroupId = 1,
                    Active = true,
                },
                new PayPalKey
                {
                    PayPalKeyId = 2,
                    CurrencyId = CurrencyEnum.USD,
                    UserGroupTypeId = UserGroupTypeEnum.MainProfile,
                    UserGroupId = 1,
                    Active = true
                }
            };
        }

        private List<PriceList> CreatePriceLists()
        {
            return new List<PriceList>
            {
                new PriceList
                {
                    Name = "first",
                    PriceListId = 1,
                    CurrencyId = CurrencyEnum.USD,
                    PriceListTypeId = PriceListTypeEnum.WwaClass
                },
                new PriceList
                {
                    Name = "second",
                    PriceListId = 2,
                    CurrencyId = CurrencyEnum.USD,
                    PriceListTypeId = PriceListTypeEnum.WwaClass
                }
            };
        }
        
        private DistanceProfileFactory CreateFactory()
        {
            return new DistanceProfileFactory(
                paymentDbMock.Object,
                priceListDbMock.Object,
                personDbMock.Object);
        }
        #endregion
    }
}
