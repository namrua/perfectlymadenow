using System;
using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models
{
    /// <summary>
    /// Batch upload list item
    /// </summary>
    public class BatchUploadListItem
    {

        [DisplayName("ID")]
        public long BatchUploadId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Parent entity ID")]
        public long? ParentEntityId { get; set; }

        [DisplayName("Batch upload type code")]
        public BatchUploadTypeEnum BatchUploadTypeId { get; set; }

        [DisplayName("Batch upload type")]
        public string BatchUploadType { get; set; }

        [DisplayName("Batch upload state code")]
        public BatchUploadStateEnum BatchUploadStateId { get; set; }

        [DisplayName("Batch upload state")]
        public string BatchUploadState { get; set; }

        [DisplayName("Uploaded")]
        public DateTime Uploaded { get; set; }

        [DisplayName("Is processed")]
        public bool IsProcessed { get; set; }

        [DisplayName("Processed")]
        public DateTime? Processed { get; set; }

        [DisplayName("Origin batch file")]
        public long? FileId { get; set; }

    }
    
}
