using AutomationSystem.Main.Core.RegistrationUpload.AppLogic.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic;
using AutomationSystem.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationSystem.Main.Core.RegistrationUpload.AppLogic
{
    public class StudentRegistrationBatchMergeResolver : IBatchUploadMergeResolver<ClassRegistration>
    {
        private const string separator = "@#";

        private readonly Dictionary<string, long> idMap = new Dictionary<string, long>();

        public void BindEntities(IEnumerable<ClassRegistration> entitySet)
        {
            idMap.Clear();
            foreach (var registration in entitySet)
                idMap[GetRegistrationStudentKey(registration)] = registration.ClassRegistrationId;
        }

        public long? TryPair(BatchUploadItem item)
        {
            var key = GetBatchUploadItemKey(item);
            if (!idMap.TryGetValue(key, out var result))
            {
                return null;
            }

            return result;
        }

        #region private methods

        private string GetRegistrationStudentKey(ClassRegistration registration)
        {
            if (registration.StudentAddress == null)
            {
                throw new ArgumentException("Address is not included into ClassRegistration object.");
            }

            var keyParts = new[]
            {
                registration.StudentAddress.FirstName.ToLower(),
                registration.StudentAddress.LastName.ToLower(),
                registration.StudentAddress.Street.ToLower(),
                registration.StudentAddress.Street2?.ToLower() ?? "",
                registration.StudentAddress.City.ToLower(),
                registration.StudentAddress.State?.ToLower() ?? "",
                registration.StudentAddress.ZipCode.ToLower(),
                ((int)(registration.StudentAddress.CountryId)).ToString(),
                registration.StudentEmail.ToLower()
            };

            var result = string.Join(separator, keyParts);
            return result;
        }
        
        private string GetBatchUploadItemKey(BatchUploadItem item)
        {
            var fields = StudentRegistrationBatchMapper.GetFieldValueMap(item);

            var keyColumns = new[]
            {
                StudentRegistrationBatchColumn.FirstName,
                StudentRegistrationBatchColumn.LastName,
                StudentRegistrationBatchColumn.Street,
                StudentRegistrationBatchColumn.Street2,
                StudentRegistrationBatchColumn.City,
                StudentRegistrationBatchColumn.State,
                StudentRegistrationBatchColumn.ZipCode,
                StudentRegistrationBatchColumn.Country,
                StudentRegistrationBatchColumn.Email,
            };

            var keyParts = keyColumns.Select(x => fields[x]?.ToLower() ?? "");
            var result = string.Join(separator, keyParts);
            return result;
        }

        #endregion
    }
}
