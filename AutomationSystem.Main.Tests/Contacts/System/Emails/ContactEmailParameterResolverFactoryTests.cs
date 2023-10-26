using AutomationSystem.Main.Core.Contacts.System.Emails;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Contacts.System.Emails
{
    public class ContactEmailParameterResolverFactoryTests
    {
        private readonly Mock<IEmailServiceHelper> helperMock;

        public ContactEmailParameterResolverFactoryTests()
        {
            helperMock = new Mock<IEmailServiceHelper>();
        }

        #region CreateFormerStudentParameterResolver() tests

        [Fact]
        public void CreateFormerStudentParameterResolver_ReturnsFormerStudentParameterResolver()
        {
            // arrange
            var factory = CreateFactory();
            
            // act
            var result = factory.CreateFormerStudentParameterResolver();

            // assert
            Assert.IsAssignableFrom<IEmailParameterResolverWithBinding<FormerStudent>>(result);
        }

        #endregion

        #region private methods

        private ContactEmailParameterResolverFactory CreateFactory()
        {
            return new ContactEmailParameterResolverFactory(helperMock.Object);
        }

        #endregion
    }
}
