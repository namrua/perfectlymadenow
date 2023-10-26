using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.PriceLists.AppLogic;
using AutomationSystem.Main.Core.PriceLists.System;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Enums.Data.Models;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.PriceLists
{
    public class PriceListFactoryTests
    {
        private readonly Mock<IEnumDatabaseLayer> enumDbMock;
        private readonly Mock<IPriceListTypeResolver> priceListTypeResolverMock;

        public PriceListFactoryTests()
        {
            enumDbMock = new Mock<IEnumDatabaseLayer>();
            priceListTypeResolverMock = new Mock<IPriceListTypeResolver>();
            AddRegistrationTypeEnum();
            AddCurrencies();
        }

        #region InitializePriceListForEdit() tests
        [Fact]
        public void InitializePriceListForEdit_RegistrationTypeEnums_PriceListForEdit()
        {
            // arrange
            var registrationTypeEnums = new HashSet<RegistrationTypeEnum>
            {
                RegistrationTypeEnum.NewChild,
                RegistrationTypeEnum.NewAdult,
                RegistrationTypeEnum.ReviewAdult,
                RegistrationTypeEnum.ReviewChild,
                RegistrationTypeEnum.NewAdultWeekOfClass
            };
            priceListTypeResolverMock.Setup(e => e.GetRegistrationTypesForPriceList(It.IsAny<PriceListTypeEnum>())).Returns(registrationTypeEnums);
            var factory = CreateFactory();

            // act
            var forEdit = factory.InitializePriceListForEdit(PriceListTypeEnum.Class);

            // assert
            Assert.Collection(forEdit.RegistrationTypeDescriptions,
                item => Assert.Equal(RegistrationTypeEnum.NewChild, item.Key),
                item => Assert.Equal(RegistrationTypeEnum.NewAdult, item.Key),
                item => Assert.Equal(RegistrationTypeEnum.ReviewAdult, item.Key),
                item => Assert.Equal(RegistrationTypeEnum.ReviewChild, item.Key),
                item => Assert.Equal(RegistrationTypeEnum.NewAdultWeekOfClass, item.Key));
            priceListTypeResolverMock.Verify(e => e.GetRegistrationTypesForPriceList(PriceListTypeEnum.Class), Times.Once);
            enumDbMock.Verify(e => e.GetItemsByFilter(EnumTypeEnum.MainRegistrationType, null), Times.Once);
        }

        [Fact]
        public void InitializePriceListForEdit_Currencies_PriceListForEdit()
        {
            // arrange
            priceListTypeResolverMock.Setup(e => e.GetRegistrationTypesForPriceList(It.IsAny<PriceListTypeEnum>())).Returns(new HashSet<RegistrationTypeEnum>());
            var factory = CreateFactory();

            // act
            var forEdit = factory.InitializePriceListForEdit(PriceListTypeEnum.Class);

            // assert
            Assert.Collection(forEdit.Currencies,
                item => Assert.Equal("USD", item.Name),
                item => Assert.Equal("MXN", item.Name));
            enumDbMock.Verify(e => e.GetItemsByFilter(EnumTypeEnum.Currency, null), Times.Once);
        }
        #endregion

        #region private methods
        private void AddRegistrationTypeEnum()
        {
            var enumTypesEnum = new List<IEnumItem>
            {
                new RegistrationType
                {
                    RegistrationTypeId = RegistrationTypeEnum.NewChild,
                    Name = "NewChild",
                    Description = "New Child",
                },
                new RegistrationType
                {
                    RegistrationTypeId = RegistrationTypeEnum.NewAdult,
                    Name = "NewAdult",
                    Description = "New Adult"
                },
                new RegistrationType
                {
                    RegistrationTypeId = RegistrationTypeEnum.ReviewAdult,
                    Name = "ReviewAdult",
                    Description = "Review Adult"
                },
                new RegistrationType
                {
                    RegistrationTypeId = RegistrationTypeEnum.ReviewChild,
                    Name = "ReviewChild",
                    Description = "Review Child"
                },
                new RegistrationType
                {
                    RegistrationTypeId = RegistrationTypeEnum.NewAdultWeekOfClass,
                    Name = "NewAdultWeekOfClass",
                    Description = "New Adult Week Of Class"
                }
            };

            enumDbMock.Setup(x => x.GetItemsByFilter(EnumTypeEnum.MainRegistrationType, It.IsAny<EnumItemFilter>())).Returns(enumTypesEnum);
        }

        private void AddCurrencies()
        {
            var currencies = new List<IEnumItem>
            {
                new Currency
                {
                    CurrencyId = CurrencyEnum.USD,
                    Name = "USD"                    
                },
                new Currency
                {
                    CurrencyId = CurrencyEnum.MXN,
                    Name ="MXN"
                }
            };

            enumDbMock.Setup(x => x.GetItemsByFilter(EnumTypeEnum.Currency, It.IsAny<EnumItemFilter>())).Returns(currencies);
        }

        private PriceListFactory CreateFactory()
        {
            return new PriceListFactory(enumDbMock.Object, priceListTypeResolverMock.Object);
        }
        #endregion
    }
}
