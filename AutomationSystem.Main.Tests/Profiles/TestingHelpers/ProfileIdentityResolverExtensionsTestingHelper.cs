using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using Moq;
using Moq.Language.Flow;

namespace AutomationSystem.Main.Tests.Profiles.TestingHelpers
{
    public static class ProfileIdentityResolverExtensionsTestingHelper
    {

        #region GetGrantedProfilesForEntitle()
        public static ISetup<IIdentityResolver, UserGroupsForEntitle> SetupGetGrantedProfilesForEntitle(this Mock<IIdentityResolver> identityResolverMock)
        {
            return identityResolverMock.Setup(e => e.GetGrantedUserGroupsForEntitle(It.IsAny<Entitle>(), It.IsAny<UserGroupTypeEnum>()));
        }

        public static void VerifyGetGrantedProfilesForEntitle(this Mock<IIdentityResolver> identityResolverMock, Entitle entitle, Times times)
        {
            identityResolverMock.Verify(e => e.GetGrantedUserGroupsForEntitle(entitle, UserGroupTypeEnum.MainProfile), times);
        }

        public static bool CheckProfileFilterByUserGroups(ProfileFilter filter, UserGroupsForEntitle userGroups)
        {
            if (userGroups.IncludeDefaultGroup != filter.IncludeDefaultProfile)
            {
                return false;
            }

            if (filter.ProfileIds == null)
            {
                return userGroups.GrantedUserGroupIds == null;
            }

            if (userGroups.GrantedUserGroupIds == null
                && filter.ProfileIds.Any(x => !userGroups.GrantedUserGroupIds.Contains(x))
                && filter.ProfileIds.Count != userGroups.GrantedUserGroupIds.Count)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region CheckEntitleForProfile()
        public static ISetup<IIdentityResolver> SetupCheckEntitleForProfile(this Mock<IIdentityResolver> identityResolverMock)
        {
            return identityResolverMock.Setup(e => e.CheckEntitleForUserGroup(
                It.IsAny<Entitle>(),
                It.IsAny<UserGroupTypeEnum?>(),
                It.IsAny<long?>(),
                It.IsAny<EntityTypeEnum?>(),
                It.IsAny<long?>(),
                It.IsAny<string>()));
        }

        public static void VerifyCheckEntitleForProfile(this Mock<IIdentityResolver> identityResolverMock, Entitle entitle, long profileId, Times times)
        {
            identityResolverMock.Verify(e => e.CheckEntitleForUserGroup(
                   entitle,
                   UserGroupTypeEnum.MainProfile,
                   profileId,
                   EntityTypeEnum.MainProfile,
                   profileId,
                   It.IsAny<string>()), times);
        }
        #endregion

        #region CheckEntitleForProfileId()
        public static ISetup<IIdentityResolver> SetupCheckEntitleForProfileId(this Mock<IIdentityResolver> identityResolverMock)
        {
            return identityResolverMock.Setup(e => e.CheckEntitleForUserGroup(
                It.IsAny<Entitle>(),
                It.IsAny<UserGroupTypeEnum?>(),
                It.IsAny<long?>(),
                It.IsAny<EntityTypeEnum?>(),
                It.IsAny<long?>(),
                It.IsAny<string>()));
        }

        public static void VerifyCheckEntitleForProfileId(this Mock<IIdentityResolver> identityResolverMock, Entitle entitle, long profileId, Times times)
        {
            identityResolverMock.Verify(e => e.CheckEntitleForUserGroup(
                   entitle,
                   UserGroupTypeEnum.MainProfile,
                   profileId,
                   EntityTypeEnum.MainProfile,
                   profileId,
                   It.IsAny<string>()), times);
        }
        #endregion

        #region ResolveOnlyWWaEmailTypes()

        public static void SetupResolveOnlyWwaEmailTypes(this Mock<IIdentityResolver> identityResolverMock, bool? expectedResult)
        {
            identityResolverMock.Setup(e => e.IsEntitleGrantedForUserGroup(
                Entitle.MainClasses,
                It.IsAny<UserGroupTypeEnum?>(),
                It.IsAny<long?>()))
                .Returns(expectedResult == false);

            identityResolverMock.Setup(e => e.IsEntitleGrantedForUserGroup(
                Entitle.MainDistanceClasses,
                It.IsAny<UserGroupTypeEnum?>(),
                It.IsAny<long?>()))
                .Returns(expectedResult == true);
        }

        public static void VerifyResolverOnlyWwaEmailTypes(
            this Mock<IIdentityResolver> identityResolverMock,
            long? profileId,
            bool? expectedResult,
            Times times)
        {
            identityResolverMock.Verify(e => e.IsEntitleGrantedForUserGroup(Entitle.MainClasses, UserGroupTypeEnum.MainProfile, profileId), times);
            if (expectedResult != false)
            {
                identityResolverMock.Verify(e => e.IsEntitleGrantedForUserGroup(Entitle.MainDistanceClasses, UserGroupTypeEnum.MainProfile, profileId), times);
            }
        }

        #endregion
    }
}
