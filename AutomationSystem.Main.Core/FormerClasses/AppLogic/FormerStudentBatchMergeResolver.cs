using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudentsBatchUploads;
using AutomationSystem.Main.Core.FormerClasses.AppLogic.Convertors;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic
{      

    /// <summary>
    /// Resolves merging of uploaded items on specified entity set
    /// </summary>
    public class FormerStudentBatchMergeResolver : IBatchUploadMergeResolver<FormerStudent>
    {

        private const string separator = "@#";

        private readonly Dictionary<string, long> idMap = new Dictionary<string, long>();


        // binds entity set
        public void BindEntities(IEnumerable<FormerStudent> entitySet)
        {
            idMap.Clear();
            foreach (var formerStudent in entitySet)
                idMap[GetFormerStudentKey(formerStudent)] = formerStudent.FormerStudentId;
        }
      
        // tries to pair batch upload item to existing entity from set
        public long? TryPair(BatchUploadItem item)
        {
            var key = GetBatchUploadItemKey(item);
            if (!idMap.TryGetValue(key, out var result))
                return null;
            return result;
        }

      
        #region private methods

        // gets former student key
        private string GetFormerStudentKey(FormerStudent formerStudent)
        {
            if (formerStudent.Address == null)
                throw new ArgumentException("Address is not included into FormerStudent object.");

            var keyParts = new[]
            {
                formerStudent.Address.FirstName.ToLower(),
                formerStudent.Address.LastName.ToLower(),
                formerStudent.Address.Street.ToLower(),
                formerStudent.Address.Street2?.ToLower() ?? "",
                formerStudent.Address.City.ToLower(),
                formerStudent.Address.State?.ToLower() ?? "",
                formerStudent.Address.ZipCode.ToLower(),
                ((int)(formerStudent.Address.CountryId)).ToString(),
                formerStudent.Email.ToLower()
            };

            var result = string.Join(separator, keyParts);
            return result;
        }

        // gets batch upload item key
        private string GetBatchUploadItemKey(BatchUploadItem item)
        {
            var fields = FormerStudentBatchConvertor.GetFieldValueMap(item);

            var keyColumns = new[]
            {
                FormerStudentBatchColumn.FirstName,
                FormerStudentBatchColumn.LastName,
                FormerStudentBatchColumn.Street,
                FormerStudentBatchColumn.Street2,
                FormerStudentBatchColumn.City,
                FormerStudentBatchColumn.State,
                FormerStudentBatchColumn.ZipCode,
                FormerStudentBatchColumn.Country,
                FormerStudentBatchColumn.Email,
            };

            var keyParts = keyColumns.Select(x => fields[x]?.ToLower() ?? "");
            var result = string.Join(separator, keyParts);
            return result;
        }

        #endregion


    }

}
