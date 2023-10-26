using AutomationSystem.Main.Core.Emails.System;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Emails.System
{
    public class EmailTypeResolverTests
    {
        private readonly Mock<IRegistrationTypeResolver> registrationTypeResolverMock;

        public EmailTypeResolverTests()
        {
            registrationTypeResolverMock = new Mock<IRegistrationTypeResolver>();
        }

        #region GetEmailTypesForProfile() tests

        [Theory]
        [InlineData(true, 6)]
        [InlineData(false, 15)]
        public void GetEmailTypesForProfile_OnlyWwaEmailTypesIsSet_ExpectedResult(bool onlyWwaEmailTypes, long expectedEmailTypeCount)
        {
            // arrange
            var resolver = CreateResolver();

            // act
            var result = resolver.GetEmailTypesForProfile(onlyWwaEmailTypes);

            // assert
            Assert.Equal(expectedEmailTypeCount, result.Count);
        }
        #endregion

        #region private methods

        private EmailTypeResolver CreateResolver()
        {
            return new EmailTypeResolver(registrationTypeResolverMock.Object);
        }
        #endregion
    }
}
