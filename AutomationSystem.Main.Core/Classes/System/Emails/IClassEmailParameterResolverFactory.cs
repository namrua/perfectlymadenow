using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.Classes.System.Emails
{
    public interface IClassEmailParameterResolverFactory
    {
        IEmailParameterResolver CreateClassParameterResolver(Class cls);
    }
}
