using System;
using Xunit;
using AutoMapper;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Contract.DistanceProfiles.AppLogic.Models;
using AutomationSystem.Main.Core.DistanceProfiles.AppLogic.MappingProfiles;

namespace AutomationSystem.Main.Tests.DistanceProfiles.AppLogic.MappingProfiles
{
    public class DistanceProfileProfileTests
    {
        #region CreateMap<DistanceProfile, DistanceProfileListItem>() test
        [Fact]
        public void MapDistanceProfileToDistanceProfileListItem_ProfileIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var distanceProfile = CreateProfile();
            distanceProfile.Profile = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<DistanceProfileListItem>(distanceProfile));
        }

        [Fact]
        public void MapDistanceProfileToDistanceProfileListItem_PriceListIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var distanceProfile = CreateProfile();
            distanceProfile.PriceList = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<DistanceProfileListItem>(distanceProfile));
        }

        [Fact]
        public void MapDistanceProfileToDistanceProfileListItem_DistanceCoordinatorIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var distanceProfile = CreateProfile();
            distanceProfile.DistanceCoordinator = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<DistanceProfileListItem>(distanceProfile));
        }

        [Fact]
        public void MapDistanceProfileToDistanceProfileListItem_DistanceCoordinatorAddressIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var distanceProfile = CreateProfile();
            distanceProfile.DistanceCoordinator.Address = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<DistanceProfileListItem>(distanceProfile));
        }

        [Fact]
        public void MapDistanceProfileToDistanceProfileListItem_DistanceProfile_ReturnsDistanceProfileListItem()
        {
            // arrange
            var distanceProfile = CreateProfile();
            var mapper = CreateMapper();

            // act
            var item = mapper.Map<DistanceProfileListItem>(distanceProfile);

            // assert
            Assert.Equal(1, item.DistanceProfileId);
            Assert.Equal("Profile", item.Profile);
            Assert.Equal("PriceList", item.PriceList);
            Assert.Equal("FirstName LastName", item.DistanceCoordinator);
            Assert.True(item.IsActive);
        }
        #endregion

        #region CreateMap<DistanceProfile, DistanceProfileDetail>() tests
        [Fact]
        public void MapDistanceProfileDetail_ProfileIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var distanceProfile = CreateProfile();
            distanceProfile.Profile = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<DistanceProfileDetail>(distanceProfile));
        }

        [Fact]
        public void MapDistanceProfileDetail_PriceListIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var distanceProfile = CreateProfile();
            distanceProfile.PriceList = null;
            var mappper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mappper.Map<DistanceProfileDetail>(distanceProfile));
        }

        [Fact]
        public void MapDistanceProfileDetail_DistanceCoordinatorIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var distanceProfile = CreateProfile();
            distanceProfile.DistanceCoordinator = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<DistanceProfileDetail>(distanceProfile));
        }

        [Fact]
        public void MapDistanceProfileDetail_DistanceCoordinatorAddressIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var distanceProfile = CreateProfile();
            distanceProfile.DistanceCoordinator.Address = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<DistanceProfileDetail>(distanceProfile));
        }

        [Fact]
        public void MapDistanceProfileDetail_DistanceProfile_ReturnsDistanceProfileDetail()
        {
            // arrange
            var distanceProfile = CreateProfile();
            var mapper = CreateMapper();

            // act
            var detail = mapper.Map<DistanceProfileDetail>(distanceProfile);

            // assert
            Assert.Equal(1, detail.DistanceProfileId);
            Assert.Equal("Profile", detail.Profile);
            Assert.Equal("PriceList", detail.PriceList);
            Assert.Equal("FirstName LastName", detail.DistanceCoordinator);
            Assert.True(detail.IsActive);
        }
        #endregion

        #region CreateMap<DistanceProfileForm, DistanceProfile>() tests
        [Fact]
        public void Map_DistanceProfileForm_ReturnsDistanceProfile()
        {
            // arrange
            var form = new DistanceProfileForm
            {
                DistanceCoordinatorId = 1,
                DistanceProfileId = 2,
                PayPalKeyId = 3,
                PriceListId = 4,
                ProfileId = 5
            };
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<DistanceProfile>(form);

            // assert
            Assert.Equal(1, result.DistanceCoordinatorId);
            Assert.Equal(2, result.DistanceProfileId);
            Assert.Equal(3, result.PayPalKeyId);
            Assert.Equal(4, result.PriceListId);
            Assert.Equal(5, result.ProfileId);
        }
        #endregion

        #region CreateMap<DistanceProfile, DistanceProfileForm>() tests
        [Fact]
        public void Map_DistanceProfile_ReturnsDistanceProfileForm()
        {
            // arrange
            var profile = CreateProfile();
            var mapper = CreateMapper();

            // act
            var form = mapper.Map<DistanceProfileForm>(profile);

            // assert
            Assert.Equal(1, form.DistanceProfileId);
            Assert.Equal(2, form.ProfileId);
            Assert.Equal(3, form.PayPalKeyId);
            Assert.Equal(3, form.CurrentPayPalKeyId);
            Assert.Equal(4, form.PriceListId);
            Assert.Equal(4, form.PriceListId);
            Assert.Equal(5, form.DistanceCoordinatorId);
        }
        #endregion

        #region private methods
        private DistanceProfile CreateProfile()
        {
            return new DistanceProfile
            {
                DistanceProfileId = 1,
                ProfileId = 2,
                PayPalKeyId = 3,
                PriceListId = 4,
                DistanceCoordinatorId = 5,
                Profile = new Model.Profile
                {
                    Name = "Profile"
                },
                PriceList = new PriceList
                {
                    Name = "PriceList"
                },
                DistanceCoordinator = new Person
                {
                    Address = new Address
                    {
                        FirstName = "FirstName",
                        LastName = "LastName"
                    }
                },
                IsActive = true
            };
        }

        private Mapper CreateMapper()
        {
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DistanceProfileProfile());
            });
            return new Mapper(mapperCfg);
        }
        #endregion
    }
}
