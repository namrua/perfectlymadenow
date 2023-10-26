using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Shared.Contract.Localisation.AppLogic.Models
{
    /// <summary>
    /// Application localisation form
    /// </summary>
    public class AppLocalisationForm
    {

        [HiddenInput]
        public LanguageEnum LanguageId { get; set; }

        [HiddenInput]
        public long AppLocalisationOriginId { get; set; }     

        [Required]
        [MaxLength(8000)]
        [AllowHtml]
        [DisplayName("Html text")]
        [TextInputOptions(ControlType = TextControlType.AceTextInput)]
        public string Value { get; set; }

    }

}
