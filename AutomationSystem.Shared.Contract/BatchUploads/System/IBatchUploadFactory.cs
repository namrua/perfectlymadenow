using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.BatchUploads.System
{
    public interface IBatchUploadFactory
    {
        BatchUpload CreateBatchUpload(
            List<string[]> data,
            BatchUploadForm form,
            EntityTypeEnum parentEntityTypeId,
            EntityTypeEnum uploadedEntityTypeId,
            IBatchUploadValueResolver batchUploadValueResolver,
            string jsonParameter = null);

        BatchUpload CreateBatchUpload<T>(
            List<string[]> data,
            BatchUploadForm form,
            EntityTypeEnum parentEntityTypeId,
            EntityTypeEnum uploadedEntityTypeId,
            IBatchUploadValueResolver batchUploadValueResolver,
            T parameter);
    }
}
