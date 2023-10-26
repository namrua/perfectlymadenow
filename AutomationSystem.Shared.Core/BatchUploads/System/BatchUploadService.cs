using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using AutomationSystem.Shared.Contract.BatchUploads.Data;
using AutomationSystem.Shared.Contract.BatchUploads.Data.Models;
using AutomationSystem.Shared.Contract.BatchUploads.System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationSystem.Shared.Core.BatchUploads.System
{
    public class BatchUploadService : IBatchUploadService
    {
        private readonly IBatchUploadDatabaseLayer batchUploadDb;
        private readonly ICoreMapper mapper;

        public BatchUploadService(
            IBatchUploadDatabaseLayer batchUploadDb,
            ICoreMapper mapper)
        {
            this.batchUploadDb = batchUploadDb;
            this.mapper = mapper;
        }

        public List<BatchUploadListItem> GetBatchUploadListItems(EntityTypeEnum parentEntityTypeId, long entityId)
        {
            var batchUploads = batchUploadDb.GetBatchUploadByParent(parentEntityTypeId, entityId,
                BatchUploadIncludes.BatchUploadState | BatchUploadIncludes.BatchUploadType);
            var result = batchUploads.Select(mapper.Map<BatchUploadListItem>).ToList();
            return result;
        }
    }
}
