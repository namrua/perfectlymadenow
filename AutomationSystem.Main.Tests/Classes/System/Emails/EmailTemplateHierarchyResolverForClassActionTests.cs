using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System.Emails;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Classes.System.Emails
{
    public class EmailTemplateHierarchyResolverForClassActionTests
    {
        private const long ProfileId = 1;
        private const long ClassActionId = 10;

        private readonly Mock<IClassDatabaseLayer> classDbMock;

        public EmailTemplateHierarchyResolverForClassActionTests()
        {
            classDbMock = new Mock<IClassDatabaseLayer>();
            classDbMock.Setup(e => e.GetClassActionById(It.IsAny<long>(), It.IsAny<ClassActionIncludes>()))
                .Returns(new ClassAction
                {
                    Class = new Class
                    {
                        ProfileId = ProfileId
                    }
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
            Assert.Equal(EntityTypeEnum.MainClassAction, entityTypeId);
        }

        #endregion

        #region GetHierarchyForParent() tests

        [Fact]
        public void GetHierarchyForParent_ForRegistrationId_ParentHierarchyIsResolver()
        {
            // arrange
            var resolver = CreateResolver();

            // act
            var hierarchy = resolver.GetHierarchyForParent(ClassActionId);

            // assert
            Assert.False(hierarchy.CanUseDefault);
            Assert.Collection(
                hierarchy.Entities,
                item => Assert.Equal(new EmailTemplateEntityId(), item),
                item => Assert.Equal(new EmailTemplateEntityId(EntityTypeEnum.MainProfile, ProfileId), item));
            classDbMock.Verify(e => e.GetClassActionById(ClassActionId, ClassActionIncludes.Class));
        }

        #endregion

        #region GetHierarchy() tests

        [Fact]
        public void GetHierarchy_ThrowsInvalidOperationException()
        {
            // arrange
            var resolver = CreateResolver();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => resolver.GetHierarchy(ClassActionId));
        }

        #endregion

        #region private methods

        private EmailTemplateHierarchyResolverForClassAction CreateResolver()
        {
            return new EmailTemplateHierarchyResolverForClassAction(classDbMock.Object);
        }

        #endregion
    }
}
