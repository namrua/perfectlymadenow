using AutomationSystem.Shared.Core.BatchUploads.System;
using AutomationSystem.Shared.Model;
using Xunit;

namespace AutomationSystem.Shared.Tests.BatchUploads.System
{
    public class BatchUploadValidationHelperTests
    {
        #region ValidateString() tests

        [Theory]
        [InlineData(null, true, "Please enter name")]
        [InlineData(null, false, null)]
        [InlineData("", true, "Please enter name")]
        [InlineData("", false, null)]
        [InlineData("x", false, null)]
        [InlineData("x", true, null)]
        [InlineData("overMaxLength", true, "Max length of name is 5 characters")]
        public void ValidateString_ForGivenParameters_ExpectedValidationMessageIsSet(string value, bool isRequired, string expectedResult)
        {
            // arrange
            var field = new BatchUploadField
            {
                Value = value
            };
            var helper = CreateHelper();

            // act
            helper.ValidateString(field, "name", isRequired, 5);

            Assert.Equal(expectedResult, field.ValidationMessage);
        }

        #endregion

        #region ValidateEmailString() tests

        [Theory]
        [InlineData("email@email.com", true, true, null)]
        [InlineData(null, false, true, null)]
        [InlineData("value", true, false, "name is not valid email address")]
        public void ValidateEmailString_ForGivenParameters_ExpectedValidationOutcomeIsSet(string value, bool isRequired, bool expectedIsValidResult, string expectedValidationMessage)
        {
            // arrange
            var field = new BatchUploadField
            {
                Value = value
            };
            var helper = CreateHelper();

            // act
            helper.ValidateEmailString(field, "name", isRequired, 16);

            // assert
            Assert.Equal(expectedIsValidResult, field.IsValid);
            Assert.Equal(expectedValidationMessage, field.ValidationMessage);
        }

        #endregion

        #region ValidateDropDown() tests

        [Theory]
        [InlineData(true, null, "Please select name", false)]
        [InlineData(true, "value", null, true)]
        [InlineData(false, null, null, true)]
        [InlineData(false, "value", null, true)]
        public void ValidateDropDown_ForGivenParameters_ExpectedValidationOutcomeIsSet(bool notSelected, string value, string expectedValidationMessage, bool expectedIsValidResult)
        {
            // arrange
            var field = new BatchUploadField
            {
                Value = value
            };
            var helper = CreateHelper();

            // act
            helper.ValidateDropDown(field, "name", notSelected);

            // assert
            Assert.Equal(expectedValidationMessage, field.ValidationMessage);
            Assert.Equal(expectedIsValidResult, field.IsValid);
        }

        #endregion

        #region private methods

        private BatchUploadValidationHelper CreateHelper()
        {
            return new BatchUploadValidationHelper();
        }

        #endregion
    }
}
