using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses
{
    /// <summary>
    /// Former class form
    /// </summary>
    public class FormerClassForm
    {

        [HiddenInput]
        public long FormerClassId { get; set; }

        [Required]
        [DisplayName("Type of class")]
        [PickInputOptions]
        public ClassTypeEnum? ClassTypeId { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Location")]
        public string Location { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/2000", "1/1/4000")]
        [DisplayName("Start of class")]
        public DateTime? EventStart { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/2000", "1/1/4000")]
        [DisplayName("End of class")]
        public DateTime? EventEnd { get; set; }
        
        [DisplayName("Profile")]
        [PickInputOptions(NoItemText = "no profile", Placeholder = "select profile")]
        public long? ProfileId { get; set; }


    }

}
