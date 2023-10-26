using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudentsBatchUploads;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using AutomationSystem.Shared.Contract.BatchUploads.Data;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic.Convertors
{

    /// <summary>
    /// Former student batch convertor
    /// </summary>
    public class FormerStudentBatchConvertor : IFormerStudentBatchConvertor
    {

        private readonly IEnumDatabaseLayer enumDb;
        private readonly IBatchUploadDatabaseLayer batchUploadDb;
        private readonly ICoreMapper coreMapper;

        public readonly Lazy<Dictionary<BatchUploadOperationTypeEnum, string>> operationMap;

        // constructor
        public FormerStudentBatchConvertor(
            IEnumDatabaseLayer enumDb,
            IBatchUploadDatabaseLayer batchUploadDb,
            ICoreMapper coreMapper)
        {
            this.enumDb = enumDb;
            this.batchUploadDb = batchUploadDb;
            this.coreMapper = coreMapper;

            operationMap = new Lazy<Dictionary<BatchUploadOperationTypeEnum, string>>(
                () => batchUploadDb.GetBatchUploadOperationTypes().ToDictionary(x => x.BatchUploadOperationTypeId, x => x.Name));
        }

        // converts batch upload to batch upload detail        
        public BatchUploadDetail<FormerStudentDetail> ConvertToBatchUploadDetail(BatchUpload batchUpload)
        {
            if (batchUpload.BatchUploadItems == null)
                throw new InvalidOperationException("BatchUploadItems is not included into BatchUpload object.");          

            var result = new BatchUploadDetail<FormerStudentDetail>
            {
                BatchUpload = coreMapper.Map<BatchUploadListItem>(batchUpload),
                Items = batchUpload.BatchUploadItems.Select(ConvertToBatchUploadItemListItem).ToList(),
                BatchUploadOperationTypes = batchUploadDb.GetBatchUploadOperationTypes()

            };
            return result;
        }

        #region batch item editing


        // update batch upload item
        public void UpdateBatchUploadItem(FormerStudentForm form, BatchUploadItem item)
        {
            var fields = GetFieldMap(item);
            fields[FormerStudentBatchColumn.FirstName].Value = form.Address.FirstName;
            fields[FormerStudentBatchColumn.LastName].Value = form.Address.LastName;
            fields[FormerStudentBatchColumn.Street].Value = form.Address.Street;
            fields[FormerStudentBatchColumn.Street2].Value = form.Address.Street2;
            fields[FormerStudentBatchColumn.City].Value = form.Address.City;
            fields[FormerStudentBatchColumn.State].Value = form.Address.State;
            fields[FormerStudentBatchColumn.ZipCode].Value = form.Address.ZipCode;
            fields[FormerStudentBatchColumn.Country].Value = form.Address.CountryId.HasValue ? ((int)form.Address.CountryId).ToString() : null;
            fields[FormerStudentBatchColumn.Phone].Value = form.Phone;
            fields[FormerStudentBatchColumn.Email].Value = form.Email;

            // makes item valid
            foreach (var field in item.BatchUploadFields)
            {
                field.IsValid = true;
                field.ValidationMessage = null;
            }
            item.IsValid = true;
        }

        #endregion


        #region Batch upload item converting       

        // converts batch upload item to batch upload item list item
        private BatchUploadItemListItem<FormerStudentDetail> ConvertToBatchUploadItemListItem(BatchUploadItem item)
        {
            if (item.BatchUploadFields == null)
                throw new InvalidOperationException("BatchUploadFields is not included into BatchUploadItem object.");

            var fields = GetFieldValueMap(item);
            var result = new BatchUploadItemListItem<FormerStudentDetail>
            {
                BatchUploadItemId = item.BatchUploadItemId,
                BatchUploadOperationTypeId = item.BatchUploadOperationTypeId,
                BatchUploadOperationType = !item.BatchUploadOperationTypeId.HasValue ? null : operationMap.Value[item.BatchUploadOperationTypeId.Value],
                EntityId = item.EntityId,
                PairEntityId = item.PairEntityId,                
                IsValid = item.IsValid,
                ValidationMessages = item.BatchUploadFields.Where(x => !x.IsValid).Select(x => x.ValidationMessage).ToList(),
                Entity = ConvertToFormerStudentDetail(fields)
            };
            return result;
        }
        
        #endregion
        
        #region Former student converting

        // convert to former student form
        public FormerStudentForm ConvertToFormerStudentForm(BatchUploadItem item)
        {
            var fields = GetFieldValueMap(item);
            var result = new FormerStudentForm()
            {
                Email = fields[FormerStudentBatchColumn.Email],
                Phone = fields[FormerStudentBatchColumn.Phone],
                Address = ConvertToAddressForm(fields)
            };
            return result;
        }

       
        // converts batch item to former student
        public FormerStudent ConvertToFormerStudent(long formerClassId, BatchUploadItem item)
        {
            var fields = GetFieldValueMap(item);
            var result = new FormerStudent
            {
                FormerClassId = formerClassId,
                Email = fields[FormerStudentBatchColumn.Email],
                Phone = fields[FormerStudentBatchColumn.Phone],
                Address = ConvertToAddress(fields),
                IsTemporary = false,
            };
            return result;
        }
       
        // converts field map to former
        private FormerStudentDetail ConvertToFormerStudentDetail(Dictionary<int, string> fields)
        {
            var result = new FormerStudentDetail
            {
                Email = fields[FormerStudentBatchColumn.Email],
                Phone = fields[FormerStudentBatchColumn.Phone],
                Address = ConvertToAddressDetail(fields)
            };
            return result;
        }

        #endregion


        #region Address converting

        // convert batch upload item to address
        private AddressDetail ConvertToAddressDetail(IDictionary<int, string> fields)
        {
            var result = new AddressDetail
            {
                FirstName = fields[FormerStudentBatchColumn.FirstName],
                LastName = fields[FormerStudentBatchColumn.LastName],
                Street = fields[FormerStudentBatchColumn.Street],
                Street2 = fields[FormerStudentBatchColumn.Street2],
                City = fields[FormerStudentBatchColumn.City],
                State = fields[FormerStudentBatchColumn.State],                
                ZipCode = fields[FormerStudentBatchColumn.ZipCode],
            };
            var countryIdValue = fields[FormerStudentBatchColumn.Country];
            if (!string.IsNullOrEmpty(countryIdValue))
            {
                result.CountryId = (CountryEnum) int.Parse(countryIdValue);
                result.Country = enumDb.GetItemById(EnumTypeEnum.Country, (int) result.CountryId).Description;
            }

            result.FullName = MainTextHelper.GetFullName(result.FirstName, result.LastName);
            result.FullStreet = MainTextHelper.GetAddressStreet(result.Street, result.Street2);
            result.FullCity = MainTextHelper.GetAddressCityState(result.City, result.State, result.ZipCode);

            return result;
        }

        // convert batch upload item to address
        private Address ConvertToAddress(IDictionary<int, string> fields)
        {
            var result = new Address
            {
                FirstName = fields[FormerStudentBatchColumn.FirstName],
                LastName = fields[FormerStudentBatchColumn.LastName],
                Street = fields[FormerStudentBatchColumn.Street],
                Street2 = fields[FormerStudentBatchColumn.Street2],
                City = fields[FormerStudentBatchColumn.City],
                State = fields[FormerStudentBatchColumn.State],
                CountryId = (CountryEnum)int.Parse(fields[FormerStudentBatchColumn.Country]),
                ZipCode = fields[FormerStudentBatchColumn.ZipCode],
                ForRegistration = false,
                IsIncomplete = false
            };
            return result;
        }

        // convert batch upload item to address
        private AddressForm ConvertToAddressForm(IDictionary<int, string> fields)
        {
            var result = new AddressForm
            {
                FirstName = fields[FormerStudentBatchColumn.FirstName],
                LastName = fields[FormerStudentBatchColumn.LastName],
                Street = fields[FormerStudentBatchColumn.Street],
                Street2 = fields[FormerStudentBatchColumn.Street2],
                City = fields[FormerStudentBatchColumn.City],
                State = fields[FormerStudentBatchColumn.State],                
                ZipCode = fields[FormerStudentBatchColumn.ZipCode],              
            };
            var countryIdValue = fields[FormerStudentBatchColumn.Country];
            if (!string.IsNullOrEmpty(countryIdValue))
            {
                result.CountryId = (CountryEnum)int.Parse(countryIdValue);               
            }
            return result;
        }

        #endregion


        #region static methods

        // gets field value map
        public static Dictionary<int, string> GetFieldValueMap(BatchUploadItem item)
        {
            if (item.BatchUploadFields == null)
                throw new InvalidOperationException("BatchUploadFields is not included into BatchUploadItem object.");
            var result = item.BatchUploadFields.ToDictionary(x => x.Order, y => y.Value);
            return result;
        }

        // gets field map
        public static Dictionary<int, BatchUploadField> GetFieldMap(BatchUploadItem item)
        {
            if (item.BatchUploadFields == null)
                throw new InvalidOperationException("BatchUploadFields is not included into BatchUploadItem object.");
            var result = item.BatchUploadFields.ToDictionary(x => x.Order, y => y);
            return result;
        }

        #endregion

    }

}
