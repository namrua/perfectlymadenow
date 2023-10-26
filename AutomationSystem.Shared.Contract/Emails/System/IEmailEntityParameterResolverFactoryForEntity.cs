using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Emails.System
{
    public interface IEmailEntityParameterResolverFactoryForEntity
    {
        EntityTypeEnum SupportedEntityType { get; }

        IEmailParameterResolver CreateParameterResolver(long entityId);

    }
}
