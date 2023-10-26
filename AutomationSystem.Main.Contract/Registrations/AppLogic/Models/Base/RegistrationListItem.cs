using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base
{
    /// <summary>
    /// Encapsulates registration list item
    /// </summary>
    public class RegistrationListItem
    {
       
        [DisplayName("ID")]
        public long ClassRegistrationId { get; set; }

        [DisplayName("Student name")]
        public string StudentName { get; set; }

        [DisplayName("Registrant name")]
        public string RegistrantName { get; set; }

        [DisplayName("Country")]
        public string Country { get; set; }

        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }


        [DisplayName("Registration type code")]
        public RegistrationTypeEnum RegistrationTypeId { get; set; }

        [DisplayName("Registration type")]
        public string RegistrationType { get; set; }

        [DisplayName("Registration state")]
        public RegistrationState RegistrationState { get; set; }

        [DisplayName("Approvement type code")]
        public ApprovementTypeEnum ApprovementTypeId { get; set; }

        [DisplayName("Approvement type")]
        public string ApprovementType { get; set; }


        [DisplayName("Language code")]
        public LanguageEnum LanguageId { get; set; }

        [DisplayName("Language")]
        public string Language { get; set; }

        [DisplayName("Review state")]
        public bool? IsReviewed { get; set; }

        [DisplayName("Created")]    
        public DateTime Created { get; set; }

        [DisplayName("Approved")]
        public DateTime? Approved { get; set; }

        [DisplayName("Canceled")]
        public DateTime? Canceled { get; set; }


        [DisplayName("Name")] public string Name { get; set; }

    }

}
