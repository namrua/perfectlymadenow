using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudentsBatchUploads;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic;
using AutomationSystem.Shared.Contract.BatchUploads.System;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.Helpers.Routines;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic
{

    /// <summary>
    /// Validates former student batch uploads
    /// </summary>
    public class FormerStudentBatchValidator : IFormerStudentUploadValidator
    {
        private readonly IBatchUploadValidationHelper batchValidationHelper;        

        // constructor
        public FormerStudentBatchValidator(IBatchUploadValidationHelper batchValidationHelper)
        {          
            this.batchValidationHelper = batchValidationHelper;
        }


        // validates batch files and writes results in it
        public bool Validate(BatchUploadItem uploadItem)
        {
            EntityHelper.CheckForNull(uploadItem.BatchUploadFields, "BatchUploadFields", "BatchUploadItem");

            var isValid = true;
            foreach (var uploadField in uploadItem.BatchUploadFields)
            {               
                switch (uploadField.Order)
                {
                    case FormerStudentBatchColumn.FirstName:
                        batchValidationHelper.ValidateString(uploadField, "First name", true, 64);
                        break;
                    case FormerStudentBatchColumn.LastName:
                        batchValidationHelper.ValidateString(uploadField, "Last name", true, 64);
                        break;
                    case FormerStudentBatchColumn.Street:
                        batchValidationHelper.ValidateString(uploadField, "Address line 1", true, 64);
                        break;
                    case FormerStudentBatchColumn.Street2:
                        batchValidationHelper.ValidateString(uploadField, "Address line 2", false, 64);
                        break;
                    case FormerStudentBatchColumn.City:
                        batchValidationHelper.ValidateString(uploadField, "City", true, 64);
                        break;
                    case FormerStudentBatchColumn.State:
                        batchValidationHelper.ValidateString(uploadField, "State", false, 64);
                        break;
                    case FormerStudentBatchColumn.ZipCode:
                        batchValidationHelper.ValidateString(uploadField, "Zip code", true, 16);
                        break;
                    case FormerStudentBatchColumn.Country:
                        batchValidationHelper.ValidateDropDown(uploadField, "Country", true);
                        break;
                    case FormerStudentBatchColumn.Phone:
                        batchValidationHelper.ValidateString(uploadField, "Phone", false, 15);
                        break;
                    case FormerStudentBatchColumn.Email:
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
