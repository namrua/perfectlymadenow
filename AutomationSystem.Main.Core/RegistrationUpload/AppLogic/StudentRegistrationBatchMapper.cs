using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.RegistrationUpload.AppLogic.Models;
using AutomationSystem.Shared.Contract;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using AutomationSystem.Shared.Contract.BatchUploads.Data;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.Helpers.Routines;

namespace AutomationSystem.Main.Core.RegistrationUpload.AppLogic
{
    public class StudentRegistrationBatchMapper : IStudentRegistrationBatchMapper
    {
        private readonly IBatchUploadDatabaseLayer batchUploadDb;
        private readonly IEnumDatabaseLayer enumDb;
        private readonly ICoreMapper coreMapper;

        public readonly Lazy<Dictionary<BatchUploadOperationTypeEnum, string>> operationMap;

        public StudentRegistrationBatchMapper(
            IEnumDatabaseLayer enumDb,
            IBatchUploadDatabaseLayer batchUploadDb,
            ICoreMapper coreMapper)
        {
            this.batchUploadDb = batchUploadDb;
            this.enumDb = enumDb;
            this.coreMapper = coreMapper;

            operationMap = new Lazy<Dictionary<BatchUploadOperationTypeEnum, string>>(
                () => batchUploadDb.GetBatchUploadOperationTypes().ToDictionary(x => x.BatchUploadOperationTypeId, x => x.Name));
        }

        public BatchUploadDetail<RegistrationStudentDetail> MapToBatchUploadDetail(BatchUpload batchUpload)
        {
            EntityHelper.CheckForNull(batchUpload.BatchUploadItems, "BatchUploadItems", "BatchUpload");

            var result = new BatchUploadDetail<RegistrationStudentDetail>
            {
                BatchUpload = coreMapper.Map<BatchUploadListItem>(batchUpload),
                Items = batchUpload.BatchUploadItems.Select(MapToBatchUploadItemListItem).ToList(),
                BatchUploadOperationTypes = batchUploadDb.GetBatchUploadOperationTypes()
                    .Where(x => x.BatchUploadOperationTypeId != BatchUploadOperationTypeEnum.Update)
                    .ToList()
            };
            return result;
        }

        public RegistrationStudentForm MapToStudentForm(BatchUploadItem item)
        {
            var fields = GetFieldValueMap(item);
            var result = new RegistrationStudentForm
            {
                Email = fields[StudentRegistrationBatchColumn.Email],
                Phone = fields[StudentRegistrationBatchColumn.Phone],
                Address = MapToAddressForm(fields)
            };
            return result;
        }

        public void UpdateBatchUploadItem(RegistrationStudentForm form, BatchUploadItem item)
        {
            var fields = GetFieldMap(item);
            fields[StudentRegistrationBatchColumn.FirstName].Value = form.Address.FirstName;
            fields[StudentRegistrationBatchColumn.LastName].Value = form.Address.LastName;
            fields[StudentRegistrationBatchColumn.Street].Value = form.Address.Street;
            fields[StudentRegistrationBatchColumn.Street2].Value = form.Address.Street2;
            fields[StudentRegistrationBatchColumn.City].Value = form.Address.City;
            fields[StudentRegistrationBatchColumn.State].Value = form.Address.State;
            fields[StudentRegistrationBatchColumn.ZipCode].Value = form.Address.ZipCode;
            fields[StudentRegistrationBatchColumn.Country].Value = form.Address.CountryId.HasValue ? ((int)form.Address.CountryId).ToString() : null;
            fields[StudentRegistrationBatchColumn.Phone].Value = form.Phone;
            fields[StudentRegistrationBatchColumn.Email].Value = form.Email;

            // makes item valid
            foreach (var field in item.BatchUploadFields)
            {
                field.IsValid = true;
                field.ValidationMessage = null;
            }
            item.IsValid = true;
        }

        #region private methods

        private BatchUploadItemListItem<RegistrationStudentDetail> MapToBatchUploadItemListItem(BatchUploadItem item)
        {
            EntityHelper.CheckForNull(item.BatchUploadFields, "BatchUploadFields", "BatchUploadItem");

            var fields = GetFieldValueMap(item);
            var result = new BatchUploadItemListItem<RegistrationStudentDetail>
            {
                BatchUploadItemId = item.BatchUploadItemId,
                BatchUploadOperationTypeId = item.BatchUploadOperationTypeId,
                BatchUploadOperationType = !item.BatchUploadOperationTypeId.HasValue ? null : operationMap.Value[item.BatchUploadOperationTypeId.Value],
                EntityId = item.EntityId,
                PairEntityId = item.PairEntityId,
                IsValid = item.IsValid,
                ValidationMessages = item.BatchUploadFields.Where(x => !x.IsValid).Select(x => x.ValidationMessage).ToList(),
                Entity = MapToRegistrationStudentDetail(fields)
            };
            return result;
        }

        public static Dictionary<int, string> GetFieldValueMap(BatchUploadItem item)
        {
            EntityHelper.CheckForNull(item.BatchUploadFields, "BatchUploadFields", "BatchUploadItem");

            var result = item.BatchUploadFields.ToDictionary(x => x.Order, y => y.Value);
            return result;
        }

        private Dictionary<int, BatchUploadField> GetFieldMap(BatchUploadItem item)
        {
            EntityHelper.CheckForNull(item.BatchUploadFields, "BatchUploadFields", "BatchUploadItem");

            var result = item.BatchUploadFields.ToDictionary(x => x.Order, y => y);
            return result;
        }

        private RegistrationStudentDetail MapToRegistrationStudentDetail(Dictionary<int, string> fields)
        {
            var result = new RegistrationStudentDetail
            {
                Email = fields[StudentRegistrationBatchColumn.Email],
                Phone = fields[StudentRegistrationBatchColumn.Phone],
                Address = MapToAddressDetail(fields)
            };
            return result;
        }

        private AddressDetail MapToAddressDetail(IDictionary<int, string> fields)
        {
            var result = new AddressDetail
            {
                FirstName = fields[StudentRegistrationBatchColumn.FirstName],
                LastName = fields[StudentRegistrationBatchColumn.LastName],
                Street = fields[StudentRegistrationBatchColumn.Street],
                Street2 = fields[StudentRegistrationBatchColumn.Street2],
                City = fields[StudentRegistrationBatchColumn.City],
                State = fields[StudentRegistrationBatchColumn.State],
                ZipCode = fields[StudentRegistrationBatchColumn.ZipCode],
            };
            var countryIdValue = fields[StudentRegistrationBatchColumn.Country];
            if (!string.IsNullOrEmpty(countryIdValue))
            {
                result.CountryId = (CountryEnum)int.Parse(countryIdValue);
                result.Country = enumDb.GetItemById(EnumTypeEnum.Country, (int)result.CountryId).Description;
            }

            result.FullName = MainTextHelper.GetFullName(result.FirstName, result.LastName);
            result.FullStreet = MainTextHelper.GetAddressStreet(result.Street, result.Street2);
            result.FullCity = MainTextHelper.GetAddressCityState(result.City, result.State, result.ZipCode);

            return result;
        }

        private AddressForm MapToAddressForm(IDictionary<int, string> fields)
        {
            var result = new AddressForm
            {
                FirstName = fields[StudentRegistrationBatchColumn.FirstName],
                LastName = fields[StudentRegistrationBatchColumn.LastName],
                Street = fields[StudentRegistrationBatchColumn.Street],
                Street2 = fields[StudentRegistrationBatchColumn.Street2],
                City = fields[StudentRegistrationBatchColumn.City],
                State = fields[StudentRegistrationBatchColumn.State],
                ZipCode = fields[StudentRegistrationBatchColumn.ZipCode],
            };
            var countryIdValue = fields[StudentRegistrationBatchColumn.Country];
            if (!string.IsNullOrEmpty(countryIdValue))
            {
                result.CountryId = (CountryEnum)int.Parse(countryIdValue);
            }

            return result;
        }

        #endregion
    }
}
