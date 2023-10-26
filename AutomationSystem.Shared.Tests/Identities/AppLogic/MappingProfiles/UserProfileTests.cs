using AutoMapper;
using AutomationSystem.Shared.Contract.Identities.AppLogic.Models;
using AutomationSystem.Shared.Core.Identities.AppLogic.MappingProfiles;
using AutomationSystem.Shared.Model;
using Xunit;

namespace AutomationSystem.Shared.Tests.Identities.AppLogic.MappingProfiles
{
    public class UserProfileTests
    {
        #region CreateMap<User, UserShortDetail>() tests
        [Fact]
        public void Map_User_ReturnsUserShortDetail()
        {
            // arrange
            var user = new User
            {
                UserId = 1,
                Name = "Name",
                GoogleAccount = "Email",
                Active = true
            };
            var mapper = CreateMapper();

            // act
            var userShortDetail = mapper.Map<UserShortDetail>(user);

            // assert
            Assert.Equal(1, userShortDetail.UserId);
            Assert.Equal("Name", userShortDetail.Name);
            Assert.Equal("Email", userShortDetail.Email);
            Assert.True(userShortDetail.Active);
        }
        #endregion

        #region private methods
        private Mapper CreateMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
            });
            return new Mapper(mapperConfiguration);
        }
        #endregion
    }
}
