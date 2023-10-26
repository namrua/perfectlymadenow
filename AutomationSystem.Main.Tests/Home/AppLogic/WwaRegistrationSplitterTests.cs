using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Core.Home.AppLogic;
using AutomationSystem.Main.Core.Registrations.System;
using Moq;
using System.Collections.Generic;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using Xunit;

namespace AutomationSystem.Main.Tests.Home.AppLogic
{
    public class WwaRegistrationSplitterTests
    {
        private readonly Mock<IRegistrationTypeResolver> typeResolverMock;

        public WwaRegistrationSplitterTests()
        {
            typeResolverMock = new Mock<IRegistrationTypeResolver>();

            typeResolverMock.Setup(e => e.IsWwaRegistration(It.IsAny<RegistrationTypeEnum>())).Returns(false);
            typeResolverMock.Setup(e => e.IsWwaRegistration(RegistrationTypeEnum.WWA)).Returns(true);
        }


        #region SplitWwaClasses() tests

        [Fact]
        public void SplitWwaClasses_ClassPublicDetails_ReturnsClsPublicDetailsWithWwa()
        {
            // arrange
            var classes = new List<ClassPublicDetail>
            {
                CreateDetail(1, ClassCategoryEnum.DistanceClass, false),
                CreateDetail(5, ClassCategoryEnum.Class, false),
                CreateDetail(10, ClassCategoryEnum.Class, true)
            };
            var splitter = CreateSplitter();

            // act
            var result = splitter.SplitWwaClasses(classes);

            // assert
            Assert.Collection(result,
                item =>
                {
                    Assert.Equal(1, item.ClassId);
                    Assert.False(item.MarkedAsWwa);
                },
                item =>
                {
                    Assert.Equal(5, item.ClassId);
                    Assert.False(item.MarkedAsWwa);
                },
                item =>
                {
                    Assert.Equal(10, item.ClassId);
                    Assert.False(item.MarkedAsWwa);
                },
                item =>
                {
                    Assert.Equal(10, item.ClassId);
                    Assert.True(item.MarkedAsWwa);
                });
        }

        #endregion

        #region FilterRegistrationTypes() tests

        [Fact]
        public void FilterRegistrationTypes_ForWwaIsNull_ReturnsGivenRegistrationTypeListItems()
        {
            // arrange
            var types = new List<RegistrationTypeListItem>
            {
                new RegistrationTypeListItem
                {
                    RegistrationTypeId = RegistrationTypeEnum.NewAdult
                },
                new RegistrationTypeListItem
                {
                    RegistrationTypeId = RegistrationTypeEnum.WWA
                }
            };
            var splitter = CreateSplitter();

            // act
            var result = splitter.FilterRegistrationTypes(types, null);

            // assert
            Assert.Collection(result,
                item => Assert.Equal(RegistrationTypeEnum.NewAdult, item.RegistrationTypeId),
                item => Assert.Equal(RegistrationTypeEnum.WWA, item.RegistrationTypeId));
        }

        [Fact]
        public void FilterRegistrationTypes_NotForWwa_ReturnsTypesWithoutWwa()
        {
            // arrange
            var splitter = CreateSplitter();

            // act
            var result = splitter.FilterRegistrationTypes(CreateTypes(), false);

            // assert
            Assert.Collection(result,
                item => Assert.Equal(RegistrationTypeEnum.NewAdult, item.RegistrationTypeId),
                item => Assert.Equal(RegistrationTypeEnum.NewChild, item.RegistrationTypeId));
        }

        [Fact]
        public void FilterRegistrationTypes_ForForWwa_ReturnsWwaRegistrationType()
        {
            // arrange
            var splitter = CreateSplitter();

            // act
            var result = splitter.FilterRegistrationTypes(CreateTypes(), true);

            // assert
            Assert.Collection(result,
                item => Assert.Equal(RegistrationTypeEnum.WWA, item.RegistrationTypeId));
        }

        #endregion

        #region ResolveForWwa() tests

        [Theory]
        [InlineData(ClassCategoryEnum.DistanceClass, true)]
        [InlineData(ClassCategoryEnum.DistanceClass, false)]
        [InlineData(ClassCategoryEnum.Class, false)]
        public void ResolveForWwa_ForWwaNotApplicable_ReturnNull(ClassCategoryEnum classCategoryId, bool isWwaAllowed)
        {
            // arrange
            var splitter = CreateSplitter();

            // act
            var result = splitter.ResolveForWwa(true, RegistrationTypeEnum.NewAdult, classCategoryId, isWwaAllowed);

            // assert
            Assert.Null(result);
            typeResolverMock.Verify(e => e.IsWwaRegistration(It.IsAny<RegistrationTypeEnum>()), Times.Never);
        }


        [Theory]
        [InlineData(RegistrationTypeEnum.WWA, true)]
        [InlineData(RegistrationTypeEnum.NewAdult, false)]
        [InlineData(null, null)]
        public void ResolveForWwa_ForWwaIsNull_ReturnsResultByBackFromRegistrationTypeId(RegistrationTypeEnum? typeId, bool? expectedResult)
        {
            // arrange
            var splitter = CreateSplitter();

            // act
            var actualResult = splitter.ResolveForWwa(null, typeId, ClassCategoryEnum.Class, true);

            // assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ResolveForWwa_RegistrationTypeIdAndForWwaHaveValue_ReturnsForWwa()
        {
            // arrange
            var splitter = CreateSplitter();

            // act
            var result = splitter.ResolveForWwa(true, RegistrationTypeEnum.NewAdult, ClassCategoryEnum.Class, true);

            // assert
            Assert.True(result);
        }

        #endregion

        #region private methods

        private WwaRegistrationSplitter CreateSplitter()
        {
            return new WwaRegistrationSplitter(typeResolverMock.Object);
        }

        private List<RegistrationTypeListItem> CreateTypes()
        {
            return new List<RegistrationTypeListItem>
            {
                new RegistrationTypeListItem
                {
                    RegistrationTypeId = RegistrationTypeEnum.NewAdult,
                },
                new RegistrationTypeListItem
                {
                    RegistrationTypeId = RegistrationTypeEnum.NewChild
                },
                new RegistrationTypeListItem
                {
                    RegistrationTypeId = RegistrationTypeEnum.WWA
                }
            };
        }

        private ClassPublicDetail CreateDetail(long classId, ClassCategoryEnum classCategoryId, bool isWwaAllowed)
        {
            return new ClassPublicDetail
            {
                ClassId = classId,
                ClassCategoryId = classCategoryId,
                IsWwaFormAllowed = isWwaAllowed
            };
        }

        #endregion
    }
}
