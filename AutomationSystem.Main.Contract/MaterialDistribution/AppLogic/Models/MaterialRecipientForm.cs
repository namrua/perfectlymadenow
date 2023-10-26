using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Material recipient
    /// </summary>
    public class MaterialRecipientForm
    {
        [HiddenInput]
        public long ClassMaterialRecipientId { get; set; }

        [HiddenInput]
        public EntityTypeEnum RecipientTypeId { get; set; }

        [HiddenInput]
        public long RecipientId { get; set; }

        [Required]
        [MaxLength(32)]
        [DisplayName("Student's password")]
        public string Password { get; set; }

        [PickInputOptions(NoItemText = "no language", Placeholder = "select language")]
        [DisplayName("Language")]
        public LanguageEnum? LanguageId { get; set; }


        [Range(0, 100)]
        [SpinnerInputOptions(Min = 0, Max = 100)]
        [DisplayName("Download limit")]
        public int? DownloadLimit { get; set; }

    }
}
