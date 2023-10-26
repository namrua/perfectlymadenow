using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Shared.Model;
using Moq;
using Moq.Language.Flow;

namespace AutomationSystem.Shared.Tests.TestingHelpers.Services.Identities.Extensions
{
    public static class IdentityResolverExtensionsHelpers
    {
        #region CheckEntitleForPayPalKey()
        public static ISetup<IIdentityResolver> SetupCheckEntitleForPayPalKey(this Mock<IIdentityResolver> identityResolverMock)
        {
            return identityResolverMock.Setup(e => e.CheckEntitleForUserGroup(
                It.IsAny<Entitle>(),
                It.IsAny<UserGroupTypeEnum?>(),
                It.IsAny<long?>(),
                It.IsAny<EntityTypeEnum?>(),
                It.IsAny<long?>(),
                It.IsAny<string>()));
        }

        public static void VerifyCheckEntitleForPayPalKey(this Mock<IIdentityResolver> identityResolverMock, Entitle entitle, PayPalKey key, Times times)
        {
            identityResolverMock.Verify(e => e.CheckEntitleForUserGroup(
                entitle,
                key.UserGroupTypeId,
                key.UserGroupId,
                null,
                key.PayPalKeyId,
                It.IsAny<string>()), times);
        }
        #endregion
    }
}
