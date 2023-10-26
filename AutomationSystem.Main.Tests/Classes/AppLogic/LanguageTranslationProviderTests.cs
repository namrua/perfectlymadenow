using System.Collections.Generic;
using Xunit;
using Moq;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Localisation.System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Classes.AppLogic;

namespace AutomationSystem.Main.Tests.Classes.AppLogic
{
    public class LanguageTranslationProviderTests
    {
        private readonly Mock<IEnumDatabaseLayer> enumDbMock;
        private readonly Mock<ILocalisationService> localisationServiceMock;
        
        public LanguageTranslationProviderTests()
        {
            enumDbMock = new Mock<IEnumDatabaseLayer>();
            localisationServiceMock = new Mock<ILocalisationService>();
        }

        #region GetTranslationCode() tests
        [Fact]
        public void GetTranslationCode_TransLanguageIsNull_ReturnsTranslationCode()
        {
            // arrange
            var originalLanguageId = LanguageEnum.En;
            LanguageEnum? transLanguageId = null;
            var provider = CreateProvider();

            // act
            var result = provider.GetTranslationCode(originalLanguageId, transLanguageId);

            // assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void GetTranslationCode_ForGivenLanguageIds_ReturnsTranslationCode()
        {
            // arrange
            var originalLanguageId = LanguageEnum.En;
            var translationLanguageId = LanguageEnum.Cs;
            var provider = CreateProvider();

            // act
            var result = provider.GetTranslationCode(originalLanguageId, translationLanguageId);

            // assert
            Assert.Equal(2001, result);
        }
        #endregion

        #region GetOriginalLanguageId() tests
        [Fact]
        public void GetOriginalLanguageId_TransaltionCode_ReturnsOriginalLanguageId()
        {
            // arrange
            var code = 2001;
            var provider = CreateProvider();

            // act
            var result = provider.GetOriginalLanguageId(code);

            // assert
            Assert.Equal(LanguageEnum.En, result);
        }
        #endregion

        #region GetTranslationLanguageId() tests
        [Fact]
        public void GetTranslationLanguageId_NoTransaltionLanguage_ReturnsNull()
        {
            // arrange
            var code = 1;
            var provider = CreateProvider();

            // act
            var result = provider.GetTranslationLanguageId(code);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public void GetTranslationLanguageId_TranslationCode_ReturnsTranslationLanguageId()
        {
            // arrange
            var code = 2001;
            var provider = CreateProvider();

            // act
            var result = provider.GetTranslationLanguageId(code);

            // assert
            Assert.Equal(LanguageEnum.Cs, result);
        }
        #endregion

        #region GetTranslationOptions() tests
        [Fact]
        public void GetTranslationOptions_ReturnsDropDownItems()
        {
            // arrange
            var defLang = new EnumItem
            {
                Description = "English"
            };
            var languages = new List<IEnumItem>
            {
                new EnumItem
                {
                    Description = "Czech"
                },
                new EnumItem
                {
                    Description = "French"
                }
            };
            enumDbMock.Setup(e => e.GetItemById(It.IsAny<EnumTypeEnum>(), It.IsAny<int>())).Returns(defLang);
            localisationServiceMock.Setup(e => e.GetSupportedLanguages(It.IsAny<bool>())).Returns(languages);
            var provider = CreateProvider();

            // act
            var result = provider.GetTranslationOptions();

            // assert
            Assert.Collection(result,
                item => Assert.Equal("English only", item.Text),
                item => Assert.Equal("English to Czech", item.Text),
                item => Assert.Equal("English to French", item.Text),
                item => Assert.Equal("Czech to English", item.Text),
                item => Assert.Equal("French to English", item.Text));
                
            enumDbMock.Verify(e => e.GetItemById(EnumTypeEnum.Language, (int)LanguageEnum.En), Times.Once);
            localisationServiceMock.Verify(e => e.GetSupportedLanguages(true), Times.Once);
        }
        #endregion

        #region private methods
        private LanguageTranslationProvider CreateProvider()
        {
            return new LanguageTranslationProvider(
                enumDbMock.Object,
                localisationServiceMock.Object);
        }
        #endregion
    }
}
