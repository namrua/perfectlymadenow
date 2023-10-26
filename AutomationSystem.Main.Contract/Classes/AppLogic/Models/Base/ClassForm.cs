using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base
{
    /// <summary>
    /// Class form
    /// </summary>
    public class ClassForm
    {
        [HiddenInput]
        public long ClassId { get; set; }

        [HiddenInput]
        public long ProfileId { get; set; }

        // common code lists id preserving
        [HiddenInput]
        public long? CurrentPriceListId { get; set; }
        [HiddenInput]
        public long? CurrentIntegrationCode { get; set; }
        [HiddenInput]
        public long? CurrentPayPayKeyId { get; set; }
        [HiddenInput]
        public ClassState ClassState { get; set; }
        [HiddenInput]
        public ClassCategoryEnum ClassCategoryId { get; set; }
        [HiddenInput]
        public CurrencyEnum ProfileCurrencyId { get; set; }

        [Required]
        [DisplayName("Type of class")]
        [PickInputOptions(NoItemText = "no type", Placeholder = "select type of class")]
        public ClassTypeEnum? ClassTypeId { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Location")]
        public string Location { get; set; }

        [Required]
        [DisplayName("Translation")]
        [PickInputOptions(NoItemText = "not selected", Placeholder = "select translation")]
        public long? TranslationCode { get; set; }

        [Required]
        [DisplayName("Time zone")]
        [PickInputOptions]
        public TimeZoneEnum? TimeZoneId { get; set; }

        [Range(typeof(DateTime), "1/1/2000", "1/1/4000")]
        [DisplayName("Start of registration")]
        public DateTime? RegistrationStart { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/2000", "1/1/4000")]
        [DisplayName("End of registration")]
        public DateTime? RegistrationEnd { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/2000", "1/1/4000")]
        [DisplayName("Start of class")]
        public DateTime? EventStart { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/2000", "1/1/4000")]
        [DisplayName("End of class")]
        public DateTime? EventEnd { get; set; }
        
        [Required]
        [DisplayName("Coordinator")]
        [PickInputOptions(ControlType = PickControlType.TypeaheadDropDownInput, Placeholder = "select coordinator")]
        public long? CoordinatorId { get; set; }
        
        [DisplayName("Guest instructor")]
        [PickInputOptions(ControlType = PickControlType.TypeaheadDropDownInput, Placeholder = "select guest instructor")]
        public long? GuestInstructorId { get; set; }

        [DisplayName("Instructors")]
        [PickInputOptions(ControlType = PickControlType.TypeaheadSetInput, Placeholder = "select instructor")]
        public List<long> InstructorIds { get; set; } = new List<long>();

        [DisplayName("Approved staff")]
        [PickInputOptions(ControlType = PickControlType.TypeaheadSetInput, Placeholder = "select approved staff")]
        public List<long> ApprovedStaffIds { get; set; } = new List<long>();

        [Required]
        [DisplayName("Price list")]
        [PickInputOptions(NoItemText = "no price list", Placeholder = "select price list")]
        public long? PriceListId { get; set; }

        [Required]
        [DisplayName("PayPal key")]
        [PickInputOptions(NoItemText = "no PayPal key", Placeholder = "select PayPal key")]
        public long? PayPalKeyId { get; set; }

        [Required]
        [DisplayName("Integration")]
        [PickInputOptions(NoItemText = "no selection", Placeholder = "select integration")]
        public long? IntegrationCode { get; set; }      

        [DisplayName("Is WWA registration allowed")]
        public bool IsWwaFormAllowed { get; set; }
    }
}