using System.Collections.Generic;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Core.Emails.System.EmailParameterResolvers;
using AutomationSystem.Shared.Core.Incidents.System.Emails.EmailParameterResolvers;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Emails.System
{
    public class CoreEmailParameterResolverFactory : ICoreEmailParameterResolverFactory
    {
        private readonly IEmailServiceHelper emailServiceHelper;

        public CoreEmailParameterResolverFactory(IEmailServiceHelper emailServiceHelper)
        {
            this.emailServiceHelper = emailServiceHelper;
        }

        public IEmailParameterResolver CreateComposedParameterResolver(params IEmailParameterResolver[] resolvers)
        {
            return new ComposedParameterResolver(resolvers);
        }

        public IEmailParameterResolver CreateLocalisedParameterResolver(IEnumerable<string> supportedTextsLabels)
        {
            return new LocalisedParameterResolver(emailServiceHelper, supportedTextsLabels);
        }

        public IEmailParameterResolverWithBinding<IDictionary<string, object>> CreateTextMapParameterResolver()
        {
            return new TextMapParameterResolver(emailServiceHelper);
        }

        public IEmailParameterResolverWithBinding<Incident> CreateIncidentParameterResolver()
        {
            return new IncidentParameterResolver(emailServiceHelper);
        }
    }
}
