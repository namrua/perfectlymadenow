using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Core.Emails.System.EmailPermissionResolvers;
using Moq;
using Xunit;

namespace AutomationSystem.Shared.Tests.Emails.System.EmailPermissionResolvers
{
    public class EmailPermissionResolverForAdminTests
    {
        private readonly Mock<IIdentityResolver> identityResolverMock;

        public EmailPermissionResolverForAdminTests()
        {
            identityResolverMock = new Mock<IIdentityResolver>();
        }

        #region SupportedEntityTypeIds tests

        [Fact]
        public void SupportedEntityTypeIds_SupportsGlobalAndCoreEmail()
        {
            // arrange
            var admin = CreateAdmin();

            // act
            var result = admin.SupportedEntityTypeIds;

            // assert
            Assert.Collection(
                result,
                item => Assert.Null(item),
                item => Assert.Equal(EntityTypeEnum.CoreEmail, item));
        }

        #endregion

        #region IsGrantedForEmail() tests

        [Fact]
        public void IsGrantedForEmail_EmailEntityIdAndEmailTypeId_ReturnsTrue()
        {
            // arrange
            identityResolverMock.Setup(e => e.IsEntitleGranted(It.IsAny<Entitle>())).Returns(true);
            var admin = CreateAdmin();

            // act
            var result = admin.IsGrantedForEmail(new EmailEntityId(EntityTypeEnum.MainProfile, 2), EmailTypeEnum.ConversationCanceled);

            // assert
            Assert.True(result);
            identityResolverMock.Verify(e => e.IsEntitleGranted(Entitle.CoreEmailTemplatesIntegration), Times.Once);
        }

        #endregion

        #region IsGrantedForEmailTemplate() tests

        [Fact]
        public void IsGrantedForEmailTemplate_EmailTemplateEntityIdAndEmailTypeId_ReturnsTrue()
        {
            // arrange
            identityResolverMock.Setup(e => e.IsEntitleGranted(It.IsAny<Entitle>())).Returns(true);
            var admin = CreateAdmin();

            // act
            var result = admin.IsGrantedForEmailTemplate(new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 2), EmailTypeEnum.ConversationCanceled);

            // assert
            Assert.True(result);
            identityResolverMock.Verify(e => e.IsEntitleGranted(Entitle.CoreEmailTemplates), Times.Once);
        }

        #endregion

        #region private methods

        private EmailPermissionResolverForAdmin CreateAdmin()
        {
            return new EmailPermissionResolverForAdmin(identityResolverMock.Object);
        }

        #endregion
    }
}
