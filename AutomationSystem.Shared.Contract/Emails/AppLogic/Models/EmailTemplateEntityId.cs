using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Models;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    public class EmailTemplateEntityId : NullableEntityId
    {
        public EmailTemplateEntityId() { }

        public EmailTemplateEntityId (EntityTypeEnum? emailTemplateEntityTypeId, long? emailTemplateEntityId) : base(emailTemplateEntityTypeId, emailTemplateEntityId) { }

    }
}
