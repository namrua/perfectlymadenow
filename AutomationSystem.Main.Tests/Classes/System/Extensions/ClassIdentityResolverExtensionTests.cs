using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Main.Model;
using Xunit;

namespace AutomationSystem.Main.Tests.Classes.System.Extensions
{
    public class ClassIdentityResolverExtensionTests
    {
        private readonly Mock<IIdentityResolver> identityResolverMock;
        private const int ProfileId = 1;
        private const int ClassId = 5;

        public ClassIdentityResolverExtensionTests()
        {
            identityResolverMock = new Mock<IIdentityResolver>();
        }

        #region CheckEntitleForClass() tests

        [Theory]
        [InlineData(ClassCategoryEnum.Class, Entitle.MainClasses)]
        [InlineData(ClassCategoryEnum.Lecture, Entitle.MainClasses)]
        [InlineData(ClassCategoryEnum.DistanceClass, Entitle.MainDistanceClasses)]
        [InlineData(ClassCategoryEnum.PrivateMaterialClass, Entitle.MainPrivateMaterialClasses)]
        public void CheckEntitleForClass_NoAccess_ThrowEntitleAccessDeniedException(ClassCategoryEnum classCategoryId, Entitle expectedEntitle)
        {
            // arrange
            var cls = CreateClass(classCategoryId);
            identityResolverMock.Setup(e => 
                e.CheckEntitleForUserGroup(
                    It.IsAny<Entitle>(),
                    It.IsAny<UserGroupTypeEnum?>(),
                    It.IsAny<long?>(),
                    It.IsAny<EntityTypeEnum?>(),
                    It.IsAny<long?>(),
                    It.IsAny<string>()))
                .Throws(new EntitleAccessDeniedException("", expectedEntitle));
            
            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => identityResolverMock.Object.CheckEntitleForClass(cls));
            identityResolverMock.Verify(e => e.CheckEntitleForUserGroup(
                expectedEntitle,
                UserGroupTypeEnum.MainProfile,
                1,
                EntityTypeEnum.MainClass,
                5,
                null), Times.Once);
        }

        #endregion

        #region CheckEntitleForProfileIdAndClassCategory() tests

        [Theory]
        [InlineData(ClassCategoryEnum.Class, Entitle.MainClasses)]
        [InlineData(ClassCategoryEnum.Lecture, Entitle.MainClasses)]
        [InlineData(ClassCategoryEnum.DistanceClass, Entitle.MainDistanceClasses)]
        [InlineData(ClassCategoryEnum.PrivateMaterialClass, Entitle.MainPrivateMaterialClasses)]
        public void CheckEntitleForProfileIdAndClassCategory_NoAccess_ThrowsEntitleAccessDeniedException(ClassCategoryEnum classCategoryId, Entitle expectedEntitle)
        {
            // arrange
            identityResolverMock.Setup(e =>
                    e.CheckEntitleForUserGroup(
                        It.IsAny<Entitle>(),
                        It.IsAny<UserGroupTypeEnum?>(),
                        It.IsAny<long?>(),
                        It.IsAny<EntityTypeEnum?>(),
                        It.IsAny<long?>(),
                        It.IsAny<string>()))
                .Throws(new EntitleAccessDeniedException("", expectedEntitle));

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => identityResolverMock.Object.CheckEntitleForProfileIdAndClassCategory(ProfileId, classCategoryId));
            identityResolverMock.Verify(e => e.CheckEntitleForUserGroup(
                expectedEntitle,
                UserGroupTypeEnum.MainProfile,
                ProfileId,
                EntityTypeEnum.MainProfile,
                ProfileId,
                null), Times.Once);
        }

        #endregion

        #region IsEntitleGrantedForClass() tests

        [Theory]
        [InlineData(true, ClassCategoryEnum.Class, Entitle.MainClasses)]
        [InlineData(false, ClassCategoryEnum.Class, Entitle.MainClasses)]
        [InlineData(true, ClassCategoryEnum.Lecture, Entitle.MainClasses)]
        [InlineData(false, ClassCategoryEnum.Lecture, Entitle.MainClasses)]
        [InlineData(true, ClassCategoryEnum.DistanceClass, Entitle.MainDistanceClasses)]
        [InlineData(false, ClassCategoryEnum.DistanceClass, Entitle.MainDistanceClasses)]
        [InlineData(true, ClassCategoryEnum.PrivateMaterialClass, Entitle.MainPrivateMaterialClasses)]
        [InlineData(false, ClassCategoryEnum.PrivateMaterialClass, Entitle.MainPrivateMaterialClasses)]
        public void IsEntitleGrantedForClass_ForGivenParameters_ReturnsExpectedResult(bool isGranted, ClassCategoryEnum classCategoryId, Entitle expectedEntitle)
        {
            // arrange
            var cls = CreateClass(classCategoryId);
            identityResolverMock.Setup(e => e.IsEntitleGrantedForUserGroup(It.IsAny<Entitle>(), It.IsAny<UserGroupTypeEnum?>(), It.IsAny<long?>())).Returns(isGranted);
            
            // act
            var actualResult = identityResolverMock.Object.IsEntitleGrantedForClass(cls);

            // assert
            Assert.Equal(isGranted, actualResult);
            identityResolverMock.Verify(e => e.IsEntitleGrantedForUserGroup(expectedEntitle, UserGroupTypeEnum.MainProfile, ProfileId), Times.Once);
        }

        #endregion

        #region GetGrantedClassCategories() tests

        [Theory]
        [MemberData(nameof(GetGrantedClassCategoriesTestingData))]
        public void GetGrantedClassCategories_SetupGrantedEntitles_ReturnsClassCategoryEnums(List<Entitle> entitles, List<ClassCategoryEnum> expectedClassCategories)
        {
            // arrange
            foreach (var entitle in entitles)
            {
                identityResolverMock.Setup(e => e.IsEntitleGranted(entitle)).Returns(true);
            }

            // act
            var result = identityResolverMock.Object.GetGrantedClassCategories();

            // assert
            Assert.Equal(expectedClassCategories, result.ToList());
        }

        public static IEnumerable<object[]> GetGrantedClassCategoriesTestingData =>
            new List<object[]>
            {
                new object[]
                {
                    new List<Entitle>(), new List<ClassCategoryEnum>()
                },
                new object[]
                {
                    new List<Entitle>
                    {
                        Entitle.MainClasses, Entitle.MainDistanceClasses, Entitle.MainPrivateMaterialClasses
                    },
                    new List<ClassCategoryEnum>
                    {
                        ClassCategoryEnum.Class, ClassCategoryEnum.Lecture, ClassCategoryEnum.DistanceClass,
                        ClassCategoryEnum.PrivateMaterialClass
                    }
                },
                new object[]
                {
                    new List<Entitle>
                    {
                        Entitle.MainClasses
                    },
                    new List<ClassCategoryEnum>
                    {
                        ClassCategoryEnum.Class, ClassCategoryEnum.Lecture
                    }

                },
                new object[]
                {
                    new List<Entitle>
                    {
                        Entitle.MainDistanceClasses
                    },
                    new List<ClassCategoryEnum>
                    {
                        ClassCategoryEnum.DistanceClass
                    }

                },
                new object[]
                {
                    new List<Entitle>
                    {
                        Entitle.MainPrivateMaterialClasses
                    },
                    new List<ClassCategoryEnum>
                    {
                        ClassCategoryEnum.PrivateMaterialClass
                    }

                }
            };


        #endregion

        #region GetProfileIdsForClasses() tests

        [Theory]
        [InlineData(Entitle.MainClasses)]
        [InlineData(Entitle.MainDistanceClasses)]
        [InlineData(Entitle.MainPrivateMaterialClasses)]
        public void GetProfileIdsForClasses_OneEntitleHasPartialAccess_PartialAccessToProfileIsReturned(Entitle entitleWithPartialAccess)
        {
            // arrange
            var noAccessUserGroup = CreateUserGroupsForEntitle(new HashSet<long>(), Entitle.MainClasses);
            var partialAccessUserGroup = CreateUserGroupsForEntitle(new HashSet<long>{1, 2}, entitleWithPartialAccess);
            identityResolverMock.Setup(e => e.GetGrantedUserGroupsForEntitle(It.IsAny<Entitle>(), It.IsAny<UserGroupTypeEnum>())).Returns(noAccessUserGroup);
            identityResolverMock.Setup(e => e.GetGrantedUserGroupsForEntitle(entitleWithPartialAccess, It.IsAny<UserGroupTypeEnum>())).Returns(partialAccessUserGroup);

            // act
            var result = identityResolverMock.Object.GetProfileIdsForClasses();

            // assert
            Assert.Collection(result,
                item => Assert.Equal(1, item),
                item => Assert.Equal(2, item));
        }

        [Theory]
        [InlineData(Entitle.MainClasses)]
        [InlineData(Entitle.MainDistanceClasses)]
        [InlineData(Entitle.MainPrivateMaterialClasses)]
        public void GetProfileIdsForClasses_OneEntitleWithFullAccessOthersWithPartial_ReturnsNull(Entitle entitleWithPartialAccess)
        {
            // arrange
            var partialAccessUserGroup = CreateUserGroupsForEntitle(new HashSet<long> {1, 2}, entitleWithPartialAccess);
            var fullAccessUserGroup = CreateUserGroupsForEntitle(null, Entitle.MainDistanceClasses);
            identityResolverMock.Setup(e => e.GetGrantedUserGroupsForEntitle(It.IsAny<Entitle>(), It.IsAny<UserGroupTypeEnum>())).Returns(partialAccessUserGroup);
            identityResolverMock.Setup(e => e.GetGrantedUserGroupsForEntitle(Entitle.MainDistanceClasses, It.IsAny<UserGroupTypeEnum>())).Returns(fullAccessUserGroup);

            // act
            var result = identityResolverMock.Object.GetProfileIdsForClasses();

            // assert
            Assert.Null(result);
        }

        [Fact]
        public void GetProfileIdsForClasses_EntitlesWithPartialAccess_ReturnsUnionProfileIds()
        {
            // arrange
            var clsUserGroup = CreateUserGroupsForEntitle(new HashSet<long> {1, 2}, Entitle.MainClasses);
            var distanceUserGroup = CreateUserGroupsForEntitle(new HashSet<long> {2, 3}, Entitle.MainDistanceClasses);
            var materialUserGroup = CreateUserGroupsForEntitle(new HashSet<long>{3, 4}, Entitle.MainPrivateMaterialClasses);
            identityResolverMock.Setup(e => e.GetGrantedUserGroupsForEntitle(Entitle.MainClasses, It.IsAny<UserGroupTypeEnum>())).Returns(clsUserGroup);
            identityResolverMock.Setup(e => e.GetGrantedUserGroupsForEntitle(Entitle.MainDistanceClasses, It.IsAny<UserGroupTypeEnum>())).Returns(distanceUserGroup);
            identityResolverMock.Setup(e => e.GetGrantedUserGroupsForEntitle(Entitle.MainPrivateMaterialClasses, It.IsAny<UserGroupTypeEnum>())).Returns(materialUserGroup);

            // act
            var result = identityResolverMock.Object.GetProfileIdsForClasses();

            // assert
            Assert.Collection(result,
                item => Assert.Equal(1, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(3, item),
                item => Assert.Equal(4, item));
        }

        #endregion

        #region private methods

        private Class CreateClass(ClassCategoryEnum classCategoryId)
        {
            return new Class
            {
                ClassId = ClassId,
                ProfileId = ProfileId,
                ClassCategoryId = classCategoryId
            };
        }

        private UserGroupsForEntitle CreateUserGroupsForEntitle(HashSet<long> userGroupIds, Entitle entitle)
        {
            return new UserGroupsForEntitle(entitle, UserGroupTypeEnum.MainProfile)
            {
                GrantedUserGroupIds = userGroupIds
            };
        }

        #endregion
    }
}
