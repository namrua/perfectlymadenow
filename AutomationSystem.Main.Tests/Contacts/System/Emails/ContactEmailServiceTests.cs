using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Core.Contacts.System.Emails;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Emails.System.Models;
using AutomationSystem.Shared.Model;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Persons.Data.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using Xunit;

namespace AutomationSystem.Main.Tests.Contacts.System.Emails
{
    public class ContactEmailServiceTests
    {
        private const string SenderEmail = "senderEmail@email.com";
        private const string SenderName = "Name";
        private const long EmailTemplateId = 123;


        private readonly Mock<IEmailDatabaseLayer> emailDbMock;
        private readonly Mock<IPersonDatabaseLayer> personDbMock;
        private readonly Mock<IContactEmailParameterResolverFactory> contactParameterResolverFactoryMock;
        private readonly Mock<IEmailTemplateResolver> emailTemplateResolverMock;
        private readonly Mock<IEmailTextResolverFactory> emailTextResolverFactoryMock;
        private readonly Mock<IEmailServiceHelper> helperMock;
        private readonly Mock<IIdentityResolver> identityResolverMock;

        public ContactEmailServiceTests()
        {
            emailDbMock = new Mock<IEmailDatabaseLayer>();
            personDbMock = new Mock<IPersonDatabaseLayer>();
            contactParameterResolverFactoryMock = new Mock<IContactEmailParameterResolverFactory>();
            emailTemplateResolverMock = new Mock<IEmailTemplateResolver>();
            emailTextResolverFactoryMock = new Mock<IEmailTextResolverFactory>();
            helperMock = new Mock<IEmailServiceHelper>();
            identityResolverMock = new Mock<IIdentityResolver>();
        }

        #region SendContactListEmails() tests

        [Fact]
        public void SendContactListEmails_ForContactList_GetsEmailTemplateAndResolversAreCreated()
        {
            // arrange
            var contactList = CreateContactList();
            SetupSendContactListEmails();
            var service = CreateService();

            // act
            service.SendContactListEmails(contactList);

            // assert
            emailTemplateResolverMock.Verify(e => e.GetEmailTemplateByEmailTemplateEntityId(
                It.Is<EmailTemplateEntityId>(x => x.Id == 5 && x.TypeId == EntityTypeEnum.MainContactList),
                EmailTemplateIncludes.None),
                Times.Once);
            contactParameterResolverFactoryMock.Verify(e => e.CreateFormerStudentParameterResolver(), Times.Once);
            emailTextResolverFactoryMock.Verify(e => e.CreateEmailTextResolver(It.Is<IEmailParameterResolverWithBinding<FormerStudent>>(x => x != null)), Times.Once);
        }

        [Fact]
        public void SendContactListEmails_ContactList_ReturnsEmailIds()
        {
            // arrange
            var contactList = CreateContactList();
            SetupSendContactListEmails();
            var service = CreateService();

            // act
            var result = service.SendContactListEmails(contactList);

            // assert
            Assert.Equal(2, result.Count);
            VerifyHelper(5, SenderName, SenderEmail, "recipientOne@email.com");
            VerifyHelper(5, SenderName, SenderEmail, "recipientTwo@email.com");
        }

        [Fact]
        public void SendContactListEmails_ContactListWithSenderId_PersonIsUsedAsSender()
        {
            // arrange 
            var contactList = CreateContactList();
            contactList.SenderId = 555;
            personDbMock.Setup(e => e.GetPersonById(It.IsAny<long>(), It.IsAny<PersonIncludes>())).Returns(new Person
            {
                Email = "person@email.com",
                Address = new Address
                {
                    FirstName = "John",
                    LastName = "Doe"
                }
            });
            SetupSendContactListEmails();
            var service = CreateService();

            // act
            service.SendContactListEmails(contactList);
            VerifyHelper(5, "John Doe", "person@email.com", "recipientOne@email.com");
            VerifyHelper(5, "John Doe", "person@email.com", "recipientTwo@email.com");
        }

        [Fact]
        public void SendContactListEmails_ContactList_EmailTemplateIsSealed()
        {
            // arrange
            var contactList = CreateContactList();
            SetupSendContactListEmails();
            var service = CreateService();

            // act
            service.SendContactListEmails(contactList);

            // assert
            emailDbMock.Verify(e => e.SetEmailTemplatesToSealed(new [] { EmailTemplateId }, EmailOperationOption.None), Times.Once);
        }

        #endregion

        #region private methods

        private ContactEmailService CreateService()
        {
            return new ContactEmailService(
                emailDbMock.Object,
                personDbMock.Object,
                contactParameterResolverFactoryMock.Object,
                emailTemplateResolverMock.Object,
                emailTextResolverFactoryMock.Object,
                helperMock.Object,
                identityResolverMock.Object);
        }

        private void SetupIdentity()
        {
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Email, SenderEmail));
            identity.AddClaim(new Claim(ClaimTypes.Name, SenderName));
            identityResolverMock.Setup(e => e.GetCurrentIdentity()).Returns(identity);
        }

        private ContactList CreateContactList()
        {
            return new ContactList
            {
                ContactListId = 5,
                ContactListItems = new List<ContactListItem>
                {
                    new ContactListItem
                    {
                        FormerStudent = new FormerStudent(),
                        Email = "recipientOne@email.com"
                    },
                    new ContactListItem
                    {
                        FormerStudent = new FormerStudent(),
                        Email = "recipientTwo@email.com"
                    }
                }
            };
        }

        private void SetupSendContactListEmails()
        {
            var formerStudentParameterResolver = new Mock<IEmailParameterResolverWithBinding<FormerStudent>>().Object;
            var textResolver = new Mock<IEmailTextResolver>().Object;
            SetupIdentity();
            emailTemplateResolverMock
                .Setup(e => e.GetEmailTemplateByEmailTemplateEntityId(
                    It.IsAny<EmailTemplateEntityId>(),
                    It.IsAny<EmailTemplateIncludes>()))
                .Returns(new EmailTemplate { EmailTemplateId = 123 });
            contactParameterResolverFactoryMock.Setup(e => e.CreateFormerStudentParameterResolver()).Returns(formerStudentParameterResolver);
            emailTextResolverFactoryMock.Setup(e => e.CreateEmailTextResolver(It.IsAny<IEmailParameterResolver>())).Returns(textResolver);
            helperMock.Setup(e => e.SendEmailForTemplate(
                    It.IsAny<EmailTemplate>(),
                    It.IsAny<IEmailTextResolver>(),
                    It.IsAny<EmailEntityId>(),
                    It.IsAny<string>(),
                    It.IsAny<SenderInfo>(),
                    It.IsAny<int>(),
                    It.IsAny<IEmailAttachmentProvider>()))
                .Returns(10);
        }

        private void VerifyHelper(long contactListId, string senderName, string senderEmail, string recipientEmail)
        {
            helperMock.Verify(e => e.SendEmailForTemplate(
                It.IsAny<EmailTemplate>(),
                It.Is<IEmailTextResolver>(x => x != null),
                It.Is<EmailEntityId>(x => x.Id == contactListId && x.TypeId == EntityTypeEnum.MainContactList),
                recipientEmail,
                It.Is<SenderInfo>(x => x.SenderName == senderName && x.SenderEmail == senderEmail),
                (int)SeverityEnum.High,
                null), Times.Once);
        }

        #endregion
    }
}
