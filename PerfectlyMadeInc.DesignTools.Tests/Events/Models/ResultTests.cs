using System;
using PerfectlyMadeInc.DesignTools.Contract.Events.Models;
using Xunit;

namespace PerfectlyMadeInc.DesignTools.Tests.Events.Models
{
    public class ResultTests
    {
        #region Success() tests

        [Fact]
        public void Success_ReturnsSuccessResult()
        {
            // arrange
            var message = "Success message";

            // act
            var result = Result.Success(message);

            // assert
            Assert.Equal(ResultType.Success, result.ResultType);
            Assert.Equal(message, result.Message);
        }

        #endregion

        #region Skipped() tests

        [Fact]
        public void Skipped_ReturnsSkippedResult()
        {
            // arrange
            var message = "Skipped message";

            // act
            var result = Result.Skipped(message);

            // assert
            Assert.Equal(ResultType.Skipped, result.ResultType);
            Assert.Equal(message, result.Message);
        }

        #endregion

        #region Error() tests

        [Fact]
        public void Skipped_ReturnsErrorResult()
        {
            // arrange
            var message = "Error message";
            var exception = new InvalidOperationException();

            // act
            var result = Result.Error(exception, message);

            // assert
            Assert.Equal(ResultType.Error, result.ResultType);
            Assert.Equal(message, result.Message);
            Assert.Same(exception, result.Exception);
        }

        #endregion
    }
}
