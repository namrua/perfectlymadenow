using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Emails.System.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.Classes.System.Emails
{
    public interface IClassEmailService
    {
        List<long> SendClassActionEmails(
            long classActionId,
            ClassActionRegistrationRestriction restriction,
            IEmailAttachmentProvider attachments = null,
            List<Person> persons = null,
            bool sealTemplate = false);
        
        List<long> SendClassTextMapEmailByTypeToPersons(
            EmailTypeEnum emailTemplateId,
            long classId,
            Dictionary<string, object> textMap,
            IEnumerable<long> recipientPersons);
        
        long SendClassTextMapEmailByTypeToRecipient(
            EmailTypeEnum emailTemplateId,
            long classId, Dictionary<string, object> textMap,
            RecipientType recipientType,
            IEmailAttachmentProvider attachments = null);
    }
}
