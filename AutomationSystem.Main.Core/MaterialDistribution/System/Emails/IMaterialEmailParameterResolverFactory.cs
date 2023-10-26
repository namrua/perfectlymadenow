using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.MaterialDistribution.System.Emails
{
    public interface IMaterialEmailParameterResolverFactory
    {
        IEmailParameterResolverWithBinding<ClassMaterialRecipient> CreateClassMaterialRecipientParameterResolver();
    }
}
