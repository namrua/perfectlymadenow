using AutomationSystem.Main.Core.MaterialDistribution.System.Emails.EmailParameterResolvers;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.MaterialDistribution.System.Emails
{
    public class MaterialEmailParameterResolverFactory : IMaterialEmailParameterResolverFactory
    {
        private readonly IEmailServiceHelper helper;

        public MaterialEmailParameterResolverFactory(IEmailServiceHelper helper)
        {
            this.helper = helper;
        }

        public IEmailParameterResolverWithBinding<ClassMaterialRecipient> CreateClassMaterialRecipientParameterResolver()
        {
            return new ClassMaterialRecipientParameterResolver(helper);
        }
    }
}
