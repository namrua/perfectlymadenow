using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Core.Emails.System;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using AutomationSystem.Base.Contract.Identities.Models;
using Xunit;

namespace AutomationSystem.Shared.Tests.Emails.System.EmailPermissionResolvers
{
    public class EmailPermissionResolverTests
    {
        private readonly Mock<IIdentityResolver> identityResolverMock;
        private readonly Mock<IEmailPermissionResolverForEntity> permissionResolverMock;

        public EmailPermissionResolverTests()
        {
            identityResolverMock = new Mock<IIdentityResolver>();
            permissionResolverMock = new Mock<IEmailPermissionResolverForEntity>();
            permissionResolverMock.Setup(e => e.SupportedEntityTypeIds).Returns(new List<EntityTypeEnum?> { null, EntityTypeEnum.MainClass });
            permissionResolverMock.Setup(e => e.IsGrantedForEmail(It.IsAny<EmailEntityId>(), It.IsAny<EmailTypeEnum>())).Returns(true);
            permissionResolverMock.Setup(e => e.IsGrantedForEmailTemplate(It.IsAny<EmailTemplateEntityId>(), It.IsAny<EmailTypeEnum>())).Returns(true);

        }

        #region IsGrantedForEmail() tests

        [Fact]
        public void IsGrantedForEmail_NoPermissionResolver_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var entityId = new EmailEntityId(EntityTypeEnum.MainProfile, 1);
            identityResolverMock.Setup(e => e.GetCurrentIdentity()).Returns(new ClaimsIdentity());
            var resolver = CreateResolver();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => resolver.CheckEmailIsGranted(entityId, EmailTypeEnum.ConversationChanged));
        }

        [Fact]
        public void IsGrantedForEmail_EntitleIsGranted_NoExceptionIsThrown()
        {
            // arrange
            var entityId = new EmailEntityId(EntityTypeEnum.MainClass, 2);
            var resolver = CreateResolver();

            // act
            resolver.CheckEmailIsGranted(entityId, EmailTypeEnum.ConversationCanceled);

            // assert
            permissionResolverMock.Verify(e => e.IsGrantedForEmail(entityId, EmailTypeEnum.ConversationCanceled), Times.Once);
        }

        [Fact]
        public void IsGrantedForEmail_EntitleIsNotGranted_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var entityId = new EmailEntityId(EntityTypeEnum.MainClass, 2);
            identityResolverMock.Setup(e => e.GetCurrentIdentity()).Returns(new ClaimsIdentity());
            permissionResolverMock.Setup(e => e.IsGrantedForEmail(It.IsAny<EmailEntityId>(), It.IsAny<EmailTypeEnum>())).Returns(false);
            var resolver = CreateResolver();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => resolver.CheckEmailIsGranted(entityId, EmailTypeEnum.ConversationCanceled));
            permissionResolverMock.Verify(e => e.IsGrantedForEmail(entityId, EmailTypeEnum.ConversationCanceled), Times.Once);
        }

        #endregion

        #region IsGrantedForEmailTemplate() tests

        [Fact]
        public void IsGrantedForEmailTemplate_NoPermissionResolver_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var entityId = new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 1);
            identityResolverMock.Setup(e => e.GetCurrentIdentity()).Returns(new ClaimsIdentity());
            var resolver = CreateResolver();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => resolver.CheckEmailTemplateIsGranted(entityId, EmailTypeEnum.ConversationCanceled));

        }

        #endregion

        #region private methods

        private EmailPermissionResolver CreateResolver()
        {
            return new EmailPermissionResolver(
                new List<IEmailPermissionResolverForEntity> { permissionResolverMock.Object },
                identityResolverMock.Object);
        }

        #endregion
    }
}
