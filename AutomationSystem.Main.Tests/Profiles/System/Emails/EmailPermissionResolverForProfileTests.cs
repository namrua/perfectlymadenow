using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Core.Emails.System;
using AutomationSystem.Main.Core.Profiles.System.Emails;
using AutomationSystem.Main.Tests.Profiles.TestingHelpers;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Profiles.System.Emails
{
    public class EmailPermissionResolverForProfileTests
    {
        private readonly Mock<IEmailTypeResolver> emailTypeResolverMock;
        private readonly Mock<IIdentityResolver> identityResolverMock;

        public EmailPermissionResolverForProfileTests()
        {
            emailTypeResolverMock = new Mock<IEmailTypeResolver>();
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
                item => Assert.Equal(EntityTypeEnum.MainProfile, item));
        }

        #endregion

        #region IsGrantedForEmail() tests

        [Fact]
        public void IsGrantedForEmail_ResolverOnlyWwaTypesIsNull_ReturnsFalse()
        {
            // arrange
            identityResolverMock.SetupResolveOnlyWwaEmailTypes(null);
            var resolver = CreateResolver();

            // act
            var result = resolver.IsGrantedForEmail(new EmailEntityId(EntityTypeEnum.MainProfile, 1), EmailTypeEnum.ConversationCanceled);
            
            // assert
            Assert.False(result);
            identityResolverMock.VerifyResolverOnlyWwaEmailTypes(1, null, Times.Once());
        }

        [Fact]
        public void IsGrantedForEmail_EmailTypesContainsEmailTypeId_ReturnsTrue()
        {
            // arrange
            var onlyWwa = true;
            var emailTypes = new HashSet<EmailTypeEnum>{ EmailTypeEnum.WwaConversationChanged };
            identityResolverMock.SetupResolveOnlyWwaEmailTypes(onlyWwa);
            emailTypeResolverMock.Setup(e => e.GetEmailTypesForProfile(It.IsAny<bool>())).Returns(emailTypes);
            var resolver = CreateResolver();

            // act
            var result = resolver.IsGrantedForEmail(new EmailEntityId(EntityTypeEnum.MainProfile, 3), EmailTypeEnum.WwaConversationChanged);

            // assert
            Assert.True(result);
            identityResolverMock.VerifyResolverOnlyWwaEmailTypes(3, true, Times.Once());
            emailTypeResolverMock.Verify(e => e.GetEmailTypesForProfile(onlyWwa), Times.Once);
        }
        
        [Fact]
        public void IsGrantedForEmail_ResolveOnlyWwaTypesIsTrueAndEmailTypesNotContainsEmailTypeId_ReturnsTrue()
        {
            // arrange
            var onlyWwa = false;
            var emailTypes = new HashSet<EmailTypeEnum> { EmailTypeEnum.WwaConversationCanceled, EmailTypeEnum.WwaConversationChanged };
            identityResolverMock.SetupResolveOnlyWwaEmailTypes(onlyWwa);
            emailTypeResolverMock.Setup(e => e.GetEmailTypesForProfile(It.IsAny<bool>())).Returns(emailTypes);
            var resolver = CreateResolver();

            // act
            var result = resolver.IsGrantedForEmail(new EmailEntityId(EntityTypeEnum.MainProfile, 3), EmailTypeEnum.RegistrationInvitation);

            // assert
            Assert.False(result);
            emailTypeResolverMock.Verify(e => e.GetEmailTypesForProfile(onlyWwa), Times.Once);
        }

        #endregion

        #region IsGrantedForEmailTemplate() tests

        [Fact]
        public void IsGrantedForEmailTemplate_EmailTypesContainsEmailTypeId_ReturnsTrue()
        {
            // arrange
            var onlyWwa = true;
            identityResolverMock.SetupResolveOnlyWwaEmailTypes(onlyWwa);
            var emailTypes = new HashSet<EmailTypeEnum> { EmailTypeEnum.WwaConversationCanceled, EmailTypeEnum.WwaConversationChanged };
            emailTypeResolverMock.Setup(e => e.GetEmailTypesForProfile(It.IsAny<bool>())).Returns(emailTypes);
            var resolver = CreateResolver();

            // act
            var result = resolver.IsGrantedForEmailTemplate(new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 4), EmailTypeEnum.WwaConversationCanceled);

            // assert
            Assert.True(result);
            identityResolverMock.VerifyResolverOnlyWwaEmailTypes(4, true, Times.Once());
            emailTypeResolverMock.Verify(e => e.GetEmailTypesForProfile(onlyWwa), Times.Once);
        }

        #endregion

        #region private methods

        private EmailPermissionResolverForProfile CreateResolver()
        {
            return new EmailPermissionResolverForProfile(emailTypeResolverMock.Object, identityResolverMock.Object);
        }

        #endregion
    }
}
