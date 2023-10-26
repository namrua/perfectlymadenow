using System.Collections.Generic;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.AppLogic.Factories;
using AutomationSystem.Main.Core.Persons.Data;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Classes.AppLogic.Factories
{
    public class ClassReportSettingFactoryTests
    {
        private readonly Mock<IPersonDatabaseLayer> personDbMock;

        public ClassReportSettingFactoryTests()
        {
            personDbMock = new Mock<IPersonDatabaseLayer>();
            AddPersonsMinimized();
        }

        #region CreateClassReportSettingForEdit() tests
        [Fact]
        public void CreateClassReportSettingForEdit_ProfileId_ReturnsClassReportSettingForEdit()
        {
            // arrange
            var profileId = 1;
            var factory = CreateClassReportSettingFactory();

            // act
            var edit = factory.CreateClassReportSettingForEdit(profileId);

            // assert
            Assert.Equal("firstName lastName", edit.PersonHelper.GetPersonNameById(1));
            Assert.Equal("secondFirstName secondLastName", edit.PersonHelper.GetPersonNameById(2));
            personDbMock.Verify(x => x.GetMinimizedPersonsByProfileId(profileId), Times.Once);
        }
        #endregion

        #region private methods
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

        private ClassReportSettingFactory CreateClassReportSettingFactory()
        {
            return new ClassReportSettingFactory(personDbMock.Object);
        }
        #endregion
    }
}
