using System;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Core.Emails.System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.System.Models;
using Moq;
using Xunit;

namespace AutomationSystem.Shared.Tests.Emails.System
{
    public class EmailTemplateHierarchyResolverTests
    {
        private readonly List<IEmailTemplateHierarchyResolverForEntity> resolvers = new List<IEmailTemplateHierarchyResolverForEntity>();
        private readonly Mock<IEmailTemplateHierarchyResolverForEntity> entityResolverMock;

        public EmailTemplateHierarchyResolverTests()
        {
            entityResolverMock = new Mock<IEmailTemplateHierarchyResolverForEntity>();
            AddResolver();
        }

        #region GetHierarchyForParent() tests

        [Fact]
        public void GetHierarchyForParent_UnsupportedEntityType_ThrowsInvalidOperationException()
        {
            // arrange
            var resolver = CreateResolver();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => resolver.GetHierarchyForParent(new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 1)));
        }

        [Fact]
        public void GetHierarchyForParent_EmailTemplateEntityId_ReturnsEmailTemplateEntityHierarchy()
        {
            // arrange
            var hierarchy = new EmailTemplateEntityHierarchy();
            entityResolverMock.Setup(e => e.GetHierarchyForParent(It.IsAny<long?>())).Returns(hierarchy);
            var resolver = CreateResolver();
            // act
            var result = resolver.GetHierarchyForParent(new EmailTemplateEntityId());

            // assert
            Assert.Same(hierarchy, result);
            entityResolverMock.Verify(e => e.GetHierarchyForParent(null), Times.Once);
        }

        #endregion

        #region GetHierarchy() tests

        [Fact]
        public void GetHierarchy_UnsupportedEntityType_ThrowsInvalidOperationException()
        {
            // arrange
            var resolver = CreateResolver();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => resolver.GetHierarchy(new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 1)));
        }

        [Fact]
        public void GetHierarchy_EmailTemplateEntityId_ReturnsEmailTemplateEntityHierarchy()
        {
            // arrange
            var hierarchy = new EmailTemplateEntityHierarchy();
            entityResolverMock.Setup(e => e.EntityTypeId).Returns(EntityTypeEnum.MainProfile);
            entityResolverMock.Setup(e => e.GetHierarchy(It.IsAny<long?>())).Returns(hierarchy);
            var resolver = CreateResolver();

            // act
            var result = resolver.GetHierarchy(new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 4));

            // assert
            Assert.Same(hierarchy, result);
            entityResolverMock.Verify(e => e.GetHierarchy(4), Times.Once);
        }

        #endregion

        #region private methods

        private EmailTemplateHierarchyResolver CreateResolver()
        {
            return new EmailTemplateHierarchyResolver(resolvers);
        }

        private void AddResolver()
        {
            resolvers.Add(entityResolverMock.Object);
        }
        #endregion
    }
}
