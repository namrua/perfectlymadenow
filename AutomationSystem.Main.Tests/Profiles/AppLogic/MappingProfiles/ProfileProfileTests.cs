using System;
using AutoMapper;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Core.Profiles.AppLogic.MappingProfiles;
using AutomationSystem.Main.Model;
using Xunit;
using Profile = AutomationSystem.Main.Model.Profile;

namespace AutomationSystem.Main.Tests.Profiles.AppLogic.MappingProfiles
{
    public class ProfileProfileTests
    {
        #region CreateMap<ProfileForm, Profile>() tests
        [Fact]
        public void Map_ProfileForm_ReturnsProfile()
        {
            // arrange
            var profileForm = new ProfileForm()
            {
                ProfileId = 1,
                Name = "Name",
                Moniker = "Moniker"
            };
            var mapper = CreateMapper();

            // act
            var profile = mapper.Map<Profile>(profileForm);

            // assert
            Assert.Equal(1, profile.ProfileId);
            Assert.Equal("Name", profile.Name);
            Assert.Equal("Moniker", profile.Moniker);
        }
        #endregion

        #region CreateMap<Profile, ProfileDetail>() tests
        [Fact]
        public void Map_Profile_ReturnsProfileDetail()
        {
            // arrange
            var profile = CreateProfile();
            var mapper = CreateMapper();

            // act
            var profileDetail = mapper.Map<ProfileDetail>(profile);

            // assert
            Assert.Equal(1, profileDetail.ProfileId);
            Assert.Equal("Name", profileDetail.Name);
            Assert.Equal("Moniker", profileDetail.Moniker);
        }
        #endregion

        #region CreateMap<Profile, ProfileForm>() tests
        [Fact]
        public void Map_Profile_ReturnsProfileForm()
        {
            // arrange
            var profile = CreateProfile();
            var mapper = CreateMapper();

            // act
            var profileForm = mapper.Map<ProfileForm>(profile);

            // assert
            Assert.Equal(1, profileForm.ProfileId);
            Assert.Equal("Name", profileForm.Name);
            Assert.Equal("Moniker", profileForm.OriginMoniker);
            Assert.Equal("Moniker", profileForm.Moniker);
        }
        #endregion

        #region CreateMap<Profile, ProfileListItem>() tests
        [Fact]
        public void Map_Profile_ReturnsProfileListItem()
        {
            // arrange
            var profile = CreateProfile();
            var mapper = CreateMapper();

            // act
            var profileListItem = mapper.Map<ProfileListItem>(profile);

            // assert
            Assert.Equal(1, profileListItem.ProfileId);
            Assert.Equal("Name", profileListItem.Name);
            Assert.Equal("Moniker", profileListItem.Moniker);
        }
        #endregion
        
        #region CreateMap<Profile, RegistrationPageStyle>() tests
        [Fact]
        public void Map_ProfileClassPreferenceIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var profile = new Profile
            {
                ClassPreference = null
            };
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<RegistrationPageStyle>(profile));
        }

        [Fact]
        public void Map_ClassPreferenceWithoutHomepageUrl_ReturnsRegistrationPageStyleWithSetHomepageUrlForProfile()
        {
            // arrange
            var profile = new Profile
            {
                Moniker = "profile",
                ClassPreference = new ClassPreference
                {
                    HeaderPictureId = 1,
                    RegistrationColorSchemeId = RegistrationColorSchemeEnum.Limet
                }
            };
            var mapper = CreateMapper();

            // act
            var registrationPageStyle = mapper.Map<RegistrationPageStyle>(profile);

            // assert
            Assert.Equal(1, registrationPageStyle.HeaderPictureId);
            Assert.Equal(RegistrationColorSchemeEnum.Limet, registrationPageStyle.ColorSchemeId);
            Assert.Equal("/Home/Index/profile", registrationPageStyle.HomepageUrl);
        }

        [Fact]
        public void Map_ClassPreferenceWithHomepageUrl_ReturnsRegistrationPageStyle()
        {
            // arrange
            var profile = new Profile
            {
                Moniker = "profile",
                ClassPreference = new ClassPreference
                {
                    HomepageUrl = "url"
                }
            };
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<RegistrationPageStyle>(profile);

            // assert
            Assert.Equal("url", result.HomepageUrl);
        }

        #endregion
        
        #region private methods
        private Profile CreateProfile()
        {
            return new Profile
            {
                ProfileId = 1,
                Name = "Name",
                Moniker = "Moniker"
            };
        }
        private Mapper CreateMapper()
        {
            var mapConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProfileProfile());
            });
            return new Mapper(mapConfiguration);
        }
        #endregion
    }
}
