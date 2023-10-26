using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Profiles.System;
using AutomationSystem.Main.Core.Profiles.System.Emails;
using Xunit;

namespace AutomationSystem.Main.Tests.Profiles.System
{
    public class EmailTemplateHierarchyResolverForProfileTests
    {
        #region GetHierarchyForParent

        [Fact]
        public void GetHierarchyForParent_EntityId_ReturnsEmailTemplateEntityHierarchy()
        {
            // arrange
            var resolver = CreateResolver();

            // act
            var result = resolver.GetHierarchyForParent(1);

            // assert
            Assert.Collection(result.Entities,
                item => Assert.True(item.IsNull));
            Assert.True(result.CanUseDefault);
        }

        #endregion

        #region GetHierarchy() tests

        [Fact]
        public void GetHierarchy_EntityId_ReturnsEmailTemplateEntityHierarchy()
        {
            // arrange
            var resolver = CreateResolver();

            // act
            var result = resolver.GetHierarchy(2);

            // assert
            Assert.False(result.CanUseDefault);
            Assert.Collection(result.Entities,
                item => Assert.True(item.IsNull),
                item =>
                {
                    Assert.Equal(2, item.Id);
                    Assert.Equal(EntityTypeEnum.MainProfile, item.TypeId);
                });
        }

        #endregion

        #region private methods

        private EmailTemplateHierarchyResolverForProfile CreateResolver()
        {
            return new EmailTemplateHierarchyResolverForProfile();
        }

        #endregion
    }
}
