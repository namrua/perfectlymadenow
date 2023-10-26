using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Base.Contract.Identities.Models.Events;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.AppLogic.Factories;
using AutomationSystem.Main.Core.Profiles.AppLogic;
using AutomationSystem.Main.Core.Profiles.AppLogic.Models.Events;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Tests.Profiles.TestingHelpers;
using Moq;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using System;
using System.Collections.Generic;
using Xunit;
using Profile = AutomationSystem.Main.Model.Profile;

namespace AutomationSystem.Main.Tests.Profiles.AppLogic
{
    public class ProfileAdministrationTests
    {
        private readonly Mock<IProfileDatabaseLayer> profileDbMock;
        private readonly Mock<IIdentityResolver> identityResolverMock;
        private readonly Mock<IMainMapper> mainMapperMock;
        private readonly Mock<IClassPreferenceFactory> classPreferenceFactoryMock;
        private readonly Mock<IEventDispatcher> eventDispatcherMock;

        public ProfileAdministrationTests()
        {
            profileDbMock = new Mock<IProfileDatabaseLayer>();
            identityResolverMock = new Mock<IIdentityResolver>();
            mainMapperMock = new Mock<IMainMapper>();
            classPreferenceFactoryMock = new Mock<IClassPreferenceFactory>();
            eventDispatcherMock = new Mock<IEventDispatcher>();
        }

        #region GetProfiles() tests
        [Fact]
        public void GetProfiles_ExpectedUserGroupsForEntitle_IsUsedForFiltering()
        {
            // arrange
            var userGroups = new UserGroupsForEntitle(Entitle.MainProfiles, UserGroupTypeEnum.MainProfile)
            {
                IncludeDefaultGroup = false,
                GrantedUserGroupIds = new HashSet<long> { 2, 3 }
            };
            identityResolverMock.SetupGetGrantedProfilesForEntitle().Returns(userGroups);
            profileDbMock.Setup(e => e.GetProfilesByFilter(It.IsAny<ProfileFilter>(), It.IsAny<ProfileIncludes>())).Returns(new List<Profile>());
            var admin = CreateAdministration();

            // act
            admin.GetProfiles();

            // assert
            identityResolverMock.VerifyGetGrantedProfilesForEntitle(Entitle.MainProfiles, Times.Once());
            profileDbMock.Verify(e =>
                e.GetProfilesByFilter(It.Is<ProfileFilter>(x =>
                    ProfileIdentityResolverExtensionsTestingHelper.CheckProfileFilterByUserGroups(x, userGroups)),
                    It.IsAny<ProfileIncludes>()));
        }

        [Fact]
        public void GetProfiles_Profile_IsMappedToProfileListItem()
        {
            // arrange
            var profile = new Profile();
            var listItem = new ProfileListItem();
            identityResolverMock.SetupGetGrantedProfilesForEntitle().Returns(new UserGroupsForEntitle(Entitle.MainProfiles, UserGroupTypeEnum.MainProfile));
            profileDbMock.Setup(e => e.GetProfilesByFilter(It.IsAny<ProfileFilter>(), It.IsAny<ProfileIncludes>())).Returns(new List<Profile> { profile });
            mainMapperMock.Setup(e => e.Map<ProfileListItem>(It.IsAny<Profile>())).Returns(listItem);
            var admin = CreateAdministration();

            // act
            var result = admin.GetProfiles();

            // assert
            Assert.Collection(result, item => Assert.Same(listItem, item));
            mainMapperMock.Verify(e => e.Map<ProfileListItem>(profile), Times.Once);
        }
        #endregion

        #region GetProfileDetail() tests
        [Fact]
        public void GetProfileDetail_ProfileIdPassToGetProfileById()
        {
            // arrange
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(new Profile());
            mainMapperMock.Setup(e => e.Map<ProfileDetail>(It.IsAny<Profile>())).Returns(new ProfileDetail());
            var admin = CreateAdministration();

            // act
            admin.GetProfileDetail(1);

            // assert
            profileDbMock.Verify(e => e.GetProfileById(1, ProfileIncludes.None), Times.Once);
        }

        [Fact]
        public void GetProfileDetail_NoAccessToProfile_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var profile = new Profile
            {
                ProfileId = 1,
            };
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            identityResolverMock.SetupCheckEntitleForProfile().Throws(new EntitleAccessDeniedException("", Entitle.MainProfiles));
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetProfileDetail(1));
            identityResolverMock.VerifyCheckEntitleForProfile(Entitle.MainProfiles, 1, Times.Once());
        }

        [Fact]
        public void GetProfileDetail_Profile_IsMappedToProfileDetail()
        {
            // arrange
            var profile = new Profile
            {
                ProfileId = 1,
            };
            var detail = new ProfileDetail();
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            mainMapperMock.Setup(e => e.Map<ProfileDetail>(It.IsAny<Profile>())).Returns(detail);
            var admin = CreateAdministration();

            // act
            var result = admin.GetProfileDetail(1);

            // assert
            Assert.Same(detail, result);
            mainMapperMock.Verify(e => e.Map<ProfileDetail>(profile), Times.Once);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, false, false)]
        [InlineData(false, true, false)]
        public void GetProfileDetail_SpecifiedCanDelete_EventIsRaisedAndReturnsExpectedCanDelete(bool canDeleteProfile, bool canDeleteUserGroup, bool expectedResult)
        {
            // arrange
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(new Profile());
            mainMapperMock.Setup(e => e.Map<ProfileDetail>(It.IsAny<Profile>())).Returns(new ProfileDetail());
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<ProfileDeletingEvent>())).Returns(canDeleteProfile);
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<UserGroupDeletingEvent>())).Returns(canDeleteUserGroup);

            var admin = CreateAdministration();

            // act
            var result = admin.GetProfileDetail(1);

            // assert
            Assert.Equal(expectedResult, result.CanDelete);
            eventDispatcherMock.Verify(e => e.Check(It.Is<ProfileDeletingEvent>(x => x.ProfileId == 1)), Times.Once);
        }

        [Fact]
        public void GetProfileDetail_ProfileCannotBeDeleted_UserGroupDeletingEventIsNotRaised()
        {
            // arrange
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(new Profile());
            mainMapperMock.Setup(e => e.Map<ProfileDetail>(It.IsAny<Profile>())).Returns(new ProfileDetail());
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<ProfileDeletingEvent>())).Returns(false);
            var admin = CreateAdministration();

            // act
            admin.GetProfileDetail(1);

            // assert
            eventDispatcherMock.Verify(e => e.Check(It.IsAny<UserGroupDeletingEvent>()), Times.Never);
        }

        [Fact]
        public void GetProfileDetail_ProfileCanBeDeleted_UserGroupDeletingEventIsRaised()
        {
            // arrange
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(new Profile());
            mainMapperMock.Setup(e => e.Map<ProfileDetail>(It.IsAny<Profile>())).Returns(new ProfileDetail());
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<ProfileDeletingEvent>())).Returns(true);
            var admin = CreateAdministration();

            // act
            admin.GetProfileDetail(1);

            // assert
            eventDispatcherMock.Verify(e => e.Check(It.Is<UserGroupDeletingEvent>(x => x.UserGroupId == 1 && x.UserGroupTypeId == UserGroupTypeEnum.MainProfile)), Times.Once);
        }
        #endregion

        #region GetNewProfileForm() tests
        [Fact]
        public void GetNewProfileForm_NoAccessToProfile_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            identityResolverMock.Setup(e => e.CheckEntitle(It.IsAny<Entitle>())).Throws(new EntitleAccessDeniedException("", Entitle.MainProfiles));
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetNewProfileForm());
            identityResolverMock.Verify(e => e.CheckEntitle(Entitle.MainProfiles), Times.Once);
        }

        [Fact]
        public void GetNewProfileForm_ReturnsExpectedProfileForm()
        {
            // arrange
            var admin = CreateAdministration();

            // act
            var result = admin.GetNewProfileForm();

            // assert
            Assert.Equal("", result.OriginMoniker);
        }
        #endregion

        #region GetProfileFormById() tests
        [Fact]
        public void GetProfileFormById_ProfileIdPassToGetProfileById()
        {
            // arrange
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(new Profile());
            mainMapperMock.Setup(e => e.Map<ProfileForm>(It.IsAny<Profile>())).Returns(new ProfileForm());
            var admin = CreateAdministration();

            // act
            admin.GetProfileFormById(1);

            // assert
            profileDbMock.Verify(e => e.GetProfileById(1, ProfileIncludes.None), Times.Once);
        }

        [Fact]
        public void GetProfileFormById_NoAccessToProfile_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var profile = new Profile
            {
                ProfileId = 1
            };
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            identityResolverMock.SetupCheckEntitleForProfile().Throws(new EntitleAccessDeniedException("", Entitle.MainProfiles));
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetProfileFormById(1));
            identityResolverMock.VerifyCheckEntitleForProfile(Entitle.MainProfiles, 1, Times.Once());
        }

        [Fact]
        public void GetProfileFormById_Profile_IsMappedToProfileForm()
        {
            // arrange
            var form = new ProfileForm();
            var profile = new Profile();
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            mainMapperMock.Setup(e => e.Map<ProfileForm>(It.IsAny<Profile>())).Returns(form);
            var admin = CreateAdministration();

            // act
            var result = admin.GetProfileFormById(1);

            // assert
            Assert.Same(form, result);
            mainMapperMock.Verify(e => e.Map<ProfileForm>(profile), Times.Once);
        }
        #endregion

        #region GetProfileFormByFormAndValidation() tests
        [Theory]
        [InlineData("ZnojmoOnline", "ZnojmoOnline")]
        [InlineData(null, null)]
        public void GetProfileFormByFormAndValidation_SpecifiedForbiddenMoniker_ReturnsExpectedForm(string validationForbiddenMoniker, string expectedForbiddenMoniker)
        {
            // arrange
            var form = new ProfileForm();
            var validation = new ProfileValidationResult
            {
                ForbiddenMoniker = validationForbiddenMoniker
            };
            var admin = CreateAdministration();

            // act
            var result = admin.GetProfileFormByFormAndValidation(form, validation);

            // assert
            Assert.Equal(expectedForbiddenMoniker, result.ForbiddenMoniker);
            Assert.Same(form, result);
        }
        #endregion

        #region ValidateProfileForm() tests
        [Fact]
        public void ValidateProfileForm_NoAccessToProfile_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var form = new ProfileForm();
            identityResolverMock.Setup(x => x.CheckEntitle(It.IsAny<Entitle>())).Throws(new EntitleAccessDeniedException("", Entitle.MainProfiles));
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.ValidateProfileForm(form));
            identityResolverMock.Verify(e => e.CheckEntitle(Entitle.MainProfiles), Times.Once);
        }

        [Fact]
        public void ValidateProfileForm_ExpectedMonikerPassToGetProfileByMoniker()
        {
            // arrange
            var form = new ProfileForm
            {
                Moniker = "ZnojmoOnline"
            };
            profileDbMock.Setup(e => e.GetProfileByMoniker(It.IsAny<string>(), It.IsAny<ProfileIncludes>())).Returns(new Profile());
            var admin = CreateAdministration();

            // act
            admin.ValidateProfileForm(form);

            // assert
            profileDbMock.Verify(e => e.GetProfileByMoniker("ZnojmoOnline", ProfileIncludes.None), Times.Once);
        }

        [Fact]
        public void ValidateProfileForm_ProfileWithMonikerIsNull_ReturnsProfileValidationResultWithSetIsValidAndForbiddenMoniker()
        {
            // arrange
            profileDbMock.Setup(e => e.GetProfileByMoniker(It.IsAny<string>(), It.IsAny<ProfileIncludes>())).Returns((Profile)null);
            var admin = CreateAdministration();

            // act
            var result = admin.ValidateProfileForm(new ProfileForm());

            // assert
            Assert.True(result.IsValid);
            Assert.Null(result.ForbiddenMoniker);
        }

        [Theory]
        [InlineData(1, 1, true, null)]
        [InlineData(1, 2, false, "Moniker")]
        public void ValidateProfileForm_ProfileIdSet_ReturnsProfileValidationResultWithExpectedValues(long profileId, long formProfileId, bool expectedIsValid, string expectedForbiddenMoniker)
        {
            // arrange
            var profile = new Profile
            {
                ProfileId = profileId,
                Moniker = "Moniker"
            };
            var form = new ProfileForm
            {
                ProfileId = formProfileId
            };
            profileDbMock.Setup(e => e.GetProfileByMoniker(It.IsAny<string>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            var admin = CreateAdministration();

            // act
            var actualResult = admin.ValidateProfileForm(form);

            // assert
            Assert.Equal(expectedIsValid, actualResult.IsValid);
            Assert.Equal(expectedForbiddenMoniker, actualResult.ForbiddenMoniker);
        }
        #endregion

        #region SaveProfile() tests
        [Fact]
        public void SaveProfile_ProfileForm_IsMappedToProfile()
        {
            // arrange
            var profile = new Profile
            {
                ProfileId = 1
            };
            var form = new ProfileForm();
            mainMapperMock.Setup(e => e.Map<Profile>(It.IsAny<ProfileForm>())).Returns(profile);
            var admin = CreateAdministration();

            // act
            var result = admin.SaveProfile(form, out _);

            // assert
            Assert.Equal(profile.ProfileId, result);
            mainMapperMock.Verify(e => e.Map<Profile>(form), Times.Once);
        }

        [Fact]
        public void SaveProfile_NoAccessToProfileForInsert_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            mainMapperMock.Setup(e => e.Map<Profile>(It.IsAny<ProfileForm>())).Returns(new Profile());
            identityResolverMock.Setup(e => e.CheckEntitle(It.IsAny<Entitle>())).Throws(new EntitleAccessDeniedException("", Entitle.MainProfiles));
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.SaveProfile(new ProfileForm(), out _));
            identityResolverMock.Verify(e => e.CheckEntitle(Entitle.MainProfiles), Times.Once);
        }

        [Fact]
        public void SaveProfile_ForNewProfile_SetsValuesAndInserts()
        {
            // arrange
            var profile = new Profile();
            Profile profileToInsert = null;
            var preference = new ClassPreference();
            mainMapperMock.Setup(e => e.Map<Profile>(It.IsAny<ProfileForm>())).Returns(profile);
            identityResolverMock.Setup(e => e.GetOwnerId()).Returns(10);
            classPreferenceFactoryMock.Setup(e => e.CreateDefaultClassPreference()).Returns(preference);
            profileDbMock.Setup(e => e.InsertProfile(It.IsAny<Profile>()))
                .Callback(new Action<Profile>(p => profileToInsert = p))
                .Returns(1);
            var admin = CreateAdministration();

            // act
            var result = admin.SaveProfile(new ProfileForm(), out _);

            // assert
            Assert.Equal(1, result);
            Assert.Equal(10, profileToInsert.OwnerId);
            Assert.Collection(profileToInsert.ProfileUsers, item => Assert.Equal(10, item.UserId));
            Assert.Same(preference, profileToInsert.ClassPreference);
            profileDbMock.Verify(e => e.InsertProfile(profile), Times.Once);
        }

        [Fact]
        public void SaveProfile_IsEntitleGrantedForUserGroupIsFalse_SetsUpdateIdentityClaims()
        {
            // arrange
            mainMapperMock.Setup(e => e.Map<Profile>(It.IsAny<ProfileForm>())).Returns(new Profile());
            identityResolverMock.Setup(e => e.IsEntitleGrantedForUserGroup(It.IsAny<Entitle>(), It.IsAny<UserGroupTypeEnum>(), It.IsAny<long>())).Returns(false);
            var admin = CreateAdministration();

            // act
            admin.SaveProfile(new ProfileForm(), out var updateIdentityClaims);

            // assert
            Assert.True(updateIdentityClaims);
        }

        [Fact]
        public void SaveProfile_NoAccessToProfileForUpdate_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var profile = new Profile
            {
                ProfileId = 1
            };
            var form = new ProfileForm();
            mainMapperMock.Setup(e => e.Map<Profile>(It.IsAny<ProfileForm>())).Returns(profile);
            identityResolverMock.SetupCheckEntitleForProfileId().Throws(new EntitleAccessDeniedException("", Entitle.MainProfiles));
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.SaveProfile(form, out _));
            identityResolverMock.VerifyCheckEntitleForProfileId(Entitle.MainProfiles, 1, Times.Once());
        }

        [Fact]
        public void SaveProfile_ForExistingProfile_ProfileIsUpdated()
        {
            // arrange
            var profile = new Profile
            {
                ProfileId = 1
            };
            var form = new ProfileForm();
            mainMapperMock.Setup(e => e.Map<Profile>(It.IsAny<ProfileForm>())).Returns(profile);
            profileDbMock.Setup(e => e.UpdateProfile(It.IsAny<Profile>()));
            var admin = CreateAdministration();

            // act
            var result = admin.SaveProfile(form, out var updateIdentityClaims);

            // assert
            Assert.False(updateIdentityClaims);
            Assert.Equal(1, result);
            profileDbMock.Verify(e => e.UpdateProfile(profile), Times.Once);

        }
        #endregion

        #region DeleteProfile() tests
        [Fact]
        public void DeleteProfile_NoAccessToProfile_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            identityResolverMock.SetupCheckEntitleForProfileId().Throws(new EntitleAccessDeniedException("", Entitle.MainProfiles));
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.DeleteProfile(1));
            identityResolverMock.VerifyCheckEntitleForProfileId(Entitle.MainProfiles, 1, Times.Once());
        }

        [Fact]
        public void DeleteProfile_ProfileCannotBeDeleted_ThrowsInvalidOperationException()
        {
            // arrange
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<ProfileDeletingEvent>())).Returns(false);
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<UserGroupDeletingEvent>())).Returns(false);
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => admin.DeleteProfile(1));
            eventDispatcherMock.Verify(e => e.Check(It.Is<ProfileDeletingEvent>(x => x.ProfileId == 1)), Times.Once);
            eventDispatcherMock.Verify(e => e.Check(It.Is<UserGroupDeletingEvent>(x => x.UserGroupId == 1 && x.UserGroupTypeId == UserGroupTypeEnum.MainProfile)), Times.Never);
        }

        [Fact]
        public void DeleteProfile_ProfileCanBeDeletedAndUserGroupCannot_ThrowsInvalidOperationException()
        {
            // arrange
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<ProfileDeletingEvent>())).Returns(true);
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<UserGroupDeletingEvent>())).Returns(false);
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => admin.DeleteProfile(1));
            eventDispatcherMock.Verify(e => e.Check(It.Is<ProfileDeletingEvent>(x => x.ProfileId == 1)), Times.Once);
            eventDispatcherMock.Verify(e => e.Check(It.Is<UserGroupDeletingEvent>(x => x.UserGroupId == 1 && x.UserGroupTypeId == UserGroupTypeEnum.MainProfile)), Times.Once);
        }

        [Fact]
        public void DeleteProfile_CanDeleteIsTrue_ProfileDeleted()
        {
            // arrange
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<ProfileDeletingEvent>())).Returns(true);
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<UserGroupDeletingEvent>())).Returns(true);

            var admin = CreateAdministration();

            // act
            admin.DeleteProfile(1000);

            // assert
            profileDbMock.Verify(e => e.DeleteProfile(1000), Times.Once);
            eventDispatcherMock.Verify(e => e.Check(It.Is<ProfileDeletingEvent>(x => x.ProfileId == 1000)), Times.Once);
            eventDispatcherMock.Verify(e => e.Check(It.Is<UserGroupDeletingEvent>(x => x.UserGroupId == 1000 && x.UserGroupTypeId == UserGroupTypeEnum.MainProfile)), Times.Once);
        }
        #endregion

        #region private methods
        private ProfileAdministration CreateAdministration()
        {
            return new ProfileAdministration(
                profileDbMock.Object,
                identityResolverMock.Object,
                mainMapperMock.Object,
                classPreferenceFactoryMock.Object,
                eventDispatcherMock.Object);
        }
        #endregion
    }
}
