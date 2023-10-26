using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.RegistrationUpload.AppLogic.Models
{
    public class RegistrationBatchUploadForm : BatchUploadForm
    {
        [Required]
        [PickInputOptions(NoItemText = "no registration type", Placeholder = "select registration type")]
        [DisplayName("Registration types")]
        public RegistrationTypeEnum RegistrationTypeId { get; set; }
    }
}
