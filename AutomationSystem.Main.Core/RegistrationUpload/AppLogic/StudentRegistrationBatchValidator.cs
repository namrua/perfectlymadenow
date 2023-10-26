using AutomationSystem.Main.Core.RegistrationUpload.AppLogic.Models;
using AutomationSystem.Shared.Contract.BatchUploads.System;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.Helpers.Routines;

namespace AutomationSystem.Main.Core.RegistrationUpload.AppLogic
{
    public class StudentRegistrationBatchValidator : IStudentRegistrationBatchValidator
    {
        private readonly IBatchUploadValidationHelper batchValidationHelper;
        public StudentRegistrationBatchValidator(IBatchUploadValidationHelper batchValidationHelper)
        {
            this.batchValidationHelper = batchValidationHelper;
        }

        public bool Validate(BatchUploadItem uploadItem)
        {
            EntityHelper.CheckForNull(uploadItem.BatchUploadFields, "BatchUploadFields", "BatchUploadItem");

            var isValid = true;
            foreach (var uploadField in uploadItem.BatchUploadFields)
            {
                switch (uploadField.Order)
                {
                    case StudentRegistrationBatchColumn.FirstName:
                        batchValidationHelper.ValidateString(uploadField, "First name", true, 64);
                        break;
                    case StudentRegistrationBatchColumn.LastName:
                        batchValidationHelper.ValidateString(uploadField, "Last name", true, 64);
                        break;
                    case StudentRegistrationBatchColumn.Street:
                        batchValidationHelper.ValidateString(uploadField, "Address line 1", true, 64);
                        break;
                    case StudentRegistrationBatchColumn.Street2:
                        batchValidationHelper.ValidateString(uploadField, "Address line 2", false, 64);
                        break;
                    case StudentRegistrationBatchColumn.City:
                        batchValidationHelper.ValidateString(uploadField, "City", true, 64);
                        break;
                    case StudentRegistrationBatchColumn.State:
                        batchValidationHelper.ValidateString(uploadField, "State", false, 64);
                        break;
                    case StudentRegistrationBatchColumn.ZipCode:
                        batchValidationHelper.ValidateString(uploadField, "Zip code", true, 16);
                        break;
                    case StudentRegistrationBatchColumn.Country:
                        batchValidationHelper.ValidateDropDown(uploadField, "Country", true);
                        break;
                    case StudentRegistrationBatchColumn.Phone:
                        batchValidationHelper.ValidateString(uploadField, "Phone", false, 15);
                        break;
                    case StudentRegistrationBatchColumn.Email:
                        batchValidationHelper.ValidateEmailString(uploadField, "Email", true, 128);
                        break;
                }
                isValid = isValid && uploadField.IsValid;
            }
            uploadItem.IsValid = isValid;
            return isValid;
        }
    }
}
