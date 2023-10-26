using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models
{
    /// <summary>
    /// Batch upload detail
    /// </summary>    
    public class BatchUploadDetail<TEnitity>
    {

        [DisplayName("Batch upload detail")]
        public BatchUploadListItem BatchUpload { get; set; }

        [DisplayName("Items")]
        public List<BatchUploadItemListItem<TEnitity>> Items { get; set; }

        [DisplayName("Invalid items")]
        public int InvalidItemsCount => Items.Count(x => !x.IsValid);

        [DisplayName("Batch upload operations")]
        public List<BatchUploadOperationType> BatchUploadOperationTypes { get; set; }

        // constructor
        public BatchUploadDetail()
        {
            BatchUpload = new BatchUploadListItem();
            Items = new List<BatchUploadItemListItem<TEnitity>>();
            BatchUploadOperationTypes = new List<BatchUploadOperationType>();
        }

    }
    
}
