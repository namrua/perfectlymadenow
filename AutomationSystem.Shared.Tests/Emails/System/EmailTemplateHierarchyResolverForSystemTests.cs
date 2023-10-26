using AutomationSystem.Shared.Core.Emails.System;
using Xunit;

namespace AutomationSystem.Shared.Tests.Emails.System
{
    public class EmailTemplateHierarchyResolverForSystemTests
    {
        #region GetHierarchyForParent() tests

        [Fact]
        public void GetHierarchyForParent_EntityId_ReturnsEmailTemplateHierarchy()
        {
            // arrange
            var resolver = CreateResolver();

            // act
            var result = resolver.GetHierarchyForParent(1);

            // assert
            Assert.True(result.CanUseDefault);
            Assert.Empty(result.Entities);
        }

        #endregion

        #region GetHierarchy() tests

        [Fact]
        public void GetHierarchy_EntityId_ReturnsEmailTemplateHierarchy()
        {
            // arrange
            var resolver = CreateResolver();

            // act
            var result = resolver.GetHierarchy(1);

            // assert
            Assert.False(result.CanUseDefault);
            Assert.Collection(result.Entities,
                item => Assert.True(item.IsNull));
        }

        #endregion

        #region private methods

        private EmailTemplateHierarchyResolverForSystem CreateResolver()
        {
            return new EmailTemplateHierarchyResolverForSystem();
        }
        #endregion
    }
}
