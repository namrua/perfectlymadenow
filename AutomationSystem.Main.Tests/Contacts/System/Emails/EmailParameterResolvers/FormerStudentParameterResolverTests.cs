using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Contacts.System.Emails.EmailParameterResolvers;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;
using Moq;
using System;
using Xunit;

namespace AutomationSystem.Main.Tests.Contacts.System.Emails.EmailParameterResolvers
{
    public class FormerStudentParameterResolverTests
    {
        private readonly Mock<IEmailServiceHelper> helperMock;

        public FormerStudentParameterResolverTests()
        {
            helperMock = new Mock<IEmailServiceHelper>();
        }

        #region IsSupportedParameters() tests

        [Theory]
        [InlineData(FormerStudentParameterResolver.FullName, true)]
        [InlineData(FormerStudentParameterResolver.FullNameCapital, true)]
        [InlineData("{{FormerStudent}}", false)]
        public void IsSupportedParameters_ParameterNameWithBrackets_ReturnsExpectedResult(string parameterNameWithBrackets, bool expectedResult)
        {
            // arrange
            var resolver = CreateResolver();

            // act
            var actualResult = resolver.IsSupportedParameters(parameterNameWithBrackets);

            // assert
            Assert.Equal(expectedResult, actualResult);
        }

        #endregion

        #region GetValue() tests

        [Fact]
        public void GetValue_FormerStudentIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var resolver = CreateResolver();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => resolver.GetValue(LanguageEnum.En, FormerStudentParameterResolver.FullName));
        }

        [Theory]
        [InlineData(FormerStudentParameterResolver.FullName, "FirstName LastName")]
        [InlineData(FormerStudentParameterResolver.FullNameCapital, "FIRSTNAME LASTNAME")]
        public void GetValue_FullName_ReturnsFormerStudentFullName(string nameWithBrackets, string expectedResult)
        {
            // arrange
            var formerStudent = CreateFormerStudent();
            helperMock.Setup(e => e.EmailParameterConvertor.Convert(It.IsAny<LanguageEnum>(), It.IsAny<string>(), It.IsAny<object>())).Returns(expectedResult);
            var resolver = CreateResolver();
            resolver.Bind(formerStudent);


            // act
            var actualResult = resolver.GetValue(LanguageEnum.En, nameWithBrackets);

            // assert
            Assert.Equal(expectedResult, actualResult);
            helperMock.Verify(e => e.EmailParameterConvertor.Convert(LanguageEnum.En, nameWithBrackets, expectedResult), Times.Once);
        }

        #endregion

        #region private methods

        private FormerStudentParameterResolver CreateResolver()
        {
            return new FormerStudentParameterResolver(helperMock.Object);
        }

        private FormerStudent CreateFormerStudent()
        {
            return new FormerStudent
            {
                Address = new Address
                {
                    FirstName = "FirstName",
                    LastName = "LastName"
                }
            };
        }

        #endregion
    }
}
