using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.BatchUploads.System
{
    public interface IBatchUploadService
    {
        List<BatchUploadListItem> GetBatchUploadListItems(EntityTypeEnum parentEntityTypeId, long entityId);
    }
}
