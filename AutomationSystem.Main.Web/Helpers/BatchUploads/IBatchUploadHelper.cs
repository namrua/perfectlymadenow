using AutomationSystem.Base.Contract.Enums;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AutomationSystem.Main.Web.Helpers.BatchUploads
{
    public interface IBatchUploadHelper
    {
        Dictionary<long, BatchUploadOperationTypeEnum> ExtractBatchUploadsOperationTypes(FormCollection collection);
    }
}