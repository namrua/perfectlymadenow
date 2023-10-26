using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Models;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    public class EmailEntityId : EntityId
    {
        public EmailEntityId(EntityTypeEnum emailEntityTypeId, long emailEntityId) : base(emailEntityTypeId, emailEntityId) { }
    }
}
