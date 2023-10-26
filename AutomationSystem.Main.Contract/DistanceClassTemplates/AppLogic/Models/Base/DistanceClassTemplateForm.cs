using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base
{
    public class DistanceClassTemplateForm
    {
        [HiddenInput]
        public long DistanceClassTemplateId { get; set; }

        [Required]
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

        [Required]
        [DisplayName("Type of class")]
        [PickInputOptions(NoItemText = "not selected", Placeholder = "select type of class")]
        public ClassTypeEnum? ClassTypeId { get; set; }

        [Required]
        [DisplayName("Translation")]
        [PickInputOptions(NoItemText = "not selected", Placeholder = "select translation")]
        public long? TranslationCode { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/2000", "1/1/4000")]
        [DisplayName("Start of registration")]
        public DateTime? RegistrationStart { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/2000", "1/1/4000")]
        [DisplayName("End of registration")]
        public DateTime? RegistrationEnd { get; set; }

        [DisplayName("Guest instructor")]
        [PickInputOptions(ControlType = PickControlType.TypeaheadDropDownInput, Placeholder = "select guest instructor")]
        public long? GuestInstructorId { get; set; }

        [DisplayName("Instructors")]
        [PickInputOptions(ControlType = PickControlType.TypeaheadSetInput, Placeholder = "select instructor")]
        public List<long> InstructorIds { get; set; } = new List<long>();
    }
}
