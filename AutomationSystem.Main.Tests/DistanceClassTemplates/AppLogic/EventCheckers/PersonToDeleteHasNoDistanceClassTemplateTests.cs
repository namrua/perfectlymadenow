using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.EventCheckers;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data;
using AutomationSystem.Main.Core.Persons.AppLogic.Models.Events;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.DistanceClassTemplates.AppLogic.EventCheckers
{
    public class PersonToDeleteHasNoDistanceClassTemplateTests
    {
        public Mock<IDistanceClassTemplateDatabaseLayer> templateDbMock;

        public PersonToDeleteHasNoDistanceClassTemplateTests()
        {
            templateDbMock = new Mock<IDistanceClassTemplateDatabaseLayer>();
        }

        #region CheckEvent() tests
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void CheckEvent_PersonDeletingEvent_ReturnsExpectedResult(bool eventValue, bool expectedResult)
        {
            // arrange
            templateDbMock.Setup(e => e.PersonOnAnyDistanceClassTemplate(It.IsAny<long>())).Returns(eventValue);
            var checker = CreateChecker();

            // act
            var result = checker.CheckEvent(new PersonDeletingEvent(1));

            // assert
            Assert.Equal(expectedResult, result);
            templateDbMock.Verify(e => e.PersonOnAnyDistanceClassTemplate(1), Times.Once);
        }
        #endregion

        #region private methods
        private PersonToDeleteHasNoDistanceClassTemplate CreateChecker()
        {
            return new PersonToDeleteHasNoDistanceClassTemplate(templateDbMock.Object);
        }
        #endregion
    }
}