using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    /// <summary>
    /// Encapsulates informations for test sending of email
    /// </summary>
    public class EmailTestSendInfo
    {

        // public properties
        public string UserEmail { get; set; }
        public long EmailTemplateId { get; set; }
        public EmailEntityId EmailEntityId { get; set; } = new EmailEntityId(EntityTypeEnum.CoreEmail, 0);
        public string CurrentSubject { get; set; }                      // whether not null, it is used instead of template's subject
        public string CurrentText { get; set; }                         // whether not null, it is used instead of template's text
        public EmailEntityId ParameterEntityId { get; set; } = new EmailEntityId(EntityTypeEnum.CoreEmail, 0);

        // constructor
        public EmailTestSendInfo(string userEmail, long emailTemplateId)
        {
            UserEmail = userEmail;
            EmailTemplateId = emailTemplateId;
        }

    }

}
