using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.Contacts.AppLogic.Models;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Core.Contacts.AppLogic;
using AutomationSystem.Main.Core.Contacts.Data;
using AutomationSystem.Main.Core.Contacts.Data.Models;
using AutomationSystem.Main.Core.Contacts.System.Emails;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Persons.Data.Models;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Tests.Profiles.TestingHelpers;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;
using ContactListItem = AutomationSystem.Main.Contract.Contacts.AppLogic.Models.ContactListItem;
using Profile = AutomationSystem.Main.Model.Profile;

namespace AutomationSystem.Main.Tests.Contacts.AppLogic
{
    public class ContactAdministrationTests
    {
        private readonly Mock<IContactDatabaseLayer> contactDbMock;
        private readonly Mock<IContactEmailService> contactEmailServiceMock;
        private readonly Mock<IContactProvider> contactProviderMock;
        private readonly Mock<IIdentityResolver> identityResolverMock;
        private readonly Mock<IProfileDatabaseLayer> profileDbMock;
        private readonly Mock<IMainMapper> mainMapperMock;
        private readonly Mock<IEmailIntegration> emailIntegrationMock;
        private readonly Mock<IPersonDatabaseLayer> personDbMock;

        public ContactAdministrationTests()
        {
            contactDbMock = new Mock<IContactDatabaseLayer>();
            contactEmailServiceMock = new Mock<IContactEmailService>();
            contactProviderMock = new Mock<IContactProvider>();
            identityResolverMock = new Mock<IIdentityResolver>();
            profileDbMock = new Mock<IProfileDatabaseLayer>();
            mainMapperMock = new Mock<IMainMapper>();
            emailIntegrationMock = new Mock<IEmailIntegration>();
            personDbMock = new Mock<IPersonDatabaseLayer>();
        }

        #region GetContactListPageModel() tests

        [Fact]
        public void GetContactListPageModel_AreProfilesGranted_ReturnProfileFilter()
        {
            // arrange
            var userGroups = new UserGroupsForEntitle(Entitle.MainContacts, UserGroupTypeEnum.MainProfile)
            {
                IncludeDefaultGroup = false,
                GrantedUserGroupIds = new HashSet<long> { 4, 5 }
            };
            var profiles = new List<Profile>
            {
                new Profile
                {
                    ProfileId = 4
                },
                new Profile
                {
                    ProfileId = 5
                }
            };
            identityResolverMock.SetupGetGrantedProfilesForEntitle().Returns(userGroups);
            profileDbMock.Setup(e => e.GetProfilesByFilter(It.IsAny<ProfileFilter>(), It.IsAny<ProfileIncludes>()))
                .Returns(profiles);
            var admin = CreateAdmin();

            // act
            var result = admin.GetContactListPageModel(new ContactListFilter(), false);

            // assert
            Assert.Collection(result.Profiles,
                item => Assert.Equal("4", item.Id),
                item => Assert.Equal("5", item.Id));
            identityResolverMock.VerifyGetGrantedProfilesForEntitle(Entitle.MainContacts, Times.Once());
            profileDbMock.Verify(e => e.GetProfilesByFilter(It.Is<ProfileFilter>(x =>
                    ProfileIdentityResolverExtensionsTestingHelper.CheckProfileFilterByUserGroups(x, userGroups)),
                It.IsAny<ProfileIncludes>()));
        }

        [Fact]
        public void GetContactListPageModel_NoProfiles_FilterProfileIdIsSet()
        {
            // arrange
            var userGroups = new UserGroupsForEntitle(Entitle.MainContacts, UserGroupTypeEnum.MainProfile)
            {
                IncludeDefaultGroup = false,
                GrantedUserGroupIds = new HashSet<long>()
            };
            identityResolverMock.SetupGetGrantedProfilesForEntitle().Returns(userGroups);
            profileDbMock.Setup(e => e.GetProfilesByFilter(It.IsAny<ProfileFilter>(), It.IsAny<ProfileIncludes>()))
                .Returns(new List<Profile>());
            var admin = CreateAdmin();

            // act
            var result = admin.GetContactListPageModel(new ContactListFilter(), false);

            // assert
            Assert.Equal(0, result.Filter.ProfileId);
        }

        [Fact]
        public void GetContactListPageModel_WasSearched_PopulatesContactListItems()
        {
            // arrange
            var userGroups = new UserGroupsForEntitle(Entitle.MainContacts, UserGroupTypeEnum.MainProfile);
            var contactListFilter = new ContactListFilter();
            var contactListItems = new List<ContactListItem>();
            identityResolverMock.SetupGetGrantedProfilesForEntitle().Returns(userGroups);
            profileDbMock.Setup(e => e.GetProfilesByFilter(It.IsAny<ProfileFilter>(), It.IsAny<ProfileIncludes>()))
                .Returns(new List<Profile>());
            contactProviderMock.Setup(e => e.GetContacts(It.IsAny<ContactListFilter>())).Returns(contactListItems);
            var admin = CreateAdmin();

            // act
            var result = admin.GetContactListPageModel(contactListFilter, true);

            // assert
            Assert.Same(contactListItems, result.Items);
            contactProviderMock.Verify(e => e.GetContacts(contactListFilter), Times.Once);
        }

        #endregion

        #region AddContactToBlackList() tests

        [Fact]
        public void AddContactToBlackList_Email_ContactDbIsCalled()
        {
            // arrange
            var email = "TesTing@email.COM  ";
            var admin = CreateAdmin();

            // act
            admin.AddContactToBlackList(email);

            // assert
            contactDbMock.Verify(
                e => e.AddContactToBlackList(It.Is<ContactBlackList>(x => x.Email == "testing@email.com")), Times.Once);
        }

        #endregion

        #region RemoveContactFromBlackList() tests

        [Fact]
        public void RemoveContactFromBlackList_Email_ContactDbIsCalled()
        {
            // arrange
            var email = "TesTing@email.COM  ";
            var admin = CreateAdmin();

            // act
            admin.RemoveContactFromBlackList(email);

            // assert
            contactDbMock.Verify(e => e.RemoveContactFromBlackList("testing@email.com"), Times.Once);
        }

        #endregion

        #region CreateContactList() tests

        [Fact]
        public void CreateContactList_InaccessibleProfile_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var contactListDefinition = new ContactListDefinition
            {
                ProfileId = 5
            };
            identityResolverMock.SetupCheckEntitleForProfileId().Throws(EntitleAccessDeniedException.New(Entitle.MainContacts, new ClaimsIdentity()));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.CreateContactList(contactListDefinition));
            identityResolverMock.VerifyCheckEntitleForProfileId(Entitle.MainContacts, 5, Times.Once());
        }

        [Fact]
        public void CreateContactList_ContactListItemDefinitions_RemoveBlackListedItemsIsCalled()
        {
            // arrange
            var contactListDef = new ContactListDefinition();
            contactProviderMock.Setup(e => e.RemoveBlackListedItems(It.IsAny<List<ContactListItemDefinition>>())).Returns(new List<ContactListItemDefinition>());
            mainMapperMock.Setup(e => e.Map<ContactList>(It.IsAny<ContactListDefinition>())).Returns(new ContactList());
            var admin = CreateAdmin();

            // act
            admin.CreateContactList(contactListDef);

            // assert
            contactProviderMock.Verify(e => e.RemoveBlackListedItems(contactListDef.ContactListItems), Times.Once);
        }

        [Fact]
        public void CreateContactList_ContactListDefinition_IsMappedToContactList()
        {
            // arrange
            var contactListDef = new ContactListDefinition();
            mainMapperMock.Setup(e => e.Map<ContactList>(It.IsAny<ContactListDefinition>())).Returns(new ContactList());
            var admin = CreateAdmin();

            // act
            admin.CreateContactList(contactListDef);

            // assert
            mainMapperMock.Verify(e => e.Map<ContactList>(contactListDef), Times.Once);
        }

        [Fact]
        public void CreateContactList_ContactList_IsInsertedIntoDb()
        {
            // arrange
            var contactList = new ContactList();
            mainMapperMock.Setup(e => e.Map<ContactList>(It.IsAny<ContactListDefinition>())).Returns(contactList);
            contactDbMock.Setup(e => e.InsertContactList(It.IsAny<ContactList>())).Returns(1);
            var admin = CreateAdmin();

            // act
            var result = admin.CreateContactList(new ContactListDefinition());

            // assert
            Assert.Equal(1, result);
            contactDbMock.Verify(e => e.InsertContactList(contactList), Times.Once);
        }

        [Fact]
        public void CreateContactList_ContactListId_EmailTemplateIsCreatedWithContactListId()
        {
            // arrange
            var templates = new List<EmailTemplate>();
            mainMapperMock.Setup(e => e.Map<ContactList>(It.IsAny<ContactListDefinition>())).Returns(new ContactList());
            contactDbMock.Setup(e => e.InsertContactList(It.IsAny<ContactList>())).Returns(1);
            emailIntegrationMock.Setup(e => e.CloneEmailTemplates(
                    It.IsAny<EmailTypeEnum>(),
                    It.IsAny<EmailTemplateEntityId>(),
                    It.IsAny<LanguageEnum>()))
                .Returns(templates);
            var admin = CreateAdmin();

            // act
            admin.CreateContactList(new ContactListDefinition());

            // assert
            emailIntegrationMock.Verify(e => e.CloneEmailTemplates(
                    EmailTypeEnum.ContactNotification,
                    It.Is<EmailTemplateEntityId>(x => x.TypeId == EntityTypeEnum.MainProfile),
                    LanguageEnum.En),
                Times.Once);
            emailIntegrationMock.Verify(e =>
                    e.SaveClonedEmailTemplates(templates,
                        It.Is<EmailTemplateEntityId>(x => x.TypeId == EntityTypeEnum.MainContactList && x.Id == 1)),
                Times.Once());
        }

        #endregion

        #region GetContactListDetail() tests

        [Fact]
        public void GetContactListDetail_ExpectedIncludesPassToGetContactListById()
        {
            // arrange
            contactDbMock.Setup(e => e.GetContactListById(It.IsAny<long>(), It.IsAny<ContactListIncludes>())).Returns(new ContactList());
            personDbMock.Setup(e => e.GetPersons(It.IsAny<PersonFilter>(), It.IsAny<PersonIncludes>())).Returns(new List<Person>());
            mainMapperMock.Setup(e => e.Map<ContactListDetail>(It.IsAny<ContactList>())).Returns(new ContactListDetail());
            var admin = CreateAdmin();

            // act
            admin.GetContactListDetail(1);

            // assert
            contactDbMock.Verify(e => e.GetContactListById(1, ContactListIncludes.ContactListItems), Times.Once);
        }

        [Fact]
        public void GetContactListDetail_ContactListOnInaccessibleProfile_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var contactList = new ContactList
            {
                ProfileId = 5
            };
            contactDbMock.Setup(e => e.GetContactListById(It.IsAny<long>(), It.IsAny<ContactListIncludes>())).Returns(contactList);
            identityResolverMock.SetupCheckEntitleForProfileId().Throws(EntitleAccessDeniedException.New(Entitle.MainContacts, new ClaimsIdentity()));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetContactListDetail(1));
            identityResolverMock.VerifyCheckEntitleForProfileId(Entitle.MainContacts, 5, Times.Once());
        }

        [Fact]
        public void GetContactListDetail_NoContactListInDb_ThrowsArgumentException()
        {
            // arrange
            contactDbMock.Setup(e => e.GetContactListById(It.IsAny<long>(), It.IsAny<ContactListIncludes>())).Returns((ContactList)null);
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.GetContactListDetail(1));
        }

        [Fact]
        public void GetContactListDetail_ContactList_IsMappedToContactListDetail()
        {
            // arrange
            var contactList = new ContactList();
            var contactListDetail = new ContactListDetail();
            contactDbMock.Setup(e => e.GetContactListById(It.IsAny<long>(), It.IsAny<ContactListIncludes>()))
                .Returns(contactList);
            personDbMock.Setup(e => e.GetPersons(It.IsAny<PersonFilter>(), It.IsAny<PersonIncludes>())).Returns(new List<Person>());
            mainMapperMock.Setup(e => e.Map<ContactListDetail>(It.IsAny<ContactList>())).Returns(contactListDetail);
            var admin = CreateAdmin();

            // act
            var result = admin.GetContactListDetail(1);

            // assert
            Assert.Same(contactListDetail, result);
            mainMapperMock.Verify(e => e.Map<ContactListDetail>(contactList), Times.Once);
        }

        [Fact]
        public void GetContactListDetail_ContactListId_ReturnsEmailTemplateListItems()
        {
            // arrange
            var templates = new List<EmailTemplateListItem>();
            contactDbMock.Setup(e => e.GetContactListById(It.IsAny<long>(), It.IsAny<ContactListIncludes>()))
                .Returns(new ContactList());
            personDbMock.Setup(e => e.GetPersons(It.IsAny<PersonFilter>(), It.IsAny<PersonIncludes>())).Returns(new List<Person>());
            mainMapperMock.Setup(e => e.Map<ContactListDetail>(It.IsAny<ContactList>())).Returns(new ContactListDetail());
            emailIntegrationMock.Setup(e => e.GetEmailTemplateListItemsByEntity(It.IsAny<EmailTemplateEntityId>(), It.IsAny<bool>())).Returns(templates);
            var admin = CreateAdmin();

            // act
            var result = admin.GetContactListDetail(1);

            // assert
            Assert.Same(templates, result.EmailTemplates);
            emailIntegrationMock.Verify(e =>
                e.GetEmailTemplateListItemsByEntity(It.Is<EmailTemplateEntityId>(x => x.TypeId == EntityTypeEnum.MainContactList && x.Id == 1), false),
                Times.Once);
        }

        [Fact]
        public void GetContactListDetail_ProfileHasPersonsAndContactListHasSenderId_ReturnsSendersData()
        {
            // arrange
            var contactList = new ContactList
            {
                ProfileId = 3,
                SenderId = 20
            };
            contactDbMock.Setup(e => e.GetContactListById(It.IsAny<long>(), It.IsAny<ContactListIncludes>())).Returns(contactList);
            personDbMock.Setup(e => e.GetPersons(It.IsAny<PersonFilter>(), It.IsAny<PersonIncludes>())).Returns(CreateSenders);
            mainMapperMock.Setup(e => e.Map<ContactListDetail>(It.IsAny<ContactList>())).Returns(new ContactListDetail());
            var admin = CreateAdmin();

            // act
            var result = admin.GetContactListDetail(1);

            // assert
            personDbMock.Verify(
                e => e.GetPersons(
                    It.Is<PersonFilter>(x => x.IncludeDefaultProfile && x.ProfileIds != null && x.ProfileIds.Count == 1 && x.ProfileIds[0] == 3), 
                    PersonIncludes.Address), 
                Times.Once);
            Assert.Equal(MainTextHelper.GetFullNameWithEmail("second", "person", "second@person.com"), result.SenderName);
            Assert.Collection(
                result.Senders,
                item =>
                {
                    Assert.Equal("10", item.Value);
                    Assert.Equal(MainTextHelper.GetFullNameWithEmail("first", "person", "first@person.com"), item.Text);
                },
                item =>
                {
                    Assert.Equal("20", item.Value);
                    Assert.Equal(MainTextHelper.GetFullNameWithEmail("second", "person", "second@person.com"), item.Text);
                });
        }

        #endregion

        #region UpdateSenderId() tests

        [Fact]
        public void UpdateSenderId_ContactListOnInaccessibleProfile_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var contactListSenderForm = new ContactListSenderForm
            {
                ContactListId = 1
            };
            var contactList = new ContactList
            {
                ProfileId = 5
            };
            contactDbMock.Setup(e => e.GetContactListById(It.IsAny<long>(), It.IsAny<ContactListIncludes>())).Returns(contactList);
            identityResolverMock.SetupCheckEntitleForProfileId().Throws(EntitleAccessDeniedException.New(Entitle.MainContacts, new ClaimsIdentity()));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.UpdateSenderId(contactListSenderForm));
            identityResolverMock.VerifyCheckEntitleForProfileId(Entitle.MainContacts, 5, Times.Once());
            contactDbMock.Verify(e => e.GetContactListById(1, It.IsAny<ContactListIncludes>()));
        }

        [Fact]
        public void UpdateSenderId_OnContactList_SenderIdIsSet()
        {
            // arrange
            var contactListSenderForm = new ContactListSenderForm
            {
                ContactListId = 1,
                SenderId = 50
            };
            var contactList = new ContactList
            {
                ProfileId = 5
            };
            contactDbMock.Setup(e => e.GetContactListById(It.IsAny<long>(), It.IsAny<ContactListIncludes>())).Returns(contactList);
            var admin = CreateAdmin();

            // act & assert
            admin.UpdateSenderId(contactListSenderForm);
            contactDbMock.Verify(e => e.SetContactListSenderId(1, 50), Times.Once);
        }

        #endregion

        #region NotifyContacts() tests

        [Fact]
        public void NotifyContacts_ExpectedIncludesPassToGetContactListById()
        {
            // arrange
            var contactList = new ContactList();
            contactDbMock.Setup(e => e.GetContactListById(It.IsAny<long>(), It.IsAny<ContactListIncludes>())).Returns(contactList);
            var admin = CreateAdmin();

            // act
            admin.NotifyContacts(1);

            // assert
            contactDbMock.Verify(e => e.GetContactListById(1, ContactListIncludes.ContactListItemsFormerStudentAddress), Times.Once);
        }

        [Fact]
        public void NotifyContacts_ContactListId_ContactListIsNotified()
        {
            // arrange
            var contactList = new ContactList();
            contactDbMock.Setup(e => e.GetContactListById(It.IsAny<long>(), It.IsAny<ContactListIncludes>())).Returns(contactList);
            var admin = CreateAdmin();

            // act
            admin.NotifyContacts(1);

            // assert
            contactEmailServiceMock.Verify(e => e.SendContactListEmails(contactList), Times.Once);
            contactDbMock.Verify(e => e.SetContactListAsNotified(1), Times.Once);
        }

        [Fact]
        public void NotifyContacts_AlreadyNotifiedContactList_ThrowsInvalidOperationException()
        {
            // arrange
            var contactList = new ContactList
            {
                IsNotified = true
            };
            contactDbMock.Setup(e => e.GetContactListById(It.IsAny<long>(), It.IsAny<ContactListIncludes>())).Returns(contactList);
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => admin.NotifyContacts(1));
        }

        [Fact]
        public void NotifyContacts_ContactListOnInaccessibleProfile_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var contactList = new ContactList
            {
                ProfileId = 5
            };
            contactDbMock.Setup(e => e.GetContactListById(It.IsAny<long>(), It.IsAny<ContactListIncludes>())).Returns(contactList);
            identityResolverMock.SetupCheckEntitleForProfileId().Throws(EntitleAccessDeniedException.New(Entitle.MainContacts, new ClaimsIdentity()));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.NotifyContacts(1));
            identityResolverMock.VerifyCheckEntitleForProfileId(Entitle.MainContacts, 5, Times.Once());
        }

        #endregion

        #region private methods

        private Person CreateSender(long personId, string firstName, string lastName, string email)
        {
            return new Person
            {
                PersonId = personId,
                Email = email,
                Address = new Address
                {
                    FirstName = firstName,
                    LastName = lastName
                }
            };
        }

        private List<Person> CreateSenders()
        {
            var result = new List<Person>
            {
                CreateSender(10, "first", "person", "first@person.com"),
                CreateSender(20, "second", "person", "second@person.com")
            };
            return result;
        }

        private ContactAdministration CreateAdmin()
        {
            return new ContactAdministration(
                contactDbMock.Object,
                contactEmailServiceMock.Object,
                contactProviderMock.Object,
                identityResolverMock.Object,
                profileDbMock.Object,
                mainMapperMock.Object,
                emailIntegrationMock.Object,
                personDbMock.Object);
        }

        #endregion
    }
}
