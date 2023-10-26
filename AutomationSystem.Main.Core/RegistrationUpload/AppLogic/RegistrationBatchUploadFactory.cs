using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.RegistrationUpload.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.RegistrationUpload.AppLogic.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.BatchUploads.Data;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Main.Core.Enums.System;

namespace AutomationSystem.Main.Core.RegistrationUpload.AppLogic
{
    public class RegistrationBatchUploadFactory : IRegistrationBatchUploadFactory
    {
        private readonly IBatchUploadDatabaseLayer batchUploadDb;
        private readonly IClassDatabaseLayer classDb;
        private readonly IEnumDatabaseLayer enumDb;
        private readonly IRegistrationLogicProvider registrationLogicProvider;

        public RegistrationBatchUploadFactory(
            IBatchUploadDatabaseLayer batchUploadDb,
            IClassDatabaseLayer classDb,
            IEnumDatabaseLayer enumDb,
            IRegistrationLogicProvider registrationLogicProvider)
        {
            this.batchUploadDb = batchUploadDb;
            this.classDb = classDb;
            this.enumDb = enumDb;
            this.registrationLogicProvider = registrationLogicProvider;
        }

        public RegistrationStudentBatchItemForEdit CreateStudentBatchItemForEdit(BatchUploadItem batchUploadItem)
        {
            var result = new RegistrationStudentBatchItemForEdit
            {
                BatchUploadItemId = batchUploadItem.BatchUploadItemId,
                BatchUploadId = batchUploadItem.BatchUploadId,
                Countries = enumDb.GetItemsByFilter(EnumTypeEnum.Country).SortDefaultCountryFirst()
            };

            var fields = GetFieldMap(batchUploadItem);
            result.OriginalValues["First name"] = fields[StudentRegistrationBatchColumn.FirstName].OriginValue;
            result.OriginalValues["Last name"] = fields[StudentRegistrationBatchColumn.LastName].OriginValue;
            result.OriginalValues["Address line 1"] = fields[StudentRegistrationBatchColumn.Street].OriginValue;
            result.OriginalValues["Address line 2"] = fields[StudentRegistrationBatchColumn.Street2].OriginValue;
            result.OriginalValues["City"] = fields[StudentRegistrationBatchColumn.City].OriginValue;
            result.OriginalValues["State"] = fields[StudentRegistrationBatchColumn.State].OriginValue;
            result.OriginalValues["Country"] = fields[StudentRegistrationBatchColumn.Country].OriginValue;
            result.OriginalValues["Zip code"] = fields[StudentRegistrationBatchColumn.ZipCode].OriginValue;
            result.OriginalValues["Phone"] = fields[StudentRegistrationBatchColumn.Phone].OriginValue;
            result.OriginalValues["Email"] = fields[StudentRegistrationBatchColumn.Email].OriginValue;
            return result;
        }

        public RegistrationBatchUploadForEdit CreateRegistrationBatchUploadForEdit(ICollection<BatchUploadTypeEnum> batchUploadTypes, long classId)
        {
            var result = new RegistrationBatchUploadForEdit();
            result.FileTypes = batchUploadDb.GetBatchUploadTypes(batchUploadTypes);
            result.RegistrationTypes = GetRegistrationTypes(classId);
            return result;
        }

        #region private methods

        private Dictionary<int, BatchUploadField> GetFieldMap(BatchUploadItem item)
        {
            if (item.BatchUploadFields == null)
            {
                throw new InvalidOperationException("BatchUploadFields is not included into BatchUploadItem object.");
            }

            var result = item.BatchUploadFields.ToDictionary(x => x.Order, y => y);
            return result;
        }

        private List<IEnumItem> GetRegistrationTypes(long classId)
        {
            var cls = GetClassById(classId);
            var allowedTypes = registrationLogicProvider.RegistrationTypeFeeder.GetAllowedTypesForBatchUploadRegistration(cls, RegistrationFormTypeEnum.Adult);
            var result = enumDb.GetItemsByFilter(EnumTypeEnum.MainRegistrationType)
                .Where(x => allowedTypes.Contains((RegistrationTypeEnum)x.Id)).ToList();
            return result;
        }

        private Class GetClassById(long classId, ClassIncludes includes = ClassIncludes.None)
        {
            var cls = classDb.GetClassById(classId, includes);
            if (cls == null)
            {
                throw new ArgumentException($"There is no class with id {classId}.");
            }

            return cls;
        }

        #endregion
    }
}
