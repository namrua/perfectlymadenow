using AutomationSystem.Main.Core.RegistrationUpload.AppLogic;
using AutomationSystem.Shared.Contract.BatchUploads.System;
using AutomationSystem.Shared.Model;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace AutomationSystem.Main.Tests.RegistrationUploads.AppLogic
{
    public class StudentRegistrationBatchValidatorTests
    {
        private readonly Mock<IBatchUploadValidationHelper> validationHelperMock;

        public StudentRegistrationBatchValidatorTests()
        {
            validationHelperMock = new Mock<IBatchUploadValidationHelper>();
        }

        #region Validate() tests

        [Fact]
        public void Validate_BatchUploadFieldsNotIncludedIntoObject_ThrowsInvalidOperationException()
        {
            // arrange
            var batchUploadItem = new BatchUploadItem
            {
                BatchUploadFields = null
            };
            var validator = CreateValidator();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => validator.Validate(batchUploadItem));
        }

        [Fact]
        public void Validate_BatchUploadItem_ReturnsTrue()
        {
            // arrange
            var item = CreateItem();
            var validator = CreateValidator();

            // act
            var result = validator.Validate(item);

            // assert
            Assert.True(result);
            validationHelperMock.Verify(e => e.ValidateString(It.IsAny<BatchUploadField>(), "First name", true, 64), Times.Once);
            validationHelperMock.Verify(e => e.ValidateString(It.IsAny<BatchUploadField>(), "Last name", true, 64), Times.Once);
            validationHelperMock.Verify(e => e.ValidateString(It.IsAny<BatchUploadField>(), "Address line 1", true, 64), Times.Once);
            validationHelperMock.Verify(e => e.ValidateString(It.IsAny<BatchUploadField>(), "Address line 2", false, 64), Times.Once);
            validationHelperMock.Verify(e => e.ValidateString(It.IsAny<BatchUploadField>(), "City", true, 64), Times.Once);
            validationHelperMock.Verify(e => e.ValidateString(It.IsAny<BatchUploadField>(), "State", false, 64), Times.Once);
            validationHelperMock.Verify(e => e.ValidateString(It.IsAny<BatchUploadField>(), "Zip code", true, 16), Times.Once);
            validationHelperMock.Verify(e => e.ValidateDropDown(It.IsAny<BatchUploadField>(), "Country", true), Times.Once);
            validationHelperMock.Verify(e => e.ValidateString(It.IsAny<BatchUploadField>(), "Phone", false, 15), Times.Once);
            validationHelperMock.Verify(e => e.ValidateEmailString(It.IsAny<BatchUploadField>(), "Email", true, 128), Times.Once);
        }

        #endregion

        #region private methods

        private StudentRegistrationBatchValidator CreateValidator()
        {
            return new StudentRegistrationBatchValidator(validationHelperMock.Object);
        }

        private BatchUploadField CreateField(int order)
        {
            return new BatchUploadField
            {
                Order = order,
                IsValid = true
            };
        }
        private BatchUploadItem CreateItem()
        {
            return new BatchUploadItem
            {
                BatchUploadFields = new List<BatchUploadField>
                {
                   CreateField(0), CreateField(1), CreateField(2),CreateField(3), CreateField(4),
                   CreateField(5), CreateField(6), CreateField(7), CreateField(8), CreateField(9)
                }
            };
        }

        #endregion
    }
}
