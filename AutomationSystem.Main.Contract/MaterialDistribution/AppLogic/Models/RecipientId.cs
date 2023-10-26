using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Models;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Recipient full id
    /// </summary>
    public class RecipientId : EntityId
    {
        public RecipientId(EntityTypeEnum recipientTypeId, long recipientId) : base(recipientTypeId, recipientId) { }
    }
}
