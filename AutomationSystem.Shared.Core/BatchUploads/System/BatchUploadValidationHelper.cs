using System.ComponentModel.DataAnnotations;
using AutomationSystem.Shared.Contract.BatchUploads.System;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.BatchUploads.System
{
    public class BatchUploadValidationHelper : IBatchUploadValidationHelper
    {
        private readonly EmailAddressAttribute emailAttr;

        public BatchUploadValidationHelper()
        {
            emailAttr = new EmailAddressAttribute();
        }

        public void ValidateString(BatchUploadField field, string name, bool isRequired, int maxLength)
        {
            field.IsValid = false;
            field.ValidationMessage = null;
            var value = field.Value;
            if (string.IsNullOrEmpty(value) && isRequired)
            {
                field.ValidationMessage = $"Please enter {name}";
                return;
            }

            if (value != null && value.Length > maxLength)
            {
                field.ValidationMessage = $"Max length of {name} is {maxLength} characters";
                return;
            }
            field.IsValid = true;
        }

        public void ValidateEmailString(BatchUploadField field, string name, bool isRequired, int maxLength)
        {
            ValidateString(field, name, isRequired, maxLength);
            if (!field.IsValid || field.Value == null)
                return;
            if (emailAttr.IsValid(field.Value)) return;

            field.IsValid = false;
            field.ValidationMessage = $"{name} is not valid email address";
        }

        public void ValidateDropDown(BatchUploadField field, string name, bool notSelected)
        {
            field.IsValid = true;
            field.ValidationMessage = null;
            
            if (notSelected && field.Value == null)
            {
                field.ValidationMessage = $"Please select {name}";
                field.IsValid = false;
            }
        }
    }
}
