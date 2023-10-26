using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Core.Emails.System;
using AutomationSystem.Main.Core.Profiles.AppLogic;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Tests.Profiles.TestingHelpers;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using Moq;
using System;
using System.Collections.Generic;
using AutomationSystem.Main.Core.Profiles.System.Extensions;
using AutomationSystem.Main.Tests.Classes.TestingHelpers;
using Xunit;

namespace AutomationSystem.Main.Tests.Profiles.AppLogic
{
    public class ProfileEmailAdministrationTests
    {
        private readonly Mock<IIdentityResolver> identityResolverMock;
        private readonly Mock<IProfileDatabaseLayer> profileDbMock;
        private readonly Mock<IEmailTemplateAdministration> emailTemplateAdministrationMock;
        private readonly Mock<IEmailTypeResolver> emailTypeResolverMock;

        public ProfileEmailAdministrationTests()
        {
            identityResolverMock = new Mock<IIdentityResolver>();
            profileDbMock = new Mock<IProfileDatabaseLayer>();
            emailTemplateAdministrationMock = new Mock<IEmailTemplateAdministration>();
            emailTypeResolverMock = new Mock<IEmailTypeResolver>();
        }

        #region GetEmailTypeSummaryByProfileId() tests

        [Fact]
        public void GetEmailTypeSummaryByProfileId_NoProfileInDb_ThrowsArgumentException()
        {
            // arrange
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns((Profile)null);
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.GetEmailTypeSummaryByProfileId(15));
        }

        [Fact]
        public void GetEmailTypeSummaryByProfileId_NoAccessToProfile_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var profile = new Profile
            {
                ProfileId = 3
            };
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            identityResolverMock.SetupCheckEntitleForProfile().Throws(new EntitleAccessDeniedException("", Entitle.MainProfiles));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetEmailTypeSummaryByProfileId(3));
            identityResolverMock.VerifyCheckEntitleForProfile(Entitle.MainProfiles, 3, Times.Once());
        }

        [Fact]
        public void GetEmailTypeSummaryByProfileId_ProfileId_ReturnsEmailTypeSummary()
        {
            // arrange
            var emailTemplateEntityId = new EmailTemplateEntityId(EntityTypeEnum.MainProfile, 5);
            var allowedEmailTypes = new HashSet<EmailTypeEnum>();
            var emailTypeSummary = new EmailTypeSummary
            {
                EmailTemplateEntityId = emailTemplateEntityId,
                Items = new List<EmailTypeSummaryItem>()
            };
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(new Profile());
            emailTypeResolverMock.Setup(e => e.GetEmailTypesForProfile(It.IsAny<bool>())).Returns(allowedEmailTypes);
            identityResolverMock.SetupResolveOnlyWwaEmailTypes(true);
            emailTemplateAdministrationMock
                .Setup(e => e.GetEmailTypeSummary(It.IsAny<EmailTemplateEntityId>(), It.IsAny<HashSet<EmailTypeEnum>>()))
                .Returns(emailTypeSummary);
            var admin = CreateAdmin();

            // act
            var result = admin.GetEmailTypeSummaryByProfileId(5);

            // assert
            Assert.Same(emailTemplateEntityId, result.EmailTemplateEntityId);
            Assert.Same(emailTypeSummary, result);
            emailTypeResolverMock.Verify(e => e.GetEmailTypesForProfile(true), Times.Once);
            emailTemplateAdministrationMock.Verify(e => e.GetEmailTypeSummary(emailTemplateEntityId, allowedEmailTypes), Times.Once);
            identityResolverMock.VerifyResolverOnlyWwaEmailTypes(5, true, Times.Once());

        }
        #endregion

        #region private methods

        private ProfileEmailAdministration CreateAdmin()
        {
            return new ProfileEmailAdministration(
                identityResolverMock.Object,
                profileDbMock.Object,
                emailTemplateAdministrationMock.Object,
                emailTypeResolverMock.Object);
        }
        #endregion
    }
}
