using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    /// <summary>
    /// Email detail
    /// </summary>
    public class EmailDetail
    {

        [DisplayName("Is test email")]
        public bool IsTestEmail { get; set; }       

        [DisplayName("Related entity type code")]
        public EntityTypeEnum? EntityTypeId { get; set; }

        [DisplayName("Related entity type")]
        public string EntityType { get; set; }

        [DisplayName("Related entity ID")]
        public long? EntityId { get; set; }


        [DisplayName("Created")]
        public DateTime Created { get; set; }

        [DisplayName("Sent")]
        public DateTime? Sent { get; set; }

        [DisplayName("Sending attempts")]
        public int SendingAttempts { get; set; }


        [DisplayName("Recipient")]
        [EmailAddress]
        public string Recipient { get; set; }

        [DisplayName("Subject")]
        public string Subject { get; set; }

        [DisplayName("Body")]
        [TextInputOptions(ControlType = TextControlType.AceTextInput)]
        public string Body { get; set; }

    }

}
