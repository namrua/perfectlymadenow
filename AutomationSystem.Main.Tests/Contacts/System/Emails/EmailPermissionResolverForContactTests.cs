using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Core.Contacts.Data;
using AutomationSystem.Main.Core.Contacts.Data.Models;
using AutomationSystem.Main.Core.Contacts.System.Emails;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Tests.Profiles.TestingHelpers;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Contacts.System.Emails
{
    public class EmailPermissionResolverForContactTests
    {
        private readonly Mock<IContactDatabaseLayer> contactDbMock;
        private readonly Mock<IIdentityResolver> identityResolverMock;

        public EmailPermissionResolverForContactTests()
        {
            contactDbMock = new Mock<IContactDatabaseLayer>();
            identityResolverMock = new Mock<IIdentityResolver>();
        }

        #region SupportedEntityTypeIds() tests

        [Fact]
        public void SupportedEntityTypeIds_ReturnsSupportedEntityTypeIds()
        {
            // arrange
            var resolver = CreateResolver();

            // act
            var result = resolver.SupportedEntityTypeIds;

            // assert
            Assert.Collection(result,
                item => Assert.Equal(EntityTypeEnum.MainContactList, item));
        }

        #endregion

        #region IsGrantedForEmail() tests

        [Fact]
        public void IsGrantedForEmail_ContactListIsNotInDb_ReturnsFalse()
        {
            // arrange
            var emailEntityId = new EmailEntityId(EntityTypeEnum.MainProfile, 3);
            contactDbMock.Setup(e => e.GetContactListById(It.IsAny<long>(), It.IsAny<ContactListIncludes>())).Returns((ContactList)null);
            var resolver = CreateResolver();

            // act
            var result = resolver.IsGrantedForEmail(emailEntityId, EmailTypeEnum.ContactNotification);

            // assert
            Assert.False(result);
            contactDbMock.Verify(e => e.GetContactListById(3, ContactListIncludes.None), Times.Once);
        }

        [Fact]
        public void IsGrantedForEmail_NoAccessToContacts_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var emailEntityId = new EmailEntityId(EntityTypeEnum.MainProfile, 3);
            var contactList = new ContactList
            {
                ProfileId = 6
            };
            contactDbMock.Setup(e => e.GetContactListById(It.IsAny<long>(), It.IsAny<ContactListIncludes>())).Returns(contactList);
            identityResolverMock.SetupCheckEntitleForProfileId().Throws(new EntitleAccessDeniedException("", Entitle.MainContacts));
            var resolver = CreateResolver();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => resolver.IsGrantedForEmail(emailEntityId, EmailTypeEnum.ContactNotification));
            identityResolverMock.VerifyCheckEntitleForProfileId(Entitle.MainContacts, 6, Times.Once());
        }

        [Fact]
        public void IsGrantedForEmail_EmailEntityId_ReturnsTrue()
        {
            // arrange
            var emailEntityId = new EmailEntityId(EntityTypeEnum.MainProfile, 3);
            contactDbMock.Setup(e => e.GetContactListById(It.IsAny<long>(), It.IsAny<ContactListIncludes>())).Returns(new ContactList());
            var resolver = CreateResolver();

            // act
            var result = resolver.IsGrantedForEmail(emailEntityId, EmailTypeEnum.ContactNotification);

            // assert
            Assert.True(result);
        }

        #endregion

        #region IsGrantedForEmailTemplate() tests

        [Fact]
        public void IsGrantedForEmailTemplate_EmailTemplateEntityId_ReturnsTrue()
        {
            // arrange
            var emailTemplateEntityId = new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 5);
            contactDbMock.Setup(e => e.GetContactListById(It.IsAny<long>(), It.IsAny<ContactListIncludes>())).Returns(new ContactList());
            var resolver = CreateResolver();

            // act
            var result = resolver.IsGrantedForEmailTemplate(emailTemplateEntityId, EmailTypeEnum.ContactNotification);

            // assert
            Assert.True(result);
        }

        #endregion

        #region private methods

        private EmailPermissionResolverForContact CreateResolver()
        {
            return new EmailPermissionResolverForContact(contactDbMock.Object, identityResolverMock.Object);
        }

        #endregion
    }
}
