using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System.Emails;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Tests.Classes.TestingHelpers;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Classes.System.Emails
{
    public class EmailPermissionResolverForClassTests
    {
        private readonly Mock<IClassDatabaseLayer> classDbMock;
        private readonly Mock<IIdentityResolver> identityResolverMock;

        public EmailPermissionResolverForClassTests()
        {
            classDbMock = new Mock<IClassDatabaseLayer>();
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
                item => Assert.Equal(EntityTypeEnum.MainClass, item),
                item => Assert.Equal(EntityTypeEnum.MainClassAction, item));
        }

        #endregion

        #region IsGrantedForEmail() tests

        [Fact]
        public void IsGrantedForEmail_UnknownEntityType_ReturnsFalse()
        {
            // arrange
            var emailEntityId = new EmailEntityId(EntityTypeEnum.MainProfile, 12);
            var resolver = CreateResolver();

            // act
            var result = resolver.IsGrantedForEmail(emailEntityId, EmailTypeEnum.ConversationCanceled);

            // assert
            Assert.False(result);
            identityResolverMock.VerifyIsEntitleGrantedForClass(ClassCategoryEnum.Class, 12, Times.Never());
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void IsGrantedForEmail_IsClassGranted_ReturnsExpectedResult(bool isClassGranted, bool expectedResult)
        {
            // arrange
            var cls = CreateClass(4);
            var emailEntityId = new EmailEntityId(EntityTypeEnum.MainClass, 4);
            classDbMock.Setup(e => e.GetClassById(It.IsAny<long>(), It.IsAny<ClassIncludes>())).Returns(cls);
            identityResolverMock.SetupIsEntitleGrantedForClass().Returns(isClassGranted);
            var resolver = CreateResolver();

            // act
            var result = resolver.IsGrantedForEmail(emailEntityId, EmailTypeEnum.ConversationCanceled);

            // assert
            Assert.Equal(expectedResult, result);
            classDbMock.Verify(e => e.GetClassById(4, ClassIncludes.None), Times.Once);
            identityResolverMock.VerifyIsEntitleGrantedForClass(ClassCategoryEnum.Class, 4, Times.Once());
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void IsGrantedForEmail_IsClassActionGranted_ReturnsExpectedResult(bool isClassActionGranted, bool expectedResult)
        {
            // arrange
            var classAction = new ClassAction
            {
                Class = CreateClass(4)
            };
            var emailEntityId = new EmailEntityId(EntityTypeEnum.MainClassAction, 4);
            classDbMock.Setup(e => e.GetClassActionById(It.IsAny<long>(), It.IsAny<ClassActionIncludes>())).Returns(classAction);
            identityResolverMock.SetupIsEntitleGrantedForClass().Returns(isClassActionGranted);
            var resolver = CreateResolver();

            // act
            var result = resolver.IsGrantedForEmail(emailEntityId, EmailTypeEnum.ConversationCanceled);

            // assert
            Assert.Equal(expectedResult, result);
            classDbMock.Verify(e => e.GetClassActionById(4, ClassActionIncludes.Class), Times.Once);
            identityResolverMock.VerifyIsEntitleGrantedForClass(ClassCategoryEnum.Class, 4, Times.Once());
        }


        #endregion

        #region IsGrantedForEmailTemplate() tests

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void IsGrantedForEmailTemplate_IsClassGranted_ReturnsExpectedResult(bool isClassGranted, bool expectedResult)
        {
            // arrange
            var emailTemplateEntityId = new EmailTemplateEntityId(EntityTypeEnum.MainClass, 1);
            var cls = CreateClass(1);
            classDbMock.Setup(e => e.GetClassById(It.IsAny<long>(), It.IsAny<ClassIncludes>())).Returns(cls);
            identityResolverMock.SetupIsEntitleGrantedForClass().Returns(isClassGranted);
            var resolver = CreateResolver();

            // act
            var result = resolver.IsGrantedForEmailTemplate(emailTemplateEntityId, EmailTypeEnum.ConversationCanceled);

            // assert
            Assert.Equal(expectedResult, result);
            classDbMock.Verify(e => e.GetClassById(1, ClassIncludes.None), Times.Once);
            identityResolverMock.VerifyIsEntitleGrantedForClass(ClassCategoryEnum.Class, 1, Times.Once());
        }

        #endregion

        #region private methods

        private EmailPermissionResolverForClass CreateResolver()
        {
            return new EmailPermissionResolverForClass(classDbMock.Object, identityResolverMock.Object);
        }

        private Class CreateClass(long profileId)
        {
            return new Class
            {
                ProfileId = profileId,
                ClassCategoryId = ClassCategoryEnum.Class
            };
        }

        #endregion
    }
}
