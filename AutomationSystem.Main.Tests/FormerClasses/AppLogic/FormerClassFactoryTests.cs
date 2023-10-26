using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Core.FormerClasses.AppLogic;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Enums.Data.Models;
using Moq;
using System.Collections.Generic;
using AutomationSystem.Main.Core.Classes.System;
using Xunit;

namespace AutomationSystem.Main.Tests.FormerClasses.AppLogic
{
    public class FormerClassFactoryTests
    {
        private readonly Mock<IEnumDatabaseLayer> enumDbMock;
        private readonly Mock<IProfileDatabaseLayer> profileDbMock;
        private readonly Mock<IClassTypeResolver> classTypeResolverMock;

        public FormerClassFactoryTests()
        {
            enumDbMock = new Mock<IEnumDatabaseLayer>();
            profileDbMock = new Mock<IProfileDatabaseLayer>();
            classTypeResolverMock = new Mock<IClassTypeResolver>();
        }

        #region CreateFormerClassForEdit() tests

        [Fact]
        public void CreateFormerClassForEdit_OnlyClassTypesAreFiltered_ReturnsFormerClassForEdit()
        {
            // arrange
            var classTypes = new List<IEnumItem>
            {
                new EnumItem
                {
                    Id = 1,
                    Description = "BasicOnline"
                },
                new EnumItem
                {
                    Id = 6,
                    Description = "LectureBasicOnline"
                }
            };
            profileDbMock.Setup(e => e.GetProfilesByFilter(It.IsAny<ProfileFilter>(), It.IsAny<ProfileIncludes>())).Returns(new List<Profile>());
            enumDbMock.Setup(e => e.GetItemsByFilter(It.IsAny<EnumTypeEnum>(), It.IsAny<EnumItemFilter>())).Returns(classTypes);
            classTypeResolverMock.Setup(e => e.GetAllowedClassTypesForFormerClasses()).Returns(new HashSet<ClassTypeEnum> { ClassTypeEnum.BasicOnline });
            var factory = CreateFactory();

            // act
            var result = factory.CreateFormerClassForEdit();

            // assert
            Assert.Collection(result.ClassTypes,
                item =>
                {
                    Assert.Equal(1, item.Id);
                    Assert.Equal("BasicOnline", item.Description);
                });
            enumDbMock.Verify(e => e.GetItemsByFilter(EnumTypeEnum.MainClassType, null), Times.Once);
        }

        [Fact]
        public void CreateFormerClassForEdit_ProfilesAreSet_ReturnsFormerClassForEditWithProfilesFromDb()
        {
            // arrange
            var profiles = new List<Profile>
            {
                new Profile
                {
                    ProfileId = 1,
                    Name = "ProfileOne"
                },
                new Profile
                {
                    ProfileId = 2,
                    Name = "ProfileTwo"
                }
            };
            profileDbMock.Setup(e => e.GetProfilesByFilter(It.IsAny<ProfileFilter>(), It.IsAny<ProfileIncludes>())).Returns(profiles);
            enumDbMock.Setup(e => e.GetItemsByFilter(It.IsAny<EnumTypeEnum>(), It.IsAny<EnumItemFilter>())).Returns(new List<IEnumItem>());
            var factory = CreateFactory();

            // act
            var result = factory.CreateFormerClassForEdit();

            // assert
            Assert.Collection(result.Profiles,
                item =>
                {
                    Assert.Equal("1", item.Id);
                    Assert.Equal("ProfileOne", item.Text);
                },
                item =>
                {
                    Assert.Equal("2", item.Id);
                    Assert.Equal("ProfileTwo", item.Text);
                });
            profileDbMock.Verify(e => e.GetProfilesByFilter(null, ProfileIncludes.None), Times.Once);
        }

        #endregion

        #region private methods

        private FormerClassFactory CreateFactory()
        {
            return new FormerClassFactory(enumDbMock.Object, profileDbMock.Object, classTypeResolverMock.Object);
        }

        #endregion
    }
}
