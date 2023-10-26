using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    /// <summary>
    /// List item of email template
    /// </summary>
    public class EmailTemplateListItem
    {

        [DisplayName("ID")]
        public long EmailTemplateId { get; set; }
       
        [DisplayName("Subject")]
        public string Subject { get; set; }

        [DisplayName("Email type code")]
        public EmailTypeEnum EmailTypeId { get; set; }

        [DisplayName("Language code")]
        public LanguageEnum LanguageId { get; set; }

        [DisplayName("Language")]
        public string Language { get; set; }

        [DisplayName("Is valid")]
        public bool IsValid { get; set; }

    }

}
