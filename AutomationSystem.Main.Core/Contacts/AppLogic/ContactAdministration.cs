using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.Contacts.AppLogic;
using AutomationSystem.Main.Contract.Contacts.AppLogic.Models;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;
using AutomationSystem.Main.Core.Contacts.Data;
using AutomationSystem.Main.Core.Contacts.Data.Models;
using AutomationSystem.Main.Core.Contacts.System.Emails;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Persons.Data.Models;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.System.Extensions;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using CorabeuControl.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationSystem.Main.Core.Contacts.AppLogic
{
    public class ContactAdministration : IContactAdministration
    {
        private readonly IContactDatabaseLayer contactDb;
        private readonly IContactEmailService contactEmailService;
        private readonly IContactProvider contactProvider;
        private readonly IIdentityResolver identityResolver;
        private readonly IProfileDatabaseLayer profileDb;
        private readonly IMainMapper mainMapper;
        private readonly IEmailIntegration emailIntegration;
        private readonly IPersonDatabaseLayer personDb;

        public ContactAdministration(
            IContactDatabaseLayer contactDb,
            IContactEmailService contactEmailService,
            IContactProvider contactProvider,
            IIdentityResolver identityResolver,
            IProfileDatabaseLayer profileDb,
            IMainMapper mainMapper,
            IEmailIntegration emailIntegration,
            IPersonDatabaseLayer personDb)
        {
            this.contactDb = contactDb;
            this.contactEmailService = contactEmailService;
            this.contactProvider = contactProvider;
            this.identityResolver = identityResolver;
            this.profileDb = profileDb;
            this.mainMapper = mainMapper;
            this.emailIntegration = emailIntegration;
            this.personDb = personDb;
        }

        public ContactListPageModel GetContactListPageModel(ContactListFilter filter, bool search)
        {
            var pageModel = new ContactListPageModel(filter);
            pageModel.WasSearched = search;

            var profileFilter = identityResolver.GetGrantedProfilesForEntitle(Entitle.MainContacts);
            var profiles = profileDb.GetProfilesByFilter(profileFilter);
            pageModel.Profiles = profiles.Select(x => DropDownItem.Item(x.ProfileId, x.Name)).ToList();
            if (filter.ProfileId == 0)
            {
                pageModel.Filter.ProfileId = profiles.FirstOrDefault()?.ProfileId ?? 0;
            }

            if (pageModel.WasSearched)
            {
                pageModel.Items = contactProvider.GetContacts(filter);
            }

            return pageModel;
        }

        public void AddContactToBlackList(string email)
        {
            var normalizedEmail = MainTextHelper.GetNormalizedEmail(email);
            var contact = new ContactBlackList
            {
                Email = normalizedEmail
            };
            contactDb.AddContactToBlackList(contact);
        }

        public void RemoveContactFromBlackList(string email)
        {
            contactDb.RemoveContactFromBlackList(MainTextHelper.GetNormalizedEmail(email));
        }

        public long CreateContactList(ContactListDefinition contactListDefinition)
        {
            identityResolver.CheckEntitleForProfileId(Entitle.MainContacts, contactListDefinition.ProfileId);

            contactListDefinition.ContactListItems = contactProvider.RemoveBlackListedItems(contactListDefinition.ContactListItems);
            var contactList = mainMapper.Map<ContactList>(contactListDefinition);
            contactList.OwnerId = identityResolver.GetOwnerId();

            var result = contactDb.InsertContactList(contactList);

            var emailTemplates = emailIntegration.CloneEmailTemplates(EmailTypeEnum.ContactNotification,
                new EmailTemplateEntityId(EntityTypeEnum.MainProfile, contactList.ProfileId), LanguageEnum.En);
            emailIntegration.SaveClonedEmailTemplates(emailTemplates, new EmailTemplateEntityId(EntityTypeEnum.MainContactList, result));

            return result;
        }

        public ContactListDetail GetContactListDetail(long contactListId)
        {
            var contactList = GetContactListById(contactListId,  ContactListIncludes.ContactListItems);
            identityResolver.CheckEntitleForProfileId(Entitle.MainContacts, contactList.ProfileId);

            var result = mainMapper.Map<ContactListDetail>(contactList);

            var senders = GetPersonsOnProfile(contactList.ProfileId);
            result.Senders = senders.Select(x => PickerItem.Item(x.PersonId, MainTextHelper.GetFullNameWithEmail(x.Address.FirstName, x.Address.LastName, x.Email))).ToList();

            var sender = senders.FirstOrDefault(x => x.PersonId == contactList.SenderId);
            if (sender != null)
            {
                result.SenderName = MainTextHelper.GetFullNameWithEmail(sender.Address.FirstName, sender.Address.LastName, sender.Email);
            }

            result.EmailTemplates = emailIntegration.GetEmailTemplateListItemsByEntity(new EmailTemplateEntityId(EntityTypeEnum.MainContactList, contactListId));
            return result;
        }

        public void UpdateSenderId(ContactListSenderForm contactListSenderForm)
        {
            var contactList = GetContactListById(contactListSenderForm.ContactListId);
            identityResolver.CheckEntitleForProfileId(Entitle.MainContacts, contactList.ProfileId);
            contactDb.SetContactListSenderId(contactListSenderForm.ContactListId, contactListSenderForm.SenderId);
        }

        public void NotifyContacts(long contactListId)
        {
            var contactList = GetContactListById(contactListId, ContactListIncludes.ContactListItemsFormerStudentAddress);
            identityResolver.CheckEntitleForProfileId(Entitle.MainContacts, contactList.ProfileId);
            if (contactList.IsNotified)
            {
                throw new InvalidOperationException($"Contact list with id {contactListId} was already notified.");
            }

            contactEmailService.SendContactListEmails(contactList);

            contactDb.SetContactListAsNotified(contactListId);
        }

        #region private methods

        private ContactList GetContactListById(long contactListId, ContactListIncludes includes = ContactListIncludes.None)
        {
            var contactList = contactDb.GetContactListById(contactListId, includes);
            if (contactList == null)
            {
                throw new ArgumentException($"There is no contact list with id: {contactListId}.");
            }

            return contactList;
        }

        private List<Person> GetPersonsOnProfile(long profileId)
        {
            var personFilter = new PersonFilter
            {
                IncludeDefaultProfile = true,
                ProfileIds = new List<long> { profileId }
            };
            var persons = personDb.GetPersons(personFilter, PersonIncludes.Address);
            return persons;
        }

        #endregion
    }
}
