using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Core.Emails.System.EmailAttachmentProviders;

namespace AutomationSystem.Shared.Core.Emails.System
{
    public class EmailAttachmentProviderFactory : IEmailAttachmentProviderFactory
    {
        public IEmailAttachmentProvider CreateSimpleEmailAttachmentProvider(params long[] fileIds)
        {
            return new SimpleEmailAttachmentProvider(fileIds);
        }

        public IEmailAttachmentProviderWithAdding CreateMapEmailAttachmentProvider()
        {
            return new MapEmailAttachmentProvider();
        }
    }
}
