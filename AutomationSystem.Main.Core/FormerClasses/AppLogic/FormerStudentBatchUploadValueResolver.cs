using System;
using System.Linq;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudentsBatchUploads;
using AutomationSystem.Main.Core.Enums.System;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic
{
    public class FormerStudentBatchUploadValueResolver : IFormerStudentBatchUploadValueResolver
    {
        private readonly IEnumMappingHelper enumMappingHelper;

        public FormerStudentBatchUploadValueResolver(IEnumMappingHelper enumMappingHelper)
        {
            this.enumMappingHelper = enumMappingHelper;
        }

        public string[] GetValues(string[] origValues)
        {
            if (origValues.Length != FormerStudentBatchColumn.RowLength)
            {
                throw new ArgumentException($"Data line should have length of {FormerStudentBatchColumn.RowLength} characters.");
            }

            var result = origValues.ToArray();
            var countryId = enumMappingHelper.TypeMapCountry(origValues[FormerStudentBatchColumn.Country]);
            result[FormerStudentBatchColumn.Country] = countryId.HasValue ? ((int)countryId).ToString() : null;
            return result;
        }
    }
}
