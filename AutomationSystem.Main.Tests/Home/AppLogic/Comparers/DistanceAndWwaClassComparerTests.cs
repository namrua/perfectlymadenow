using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Home.AppLogic.Comparers;
using AutomationSystem.Main.Model;
using System;
using Xunit;

namespace AutomationSystem.Main.Tests.Home.AppLogic.Comparers
{
    public class DistanceAndWwaClassComparerTests
    {
        private readonly DateTime EventStart = new DateTime(2020, 1, 1);
        private readonly DateTime EventEnd = new DateTime(2020, 2, 2);

        #region Equals() tests

        [Fact]
        public void Equals_SameClasses_ReturnsTrue()
        {
            // arrange
            var classOne = new Class();
            var classTwo = classOne;
            var comparer = CreateComparer();

            // act
            var result = comparer.Equals(classOne, classTwo);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_ClassOneIsNull_ReturnsFalse()
        {
            // arrange
            var classOne = (Class)null;
            var classTwo = CreateClass();
            var comparer = CreateComparer();

            // act
            var result = comparer.Equals(classOne, classTwo);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_ClassTwoIsNull_ReturnsFalse()
        {
            // arrange
            var classOne = CreateClass();
            var classTwo = (Class)null;
            var comparer = CreateComparer();

            // act
            var result = comparer.Equals(classOne, classTwo);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_ClassesAreNull_ReturnsTrue()
        {
            // arrange
            var comparer = CreateComparer();

            // act
            var result = comparer.Equals((Class) null, (Class) null);

            // assert
            Assert.True(result);
        }
        
        [Theory]
        [InlineData("Location", true)]
        [InlineData("  location       ", true)]
        [InlineData("l o c a ti o n", true)]
        [InlineData("l-oca, ti()on", true)]
        [InlineData("locations", false)]
        public void Equals_LocationIsWrittenDifferently_ReturnsExpectedResult(string location, bool expectedResult)
        {
            // arrange
            var clsOne = CreateClass(location);
            var clsTwo = CreateClass();
            var comparer = CreateComparer();

            // act
            var actualResult = comparer.Equals(clsOne, clsTwo);

            // assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Equals_DifferentClassInstances_ReturnsTrue()
        {
            // arrange
            var clsOne = CreateClass();
            var clsTwo = CreateClass();
            var comparer = CreateComparer();

            // act
            var result = comparer.Equals(clsOne, clsTwo);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_DifferentClassTypeId_ReturnsFalse()
        {
            // arrange
            var clsOne = CreateClass();
            var clsTwo = CreateClass("location", ClassTypeEnum.Basic2Online);
            var comparer = CreateComparer();

            // act
            var result = comparer.Equals(clsOne, clsTwo);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_DifferentEventStart_ReturnsFalse()
        {
            // arrange
            var clsOne = CreateClass();
            var clsTwo = CreateClass();
            clsTwo.EventStart = new DateTime(2020, 2, 1);
            var comparer = CreateComparer();

            // act
            var result = comparer.Equals(clsOne, clsTwo);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_DifferentEventEnd_ReturnsFalse()
        {
            // arrange
            var clsOne = CreateClass();
            var clsTwo = CreateClass();
            clsTwo.EventEnd = new DateTime(2020, 3, 1);
            var comparer = CreateComparer();

            // act
            var result = comparer.Equals(clsOne, clsTwo);

            // assert
            Assert.False(result);
        }

        #endregion

        #region private methods

        private DistanceAndWwaClassComparer CreateComparer()
        {
            return new DistanceAndWwaClassComparer();
        }

        private Class CreateClass(string location = "location", ClassTypeEnum classTypeId = ClassTypeEnum.Basic)
        {
            return new Class
            {
                EventStart = EventStart,
                EventEnd = EventEnd,
                ClassTypeId = classTypeId,
                Location = location
            };
        }

        #endregion
    }
}
