using AutomationSystem.Base.Contract.Enums;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AutomationSystem.Main.Web.Helpers.BatchUploads
{
    public class BatchUploadHelper : IBatchUploadHelper
    {
        public const string batchItemOperationTypePrefix = "BatchOperationType";

        public Dictionary<long, BatchUploadOperationTypeEnum> ExtractBatchUploadsOperationTypes(FormCollection collection)
        {
            var result = new Dictionary<long, BatchUploadOperationTypeEnum>();
            foreach (var key in collection.AllKeys)
            {
                if (!key.StartsWith($"{batchItemOperationTypePrefix}-"))
                    continue;
                if (!long.TryParse(key.Substring(batchItemOperationTypePrefix.Length + 1), out var id))
                    continue;
                var stringValue = collection[key];
                if (!Enum.TryParse(stringValue, true, out BatchUploadOperationTypeEnum value))
                    continue;
                result[id] = value;
            }
            return result;
        }
    }
}