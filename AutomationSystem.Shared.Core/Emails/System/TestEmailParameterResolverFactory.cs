using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Core.Emails.System.EmailParameterResolvers;

namespace AutomationSystem.Shared.Core.Emails.System
{
    public class TestEmailParameterResolverFactory : ITestEmailParameterResolverFactory
    {
        private readonly IEmailServiceHelper emailServiceHelper;
        private readonly IEmailEntityParameterResolverFactory emailEntityParameterResolverFactory;

        public TestEmailParameterResolverFactory(IEmailServiceHelper emailServiceHelper, IEmailEntityParameterResolverFactory emailEntityParameterResolverFactory)
        {
            this.emailServiceHelper = emailServiceHelper;
            this.emailEntityParameterResolverFactory = emailEntityParameterResolverFactory;
        }

        public IEmailParameterResolver CreateTestParameterResolverByEntity(EmailEntityId emailEntityId)
        {
            var entityResolver = emailEntityParameterResolverFactory.CreateResolverByEntity(emailEntityId);
            return new TestParameterResolver(emailServiceHelper, entityResolver);
        }
    }
}
