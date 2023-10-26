using AutoMapper;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.MainEnum.AppLogic.MappingProfiles;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Enums.Data.Models;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace AutomationSystem.Main.Tests.MainEnum.AppLogic.MappingProfiles
{
    public class MainEnumProfileTests
    {
        private readonly Mock<IEnumDatabaseLayer> enumDbMock;

        public MainEnumProfileTests()
        {
            enumDbMock = new Mock<IEnumDatabaseLayer>();
            AddLanguageEnums();
        }

        #region CreateMap<LanguageEnum, string>() tests
        [Theory]
        [InlineData(LanguageEnum.En, "En")]
        [InlineData(LanguageEnum.Cs, "Cs")]
        [InlineData(LanguageEnum.Es, "Es")]
        [InlineData(LanguageEnum.Fr, "Fr")]
        [InlineData(LanguageEnum.Ro, "Ro")]
        public void Map_LanguageEnum_ReturnsExpectedString(LanguageEnum languageEnum, string expectedValue)
        {
            // arrange
            var mapper = CreateMapper();

            // act
            var actualValue = mapper.Map<string>(languageEnum);

            // assert
            Assert.Equal(expectedValue, actualValue);
        }
        #endregion

        #region private methods
        private Mapper CreateMapper()
        {
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MainEnumProfile(enumDbMock.Object));
            });
            return new Mapper(mapperCfg);
        }

        private void AddLanguageEnums()
        {
            var languageEnums = new List<IEnumItem>
            {
                new EnumItem
                {
                    Description = "En",
                    Id = 1,
                    Name = "English"
                },
                new EnumItem
                {
                    Description = "Cs",
                    Id = 2,
                    Name = "Czech"
                },
                new EnumItem
                {
                    Description = "Es",
                    Id = 3,
                    Name = "Spanish"
                },
                new EnumItem
                {
                    Description = "Fr",
                    Id = 4,
                    Name = "French"
                },
                new EnumItem
                {
                    Description = "Ro",
                    Id = 5,
                    Name = "Romanian"
                }
            };
            enumDbMock.Setup(x => x.GetItemsByFilter(It.IsAny<EnumTypeEnum>(), It.IsAny<EnumItemFilter>())).Returns(languageEnums);
        }
        #endregion
    }
}
