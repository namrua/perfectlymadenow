using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Localisation.AppLogic.Models
{
    /// <summary>
    /// Enum localisation form
    /// </summary>
    public class EnumLocalisationForm
    {

        [HiddenInput]
        public LanguageEnum LanguageId { get; set; }

        [HiddenInput]
        public EnumTypeEnum EnumTypeId { get; set; }

        [HiddenInput]
        public int ItemId { get; set; }


        // public properties        
        [MaxLength(100)]
        [DisplayName("Name")]
        public string Name { get; set; }

        [MaxLength(512)]
        [DisplayName("Description")]
        public string Description { get; set; }

    }

}
