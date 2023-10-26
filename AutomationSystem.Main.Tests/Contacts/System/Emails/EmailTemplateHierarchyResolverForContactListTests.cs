using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Contacts.System.Emails;
using System;
using Xunit;

namespace AutomationSystem.Main.Tests.Contacts.System.Emails
{
    public class EmailTemplateHierarchyResolverForContactListTests
    {
        #region EntityTypeId() tests

        [Fact]
        public void EntityTypeId_IsMainContactList()
        {
            // arrange
            var resolver = CreateResolver();

            // act
            var entityTypeId = resolver.EntityTypeId;

            // assert
            Assert.Equal(EntityTypeEnum.MainContactList, entityTypeId);
        }

        #endregion

        #region GetHierachy() tests

        [Fact]
        public void GetHierarchy_EntityId_ThrowsInvalidOperationException()
        {
            // arrange
            var resolver = CreateResolver();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => resolver.GetHierarchy(1));
        }

        #endregion

        #region GetHierarchyForParent() tests

        [Fact]
        public void GetHierarchyForParent_EntityId_ReturnsEmailTemplateEntityHierarchy()
        {
            // arrange
            var resolver = CreateResolver();

            // act
            var result = resolver.GetHierarchyForParent(4);

            // assert
            Assert.False(result.CanUseDefault);
            Assert.Collection(result.Entities,
                item => Assert.True(item.IsNull));
        }

        #endregion

        #region private methods

        private EmailTemplateHierarchyResolverForContactList CreateResolver()
        {
            return new EmailTemplateHierarchyResolverForContactList();
        }

        #endregion
    }
}
