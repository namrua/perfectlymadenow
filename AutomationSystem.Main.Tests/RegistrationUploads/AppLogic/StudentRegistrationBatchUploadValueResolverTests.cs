using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Enums.System;
using AutomationSystem.Main.Core.RegistrationUpload.AppLogic;
using AutomationSystem.Main.Core.RegistrationUpload.AppLogic.Models;
using Moq;
using System;
using Xunit;

namespace AutomationSystem.Main.Tests.RegistrationUploads.AppLogic
{
    public class StudentRegistrationBatchUploadValueResolverTests
    {
        private readonly Mock<IEnumMappingHelper> enumMappingHelperMock;

        public StudentRegistrationBatchUploadValueResolverTests()
        {
            enumMappingHelperMock = new Mock<IEnumMappingHelper>();
        }

        #region GetValues() tests

        [Fact]
        public void GetValues_InvalidData_ThrowsArgumentException()
        {
            // arrange
            var resolver = CreateResolver();

            // act & assert
            Assert.Throws<ArgumentException>(() => resolver.GetValues(new[] {""}));
        }

        [Fact]
        public void GetValues_OriginalValuesWithSetCountry_CountryIsMappedAndReturnsValuesWithSetCountry()
        {
            // arrange
            var origValues = CreateData("US");
            enumMappingHelperMock.Setup(e => e.TypeMapCountry(It.IsAny<string>())).Returns(CountryEnum.US);
            var resolver = CreateResolver();

            // act
            var result = resolver.GetValues(origValues);

            // assert
            Assert.Equal(((int)CountryEnum.US).ToString(), result[StudentRegistrationBatchColumn.Country]);
            enumMappingHelperMock.Verify(e => e.TypeMapCountry("US"), Times.Once);
        }

        #endregion

        #region private methods

        private StudentRegistrationBatchUploadValueResolver CreateResolver()
        {
            return new StudentRegistrationBatchUploadValueResolver(enumMappingHelperMock.Object);
        }

        private string[] CreateData(string country)
        {
            return new[]
            {
                "0", "1", "2", "3", "4", "5", "6", country, "8", "9"
            };
        }
        #endregion
    }
}
