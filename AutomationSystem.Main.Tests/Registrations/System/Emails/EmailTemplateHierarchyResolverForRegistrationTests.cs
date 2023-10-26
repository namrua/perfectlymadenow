using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System.Emails;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Registrations.System.Emails
{
    public class EmailTemplateHierarchyResolverForRegistrationTests
    {
        private const long ProfileId = 1;
        private const long RegistrationId = 10;

        private readonly Mock<IRegistrationDatabaseLayer> registrationDbMock;

        public EmailTemplateHierarchyResolverForRegistrationTests()
        {
            registrationDbMock = new Mock<IRegistrationDatabaseLayer>();
            registrationDbMock.Setup(e => e.GetClassRegistrationById(It.IsAny<long>(), It.IsAny<ClassRegistrationIncludes>()))
                .Returns(new ClassRegistration
                {
                    ProfileId = ProfileId
                });
        }

        #region EntityTypeId tests

        [Fact]
        public void EntityTypeId_IsMainClassRegistration()
        {
            // arrange
            var resolver = CreateResolver();

            // act
            var entityTypeId = resolver.EntityTypeId;

            // assert
            Assert.Equal(EntityTypeEnum.MainClassRegistration, entityTypeId);
        }

        #endregion

        #region GetHierarchyForParent() tests

        [Fact]
        public void GetHierarchyForParent_ForRegistrationId_ParentHierarchyIsResolver()
        {
            // arrange
            var resolver = CreateResolver();

            // act
            var hierarchy = resolver.GetHierarchyForParent(RegistrationId);

            // assert
            Assert.False(hierarchy.CanUseDefault);
            Assert.Collection(
                hierarchy.Entities,
                item => Assert.Equal(new EmailTemplateEntityId(), item),
                item => Assert.Equal(new EmailTemplateEntityId(EntityTypeEnum.MainProfile, ProfileId), item));
            registrationDbMock.Verify(e => e.GetClassRegistrationById(RegistrationId, ClassRegistrationIncludes.None));
        }

        #endregion

        #region GetHierarchy() tests

        [Fact]
        public void GetHierarchy_ThrowsInvalidOperationException()
        {
            // arrange
            var resolver = CreateResolver();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => resolver.GetHierarchy(RegistrationId));
        }

        #endregion

        #region private methods

        private EmailTemplateHierarchyResolverForRegistration CreateResolver()
        {
            return new EmailTemplateHierarchyResolverForRegistration(registrationDbMock.Object);
        }

        #endregion
    }
}
