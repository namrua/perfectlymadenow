using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;

namespace AutomationSystem.Shared.Contract.Emails.System
{
    public interface IEmailEntityParameterResolverFactory
    {
        IEmailParameterResolver CreateResolverByEntity(EmailEntityId emailEntityId);
    }
}
