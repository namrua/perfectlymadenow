namespace AutomationSystem.Shared.Contract.Emails.System
{
    public interface IEmailAttachmentProviderFactory
    {
        IEmailAttachmentProvider CreateSimpleEmailAttachmentProvider(params long[] fileIds);
        IEmailAttachmentProviderWithAdding CreateMapEmailAttachmentProvider();
    }
}
