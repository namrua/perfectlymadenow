using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Class material form
    /// </summary>
    public class ClassMaterialFileForm
    {
        [HiddenInput]
        public long ClassId { get; set; }

        [HiddenInput]
        public long ClassMaterialFileId { get; set; }

        [Required]
        [MaxLength(128)]
        [DisplayName("Name")]
        public string DisplayName { get; set; }

        [Required]
        [DisplayName("Language")]
        [PickInputOptions(NoItemText = "no language", Placeholder = "select language")]
        public LanguageEnum? LanguageId { get; set; }
    }
}
