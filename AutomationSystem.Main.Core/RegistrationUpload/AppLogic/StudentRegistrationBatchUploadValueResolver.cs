using System;
using System.Linq;
using AutomationSystem.Main.Core.Enums.System;
using AutomationSystem.Main.Core.RegistrationUpload.AppLogic.Models;

namespace AutomationSystem.Main.Core.RegistrationUpload.AppLogic
{
    public class StudentRegistrationBatchUploadValueResolver : IStudentRegistrationBatchUploadValueResolver
    {
        private readonly IEnumMappingHelper enumMappingHelper;

        public StudentRegistrationBatchUploadValueResolver(IEnumMappingHelper enumMappingHelper)
        {
            this.enumMappingHelper = enumMappingHelper;
        }

        public string[] GetValues(string[] origValues)
        {
            if (origValues.Length != StudentRegistrationBatchColumn.RowLength)
            {
                throw new ArgumentException($"Data line should have length of {StudentRegistrationBatchColumn.RowLength} characters.");
            }

            var result = origValues.ToArray();
            var countryId = enumMappingHelper.TypeMapCountry(origValues[StudentRegistrationBatchColumn.Country]);
            result[StudentRegistrationBatchColumn.Country] = countryId.HasValue ? ((int) countryId).ToString() : null;
            return result;
        }
    }
}
