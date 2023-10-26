using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using AutomationSystem.Shared.Contract.BatchUploads.System;
using AutomationSystem.Shared.Model;
using Newtonsoft.Json;

namespace AutomationSystem.Shared.Core.BatchUploads.System
{
    public class BatchUploadFactory : IBatchUploadFactory
    {
        public BatchUpload CreateBatchUpload(
            List<string[]> data,
            BatchUploadForm form,
            EntityTypeEnum parentEntityTypeId,
            EntityTypeEnum uploadedEntityTypeId,
            IBatchUploadValueResolver batchUploadValueResolver,
            string jsonParameter = null)
        {
            var result = new BatchUpload
            {
                BatchUploadTypeId = form.BatchUploadTypeId ?? 0,
                BatchUploadStateId = BatchUploadStateEnum.InUploading,
                Name = form.Name,
                UploadedEntityTypeId = uploadedEntityTypeId,
                ParentEntityTypeId = parentEntityTypeId,
                ParentEntityId = form.ParentEntityId,
                Uploaded = DateTime.Now,
                JsonParameters = jsonParameter,
                BatchUploadItems = data.Select(x => ConvertToBatchUploadItem(x, batchUploadValueResolver)).ToList()
            };

            return result;
        }

        public BatchUpload CreateBatchUpload<T>(
            List<string[]> data,
            BatchUploadForm form,
            EntityTypeEnum parentEntityTypeId,
            EntityTypeEnum uploadedEntityTypeId,
            IBatchUploadValueResolver batchUploadValueResolver,
            T parameter)
        {
            var jsonParameter = JsonConvert.SerializeObject(parameter);
            var result = CreateBatchUpload(data, form, parentEntityTypeId, uploadedEntityTypeId, batchUploadValueResolver, jsonParameter);
            return result;
        }

        #region private methods

        private BatchUploadItem ConvertToBatchUploadItem(string[] dataLine, IBatchUploadValueResolver batchUploadValueResolver)
        {
            var values = batchUploadValueResolver.GetValues(dataLine);
            var result = new BatchUploadItem();
            for (int i = 0; i < dataLine.Length; i++)
            {
                var field = new BatchUploadField
                {
                    Order = i,
                    OriginValue = dataLine[i],
                    Value = values[i]
                };
                result.BatchUploadFields.Add(field);
            }

            return result;
        }

        #endregion
    }
}
