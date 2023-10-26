using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudentsBatchUploads;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Main.Core.Enums.System;
using PerfectlyMadeInc.Helpers.Routines;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic
{
    public class FormerStudentBatchUploadFactory : IFormerStudentBatchUploadFactory
    {
        private readonly IEnumDatabaseLayer enumDb;

        public FormerStudentBatchUploadFactory(IEnumDatabaseLayer enumDb)
        {
            this.enumDb = enumDb;
        }

        public FormerStudentBatchItemForEdit CreateFormerStudentBatchItemForEdit(BatchUploadItem batchUploadItem)
        {
            var result = new FormerStudentBatchItemForEdit
            {
                BatchUploadItemId = batchUploadItem.BatchUploadItemId,
                BatchUploadId = batchUploadItem.BatchUploadId,
                Countries = enumDb.GetItemsByFilter(EnumTypeEnum.Country).SortDefaultCountryFirst()
            };

            // initializes original values
            var fields = GetFieldMap(batchUploadItem);
            result.OriginalValues["First name"] = fields[FormerStudentBatchColumn.FirstName].OriginValue;
            result.OriginalValues["Last name"] = fields[FormerStudentBatchColumn.LastName].OriginValue;
            result.OriginalValues["Address line 1"] = fields[FormerStudentBatchColumn.Street].OriginValue;
            result.OriginalValues["Address line 2"] = fields[FormerStudentBatchColumn.Street2].OriginValue;
            result.OriginalValues["City"] = fields[FormerStudentBatchColumn.City].OriginValue;
            result.OriginalValues["State"] = fields[FormerStudentBatchColumn.State].OriginValue;
            result.OriginalValues["Country"] = fields[FormerStudentBatchColumn.Country].OriginValue;
            result.OriginalValues["Zip code"] = fields[FormerStudentBatchColumn.ZipCode].OriginValue;
            result.OriginalValues["Phone"] = fields[FormerStudentBatchColumn.Phone].OriginValue;
            result.OriginalValues["Email"] = fields[FormerStudentBatchColumn.Email].OriginValue;
            return result;
        }

        #region private methods

        private Dictionary<int, BatchUploadField> GetFieldMap(BatchUploadItem item)
        {
            EntityHelper.CheckForNull(item.BatchUploadFields, "BatchUploadFields", "BatchUploadItem");

            var result = item.BatchUploadFields.ToDictionary(x => x.Order, y => y);
            return result;
        }

        #endregion
    }
}
