using System.Collections.Generic;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.Emails.System
{
    public interface ICoreEmailParameterResolverFactory
    {
        IEmailParameterResolver CreateComposedParameterResolver(params IEmailParameterResolver[] resolvers);
        IEmailParameterResolver CreateLocalisedParameterResolver(IEnumerable<string> supportedTextsLabels);
        IEmailParameterResolverWithBinding<IDictionary<string, object>> CreateTextMapParameterResolver();
        IEmailParameterResolverWithBinding<Incident> CreateIncidentParameterResolver();
    }
}
