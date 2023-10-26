using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.Models;
using System;
using Xunit;

namespace AutomationSystem.Main.Tests.DistanceClassTemplates.AppLogic
{
    public class DistanceClassTemplateOperationCheckerTests
    {
        #region CheckOperation() tests
        [Fact]
        public void CheckOperation_OperationIsNotAllowed_ThrowsInvalidOperationException()
        {
            // arrange
            var checker = CreateChecker();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => checker.CheckOperation(DistanceClassTemplateOperation.Approve, DistanceClassTemplateState.Approved));
        }

        [Fact]
        public void CheckOperation_OperationIsAllowed_NoExceptionIsThrown()
        {
            // arrange
            var checker = CreateChecker();

            // act & assert
            checker.CheckOperation(DistanceClassTemplateOperation.Approve, DistanceClassTemplateState.New);
        }
        #endregion

        #region IsOperationAllowed() tests
        [Fact]
        public void IsOperationAllowed_DistanceClassTemplateOperationIsInvalid_ThrowsArgumentException()
        {
            // arrange
            var checker = CreateChecker();

            // act & assert
            Assert.Throws<ArgumentException>(() => checker.IsOperationAllowed((DistanceClassTemplateOperation)100, DistanceClassTemplateState.Approved));
        }

        [Theory]
        [InlineData(DistanceClassTemplateOperation.Approve, DistanceClassTemplateState.New, true)]
        [InlineData(DistanceClassTemplateOperation.Approve, DistanceClassTemplateState.Approved, false)]
        [InlineData(DistanceClassTemplateOperation.Approve, DistanceClassTemplateState.Completed, false)]
        [InlineData(DistanceClassTemplateOperation.Delete, DistanceClassTemplateState.New, true)]
        [InlineData(DistanceClassTemplateOperation.Delete, DistanceClassTemplateState.Approved, false)]
        [InlineData(DistanceClassTemplateOperation.Delete, DistanceClassTemplateState.Completed, false)]
        [InlineData(DistanceClassTemplateOperation.Edit, DistanceClassTemplateState.New, true)]
        [InlineData(DistanceClassTemplateOperation.Edit, DistanceClassTemplateState.Approved, true)]
        [InlineData(DistanceClassTemplateOperation.Edit, DistanceClassTemplateState.Completed, false)]
        [InlineData(DistanceClassTemplateOperation.Complete, DistanceClassTemplateState.New, false)]
        [InlineData(DistanceClassTemplateOperation.Complete, DistanceClassTemplateState.Approved, true)]
        [InlineData(DistanceClassTemplateOperation.Complete, DistanceClassTemplateState.Completed, false)]
        public void IsOperationAllowed_DistanceClassOperationsAndStates_ReturnsExpectedResult(DistanceClassTemplateOperation operation, DistanceClassTemplateState state, bool expectedResult)
        {
            // arrange
            var checker = CreateChecker();

            // act
            var actualResult = checker.IsOperationAllowed(operation, state);

            // assert
            Assert.Equal(expectedResult, actualResult);
        }
        #endregion

        #region private methods
        private DistanceClassTemplateOperationChecker CreateChecker()
        {
            return new DistanceClassTemplateOperationChecker();
        }

        #endregion
    }
}
