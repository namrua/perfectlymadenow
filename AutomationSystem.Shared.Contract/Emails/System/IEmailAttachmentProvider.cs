using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Emails.System
{
    /// <summary>
    /// email attachment
    /// </summary>
    public interface IEmailAttachmentProvider
    {

        // gets field ids by entity
        List<long> GetFileIdsByEntity(EntityTypeEnum? entityTypeId, long? entityId);

    }

}
