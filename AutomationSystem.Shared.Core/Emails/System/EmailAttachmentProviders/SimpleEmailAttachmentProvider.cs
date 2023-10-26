using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Shared.Core.Emails.System.EmailAttachmentProviders
{
    /// <summary>
    /// Serves as simple email attachment provider
    /// Just pass list of fileIds independent on the entity type
    /// </summary>
    public class SimpleEmailAttachmentProvider : IEmailAttachmentProvider
    {

        private readonly List<long> fileIds;

        // constructor
        public SimpleEmailAttachmentProvider(params long[] fileIds)
        {
            this.fileIds = new List<long>(fileIds);
        }

        // gets field ids by entity
        public List<long> GetFileIdsByEntity(EntityTypeEnum? entityTypeId, long? entityId)
        {
            return fileIds;
        }

    }

}
