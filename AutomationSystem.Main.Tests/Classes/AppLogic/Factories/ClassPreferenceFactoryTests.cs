using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.AppLogic.Factories;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Classes.AppLogic.Factories
{
    public class ClassPreferenceFactoryTests
    {
        private readonly Mock<IClassDatabaseLayer> classDbMock;
        private readonly Mock<IPersonDatabaseLayer> personDbMock;
        private readonly Mock<IEnumDatabaseLayer> enumDbMock;

        public ClassPreferenceFactoryTests()
        {
            classDbMock = new Mock<IClassDatabaseLayer>();
            personDbMock = new Mock<IPersonDatabaseLayer>();
            enumDbMock = new Mock<IEnumDatabaseLayer>();
            AddRegistrationColorSchemeTypes();
            AddPersonsMinimized();
            AddCurrencies();
        }

        #region CreateDefaultClassPreference() tests
        [Fact]
        public void CreateDefaultClassPreference_ReturnsClassPreference()
        {
            // arrange
            var factory = CreateClassPreferenceFactory();

            // act
            var classPreference = factory.CreateDefaultClassPreference();

            // assert
            Assert.Equal(RegistrationColorSchemeEnum.Limet, classPreference.RegistrationColorSchemeId);
            Assert.Equal("UNKNOWN", classPreference.LocationCode);
            Assert.True(classPreference.SendCertificatesByEmail);
            Assert.Equal(LocalisationInfo.DefaultCurrency, classPreference.CurrencyId);

        }
        #endregion

        #region CreateClassPreferenceForEdit() tests
        [Fact]
        public void CreateClassPreferenceForEdit_RegistrationColorSchemeIsSet_ReturnsClassPreferenceForEditWithRegistrationColorScheme()
        {
            // arrange
            var factory = CreateClassPreferenceFactory();

            // act
            var classPreferenceForEdit = factory.CreateClassPreferenceForEdit(1);

            // assert
            Assert.Collection(classPreferenceForEdit.ColorSchemes,
                item =>
                {
                    Assert.Equal(RegistrationColorSchemeEnum.Limet, item.RegistrationColorSchemeId);
                    Assert.Equal("Limet", item.Name);
                    Assert.Equal("Limet-green scheme", item.Description);
                },
                item =>
                {
                    Assert.Equal(RegistrationColorSchemeEnum.Ocean, item.RegistrationColorSchemeId);
                    Assert.Equal("Ocean", item.Name);
                    Assert.Equal("Ocean-blue scheme", item.Description);
                }
            );
        }

        [Fact]
        public void CreateClassPreferenceForEdit_PersonHelperIsSet_ReturnsClassPreferenceForEditWithPersonHelper()
        {
            // arrange
            var profileId = 13;
            var factory = CreateClassPreferenceFactory();

            // act
            var classPreferenceForEdit = factory.CreateClassPreferenceForEdit(profileId);

            // assert
            Assert.Equal("firstName lastName", classPreferenceForEdit.PersonHelper.GetPersonNameById(1));
            Assert.Equal("secondFirstName secondLastName", classPreferenceForEdit.PersonHelper.GetPersonNameById(2));
            personDbMock.Verify(x => x.GetMinimizedPersonsByProfileId(profileId), Times.Once);
        }

        [Fact]
        public void CreateClassPreferenceForEdit_CurrencyIsSet_ReturnsClassPreferenceForEditWithCurrency()
        {
            // arrange
            var factory = CreateClassPreferenceFactory();

            // act
            var classPreferenceForEdit = factory.CreateClassPreferenceForEdit(1);

            // assert
            Assert.Collection(classPreferenceForEdit.Currencies,
                item => Assert.Equal("USD", item.Name),
                item => Assert.Equal("MXN", item.Name));
            enumDbMock.Verify(e => e.GetItemsByFilter(EnumTypeEnum.Currency, null), Times.Once);
        }
        #endregion

        #region private methods
        private void AddRegistrationColorSchemeTypes()
        {
            var colorSchemes = new List<RegistrationColorScheme>()
            {
                new RegistrationColorScheme
                {
                    RegistrationColorSchemeId = RegistrationColorSchemeEnum.Limet,
                    Description = "Limet-green scheme",
                    Name = "Limet",
                },
                new RegistrationColorScheme
                {
                    RegistrationColorSchemeId = RegistrationColorSchemeEnum.Ocean,
                    Description = "Ocean-blue scheme",
                    Name = "Ocean",
                }
            };

            classDbMock.Setup(x => x.GetRegistrationColorSchemes()).Returns(colorSchemes);
        }

        private void AddPersonsMinimized()
        {
            var personsMinimized = new List<PersonMinimized>()
            {
                new PersonMinimized
                {
                    PersonId = 1,
                    Name = "firstName lastName"
                },
                new PersonMinimized
                {
                    PersonId = 2,
                    Name = "secondFirstName secondLastName"
                }
            };

            personDbMock.Setup(x => x.GetMinimizedPersonsByProfileId(It.IsAny<long>())).Returns(personsMinimized);
        }

        private void AddCurrencies()
        {
            var currencies = new List<IEnumItem>()
            {
                new Currency
                {
                    Name = "USD"
                },
                new Currency
                {
                    Name = "MXN"
                }
            };
            enumDbMock.Setup(x => x.GetItemsByFilter(It.IsAny<EnumTypeEnum>(), null)).Returns(currencies);
        }

        private ClassPreferenceFactory CreateClassPreferenceFactory()
        {
            return new ClassPreferenceFactory(classDbMock.Object, personDbMock.Object, enumDbMock.Object);
        }
        #endregion
    }
}
