using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Emails.System
{
    public interface IEmailAttachmentProviderWithAdding : IEmailAttachmentProvider
    {
        void AddFileId(EntityTypeEnum? entityTypeId, long? entityId, long fileId);
    }
}
