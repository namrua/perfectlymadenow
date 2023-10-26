using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Emails.System.Models;

namespace AutomationSystem.Shared.Core.Emails.System
{
    public class EmailTextResolverFactory : IEmailTextResolverFactory
    {
        private readonly ICoreEmailParameterResolverFactory coreParameterResolverFactory;

        public EmailTextResolverFactory(ICoreEmailParameterResolverFactory coreParameterResolverFactory)
        {
            this.coreParameterResolverFactory = coreParameterResolverFactory;
        }

        public IEmailTextResolver CreateEmailTextResolver(params IEmailParameterResolver[] resolvers)
        {
            var composed = coreParameterResolverFactory.CreateComposedParameterResolver(resolvers);
            return new EmailTextResolver(EmailConstants.ParameterRegexPattern, composed);
        }
    }
}
