using System;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Emails.AppLogic.Convertors
{
    /// <summary>
    /// Converts emails objects
    /// </summary>
    public class EmailConvertor : IEmailConvertor
    {

        // convets email to email detail
       public EmailDetail ConvertToEmailDetail(Email email)
       {
            var result = new EmailDetail
            {
                IsTestEmail = email.IsTestEmail,
                EntityTypeId = email.EntityTypeId,
                EntityType = email.EntityType?.Name,
                EntityId = email.EntityId,
                Created = email.Created,
                Sent = email.Sent,
                SendingAttempts = email.SendingAttempts,
                Recipient = email.Recipient,
                Subject = email.Subject,
                Body = email.Text
            };
            return result;
        }

        // converts email to email list item
        public EmailListItem ConvertToEmailListItem(Email email)
        {
            if (email.EmailTemplate.EmailType == null)
                throw new InvalidOperationException("EmailTemplate.EmailType is not included into Email object.");

            var result = new EmailListItem
            {
                EmailId = email.EmailId,
                Subject = email.Subject,
                EmailType = email.EmailTemplate.EmailType.Description,
                IsSent = email.IsSent,
                Created = email.Created,
                Sent = email.Sent
            };
            return result;
        }

    }
}
