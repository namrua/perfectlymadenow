using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System.Emails;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Tests.Classes.TestingHelpers;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Registrations.System.Emails
{
    public class EmailPermissionResolverForRegistrationTests
    {
        private readonly Mock<IRegistrationDatabaseLayer> registrationDbMock;
        private readonly Mock<IIdentityResolver> identityResolverMock;

        public EmailPermissionResolverForRegistrationTests()
        {
            registrationDbMock = new Mock<IRegistrationDatabaseLayer>();
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
                item => Assert.Equal(EntityTypeEnum.MainClassRegistration, item),
                item => Assert.Equal(EntityTypeEnum.MainClassRegistrationInvitation, item));
        }

        #endregion

        #region IsGrantedForEmail() tests

        [Fact]
        public void IsGrantedForEmail_UnknownEntityType_ReturnsFalse()
        {
            // arrange
            var emailEntityId = new EmailEntityId(EntityTypeEnum.MainProfile, 1);
            var resolver = CreateResolver();

            // act
            var result = resolver.IsGrantedForEmail(emailEntityId, EmailTypeEnum.ConversationCanceled);

            // assert
            Assert.False(result);
            identityResolverMock.VerifyIsEntitleGrantedForClass(ClassCategoryEnum.Class, It.IsAny<long>(), Times.Never());
        }


        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void IsGrantedForEmail_IsClassRegistrationGranted_ReturnsExpectedResult(bool isClassRegistrationGranted, bool expectedResult)
        {
            // arrange
            var classRegistration = new ClassRegistration
            {
                Class = CreateClass()
            };
            var emailEntityId = new EmailEntityId(EntityTypeEnum.MainClassRegistration, 3);
            registrationDbMock.Setup(e => e.GetClassRegistrationById(It.IsAny<long>(), It.IsAny<ClassRegistrationIncludes>())).Returns(classRegistration);
            identityResolverMock.SetupIsEntitleGrantedForClass().Returns(isClassRegistrationGranted);
            var resolver = CreateResolver();

            // act
            var result = resolver.IsGrantedForEmail(emailEntityId, EmailTypeEnum.ConversationCanceled);

            // assert
            Assert.Equal(expectedResult, result);
            registrationDbMock.Verify(e => e.GetClassRegistrationById(3, ClassRegistrationIncludes.Class), Times.Once);
            identityResolverMock.VerifyIsEntitleGrantedForClass(ClassCategoryEnum.Class, 3, Times.Once());
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void IsGrantedForEmail_IsClassRegistrationInvitationGranted_ReturnsExpectedResult(bool isClassRegistrationInvitationGranted, bool expectedResult)
        {
            // arrange
            var invitation = new ClassRegistrationInvitation
            {
                Class = CreateClass()
            };
            var emailEntityId = new EmailEntityId(EntityTypeEnum.MainClassRegistrationInvitation, 3);
            registrationDbMock.Setup(e => e.GetClassRegistrationInvitationById(It.IsAny<long>(), It.IsAny<ClassRegistrationInvitationIncludes>())).Returns(invitation);
            identityResolverMock.SetupIsEntitleGrantedForClass().Returns(isClassRegistrationInvitationGranted);
            var resolver = CreateResolver();

            // act
            var result = resolver.IsGrantedForEmail(emailEntityId, EmailTypeEnum.ConversationCanceled);

            // assert
            Assert.Equal(expectedResult, result);
            registrationDbMock.Verify(e => e.GetClassRegistrationInvitationById(3, ClassRegistrationInvitationIncludes.Class), Times.Once);
            identityResolverMock.VerifyIsEntitleGrantedForClass(ClassCategoryEnum.Class, 3, Times.Once());
        }

        #endregion

        #region IsGrantedForEmailTemplate() tests

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void IsGrantedForEmailTemplate_IsClassRegistrationInvitationGranted_ReturnsExpectedResult(bool isClassRegistrationInvitationGranted, bool expectedResult)
        {
            // arrange
            var invitation = new ClassRegistrationInvitation
            {
                Class = CreateClass()
            };
            var emailTemplateEntityId = new EmailTemplateEntityId(EntityTypeEnum.MainClassRegistrationInvitation, 3);
            registrationDbMock.Setup(e => e.GetClassRegistrationInvitationById(It.IsAny<long>(), It.IsAny<ClassRegistrationInvitationIncludes>())).Returns(invitation);
            identityResolverMock.SetupIsEntitleGrantedForClass().Returns(isClassRegistrationInvitationGranted);
            var resolver = CreateResolver();

            // act
            var result = resolver.IsGrantedForEmailTemplate(emailTemplateEntityId, EmailTypeEnum.ConversationCanceled);

            // assert
            Assert.Equal(expectedResult, result);
            registrationDbMock.Verify(e => e.GetClassRegistrationInvitationById(3, ClassRegistrationInvitationIncludes.Class), Times.Once);
            identityResolverMock.VerifyIsEntitleGrantedForClass(ClassCategoryEnum.Class, 3, Times.Once());
        }

        #endregion

        #region private methods

        private EmailPermissionResolverForRegistration CreateResolver()
        {
            return new EmailPermissionResolverForRegistration(registrationDbMock.Object, identityResolverMock.Object);
        }

        private Class CreateClass()
        {
            return new Class
            {
                ProfileId = 3,
                ClassCategoryId = ClassCategoryEnum.Class
            };
        }

        #endregion
    }
}
