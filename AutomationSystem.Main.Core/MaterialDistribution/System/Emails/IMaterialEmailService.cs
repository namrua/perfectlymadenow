using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System.Models;

namespace AutomationSystem.Main.Core.MaterialDistribution.System.Emails
{
    public interface IMaterialEmailService
    {
        TracedSendResult<RecipientId> SendMaterialEmailsToRecipients(
            EmailTypeEnum emailTypeId,
            long classId,
            List<RecipientId> recipientIds,
            bool createIncident = false);
    }
}
