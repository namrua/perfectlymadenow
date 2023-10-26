using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Enums.System;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Enums.Data.Models;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace AutomationSystem.Main.Tests.Enums.System
{
    public class EnumMappingHelperTests
    {
        private readonly Mock<IEnumDatabaseLayer> enumDbMock;

        public EnumMappingHelperTests()
        {
            enumDbMock = new Mock<IEnumDatabaseLayer>();
            enumDbMock.Setup(e => e.GetItemsByFilter(It.IsAny<EnumTypeEnum>(), It.IsAny<EnumItemFilter>())).Returns(new List<IEnumItem>());
        }

        #region TypeMapCountry() tests

        [Fact]
        public void TypeMapCountry_Country_ReturnsCountryEnum()
        {
            // arrange
            var helper = CreateHelper();

            // act
            var result = helper.TypeMapCountry("United States");

            // assert
            Assert.Equal(CountryEnum.US, result);
            enumDbMock.Verify(e => e.GetItemsByFilter(EnumTypeEnum.Country, null), Times.Once);
        }

        [Fact]
        public void TypeMapCountry_UnknownCountry_ReturnsNull()
        {
            // arrange
            var helper = CreateHelper();

            // act
            var result = helper.TypeMapCountry("The first galactic empire");

            // assert
            Assert.Null(result);
        }

        #endregion

        #region private methods

        private EnumMappingHelper CreateHelper()
        {
            return new EnumMappingHelper(enumDbMock.Object);
        }
        

        #endregion
    }
}
