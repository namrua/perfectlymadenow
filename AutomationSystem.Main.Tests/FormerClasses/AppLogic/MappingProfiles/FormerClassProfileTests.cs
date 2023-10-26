using AutoMapper;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;
using AutomationSystem.Main.Core.FormerClasses.AppLogic.MappingProfiles;
using AutomationSystem.Main.Model;
using System;
using Xunit;

namespace AutomationSystem.Main.Tests.FormerClasses.AppLogic.MappingProfiles
{
    public class FormerClassProfileTests
    {
        private readonly DateTime eventStart = new DateTime(2021, 1, 1);
        private readonly DateTime eventEnd = new DateTime(2021, 2, 2);


        #region CreateMap<FormerClass, FormerClassDetail>() test

        [Fact]
        public void Map_ClassTypeIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var formerClass = new FormerClass
            {
                ClassType = null
            };
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<FormerClassDetail>(formerClass));
        }

        [Fact]
        public void Map_ProfileIdIsNotNullAndProfileIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var formerClass = CreateFormerClass();
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<FormerClassDetail>(formerClass));
        }

        [Fact]
        public void Map_ProfileIdAndProfileAreNull_ReturnsFormerClassDetailWithNullProfile()
        {
            // arrange
            var formerClass = CreateFormerClass();
            formerClass.ProfileId = null;
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<FormerClassDetail>(formerClass);

            // arrange
            Assert.Null(result.Profile);
        }

        [Fact]
        public void Map_FormerClass_ReturnsFormerClassDetail()
        {
            // arrange
            var formerClass = CreateFormerClass();
            formerClass.Profile = new Model.Profile
            {
                ProfileId = 15,
                Name = "Name"
            };
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<FormerClassDetail>(formerClass);

            // assert
            Assert.Equal(1, result.FormerClassId);
            Assert.Equal(ClassTypeEnum.Basic, result.ClassTypeId);
            Assert.Equal("Name", result.Profile);
            Assert.Equal("Basic", result.ClassType);
            Assert.Equal("Location", result.Location);
            Assert.Equal(eventStart, result.EventStart);
            Assert.Equal(eventEnd, result.EventEnd);
            Assert.Equal("January 01 & February 02", result.ClassDate);
            Assert.Equal("January 01 & February 02, 2021", result.FullClassDate);
            Assert.Equal("January 01 & February 02, Location, Basic", result.ClassTitle);
        }

        #endregion

        #region CreateMap<FormerClass, FormerClassForm>() tests

        [Fact]
        public void Map_FormerClass_ReturnsFormerClassForm()
        {
            // arrange
            var formerClass = CreateFormerClass();
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<FormerClassForm>(formerClass);

            // assert
            Assert.Equal(1, result.FormerClassId);
            Assert.Equal(ClassTypeEnum.Basic, result.ClassTypeId);
            Assert.Equal("Location", result.Location);
            Assert.Equal(eventStart, result.EventStart);
            Assert.Equal(eventEnd, result.EventEnd);
        }

        #endregion

        #region CreateMap<FormerClassForm, FormerClass>() tests

        [Fact]
        public void Map_FormerClassForm_ReturnsFormerClass()
        {
            // arrange
            var form = new FormerClassForm
            {
                FormerClassId = 2,
                ClassTypeId = ClassTypeEnum.Basic,
                Location = "Location",
                EventStart = eventStart,
                EventEnd = eventEnd
            };
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<FormerClass>(form);

            // assert
            Assert.Equal(2, result.FormerClassId);
            Assert.Equal(ClassTypeEnum.Basic, result.ClassTypeId);
            Assert.Equal("Location", result.Location);
            Assert.Equal(eventStart, result.EventStart);
            Assert.Equal(eventEnd, result.EventEnd);

        }

        #endregion

        #region CreateMap<FormerClass, FormerClassListItem>() tests

        [Fact]
        public void Map_ClassTypeNotIncludedIntoFormerClassObject_ThrowsInvalidOperationException()
        {
            // arrange
            var formerClass = CreateFormerClass();
            formerClass.ClassType = null;
            var mapper = CreateMapper();

            // act & asssert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<FormerClassListItem>(formerClass));
        }

        [Fact]
        public void Map_FormerClass_FormerClassListItem()
        {
            // arrange
            var formerClass = CreateFormerClass();
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<FormerClassListItem>(formerClass);

            // assert
            Assert.Equal(1, result.FormerClassId);
            Assert.Equal(ClassTypeEnum.Basic, result.ClassTypeId);
            Assert.Equal("Basic", result.ClassType);
            Assert.Equal("Location", result.Location);
            Assert.Equal(eventStart, result.EventStart);
            Assert.Equal(eventEnd, result.EventEnd);
            Assert.Equal("January 01 & February 02", result.ClassDate);
            Assert.Equal("January 01 & February 02, 2021", result.FullClassDate);
            Assert.Equal("January 01 & February 02, Location, Basic", result.ClassTitle);
        }

        #endregion

        #region private methods

        private Mapper CreateMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FormerClassProfile());
            });
            return new Mapper(mapperConfiguration);
        }

        private FormerClass CreateFormerClass()
        {
            return new FormerClass
            {
                FormerClassId = 1,
                ProfileId = 15,
                ClassTypeId = ClassTypeEnum.Basic,
                ClassType = new ClassType
                {
                    Description = "Basic"
                },
                Location = "Location",
                EventStart = eventStart,
                EventEnd = eventEnd
            };
        }

        #endregion
    }
}
