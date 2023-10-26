using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Emails.System.Models;
using AutomationSystem.Shared.Contract.Identities.AppLogic.Extensions;
using System.Collections.Generic;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Persons.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.Data;

namespace AutomationSystem.Main.Core.Contacts.System.Emails
{
    public class ContactEmailService : IContactEmailService
    {
        private readonly IEmailDatabaseLayer emailDb;
        private readonly IPersonDatabaseLayer personDb;
        private readonly IContactEmailParameterResolverFactory contactParameterResolverFactory;
        private readonly IEmailTemplateResolver emailTemplateResolver;
        private readonly IEmailTextResolverFactory emailTextResolverFactory;
        private readonly IEmailServiceHelper helper;
        private readonly IIdentityResolver identityResolver;

        public ContactEmailService(
            IEmailDatabaseLayer emailDb,
            IPersonDatabaseLayer personDb,
            IContactEmailParameterResolverFactory contactParameterResolverFactory,
            IEmailTemplateResolver emailTemplateResolver,
            IEmailTextResolverFactory emailTextResolverFactory,
            IEmailServiceHelper helper,
            IIdentityResolver identityResolver)
        {
            this.emailDb = emailDb;
            this.personDb = personDb;
            this.contactParameterResolverFactory = contactParameterResolverFactory;
            this.emailTemplateResolver = emailTemplateResolver;
            this.emailTextResolverFactory = emailTextResolverFactory;
            this.helper = helper;
            this.identityResolver = identityResolver;
        }

        public List<long> SendContactListEmails(ContactList contactList)
        {
            var template = emailTemplateResolver.GetEmailTemplateByEmailTemplateEntityId(new EmailTemplateEntityId(EntityTypeEnum.MainContactList, contactList.ContactListId));
            var formerStudentResolver = contactParameterResolverFactory.CreateFormerStudentParameterResolver();
            var textResolver = emailTextResolverFactory.CreateEmailTextResolver(formerStudentResolver);
            var sender = GetSenderInfo(contactList.SenderId);

            var result = new List<long>();
            foreach (var contactListItem in contactList.ContactListItems)
            {
                formerStudentResolver.Bind(contactListItem.FormerStudent);

                var emailId = helper.SendEmailForTemplate(
                    template,
                    textResolver,
                    new EmailEntityId(EntityTypeEnum.MainContactList, contactList.ContactListId),
                    contactListItem.Email,
                    sender,
                    (int) SeverityEnum.High);
                result.Add(emailId);
            }

            emailDb.SetEmailTemplatesToSealed(new [] { template.EmailTemplateId });

            return result;
        }

        #region private methods

        private SenderInfo GetSenderInfo(long? senderId)
        {
            SenderInfo sender;
            if (senderId.HasValue)
            {
                var person = personDb.GetPersonById(senderId.Value, PersonIncludes.Address);
                sender = new SenderInfo
                {
                    SenderEmail = person.Email,
                    SenderName = MainTextHelper.GetFullName(person.Address.FirstName, person.Address.LastName)
                };
            }
            else
            {
                sender = new SenderInfo
                {
                    SenderEmail = identityResolver.GetCurrentIdentity().GetEmail(),
                    SenderName = identityResolver.GetCurrentIdentity().Name
                };
            }

            return sender;
        }

        #endregion
    }

}
