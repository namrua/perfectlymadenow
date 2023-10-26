using System.Collections.Generic;
using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models
{
    /// <summary>
    /// List item for Batch upload item
    /// </summary>    
    public class BatchUploadItemListItem<TEnitity>
    {

        [DisplayName("Entity detail")]
        public TEnitity Entity { get; set; }


        [DisplayName("ID")]
        public long BatchUploadItemId { get; set; }

        [DisplayName("Operation code")]
        public BatchUploadOperationTypeEnum? BatchUploadOperationTypeId { get; set; }

        [DisplayName("Operation")]
        public string BatchUploadOperationType { get; set; }

        [DisplayName("Entity ID")]
        public long? EntityId { get; set; }

        [DisplayName("Paired entity ID")]
        public long? PairEntityId { get; set; }      

        [DisplayName("Is valid")]
        public bool IsValid { get; set; }

        [DisplayName("Validation messages")]
        public List<string> ValidationMessages { get; set; }     
        

        // constructor
        public BatchUploadItemListItem()
        {
            ValidationMessages = new List<string>();
        }

    }
    
}
