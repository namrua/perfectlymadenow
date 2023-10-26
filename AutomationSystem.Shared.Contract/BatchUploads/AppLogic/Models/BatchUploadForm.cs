using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models
{
    /// <summary>
    /// Batch upload form
    /// </summary>
    public class BatchUploadForm
    {        

        [HiddenInput]
        public long ParentEntityId { get; set; }

        [Required]
        [MaxLength(64)]
        [DisplayName("Name")]
        public string Name { get; set; }     

        [Required]
        [PickInputOptions(NoItemText = "no type", Placeholder = "select file type")]
        [DisplayName("Type")]
        public BatchUploadTypeEnum? BatchUploadTypeId { get; set; }

    }
    
}
