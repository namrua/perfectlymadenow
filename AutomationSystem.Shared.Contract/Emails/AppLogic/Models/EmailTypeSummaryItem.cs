using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    /// <summary>
    /// Email type summary item
    /// </summary>
    public class EmailTypeSummaryItem
    {
        
        [DisplayName("Email type code")]
        public EmailTypeEnum EmailTypeId { get; set; }

        [DisplayName("Email type")]
        public string EmailType { get; set; }
        
        [DisplayName("Email templates")]
        public int EmailCount { get; set; }

        [DisplayName("Valid templates")]
        public int ValidEmailCount { get; set; }

        [DisplayName("Is localisable")]
        public bool IsLocalisable { get; set; }

        [DisplayName("Is valid")]
        public bool IsAllValid { get; set; }

    }

}
