using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using Moq;
using Moq.Language.Flow;

namespace AutomationSystem.Main.Tests.Classes.TestingHelpers
{
    public static class ClassIdentityResolverExtensionsTestingHelpers
    {

        private static readonly Dictionary<ClassCategoryEnum, Entitle> classEntitleMap =  new Dictionary<ClassCategoryEnum, Entitle>
        {
            {ClassCategoryEnum.Class, Entitle.MainClasses},
            {ClassCategoryEnum.Lecture, Entitle.MainClasses},
            {ClassCategoryEnum.DistanceClass, Entitle.MainDistanceClasses},
            {ClassCategoryEnum.PrivateMaterialClass, Entitle.MainPrivateMaterialClasses}
        };
       
        public static ISetup<IIdentityResolver, bool> SetupIsEntitleGrantedForClass(this Mock<IIdentityResolver> identityResolverMock)
        {
            return identityResolverMock.Setup(e => e.IsEntitleGrantedForUserGroup(
                It.IsAny<Entitle>(),
                It.IsAny<UserGroupTypeEnum?>(),
                It.IsAny<long?>()));
        }

        public static void VerifyIsEntitleGrantedForClass(
            this Mock<IIdentityResolver> identityResolverMock,
            ClassCategoryEnum classCategoryId,
            long? profileId,
            Times times)
        {
            var entitle = classEntitleMap[classCategoryId];
            identityResolverMock.Verify(e => e.IsEntitleGrantedForUserGroup(entitle, UserGroupTypeEnum.MainProfile, profileId), times);
        }
    }
}
