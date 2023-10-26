using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.Contacts.System.Emails
{
    public interface IContactEmailParameterResolverFactory
    {
        IEmailParameterResolverWithBinding<FormerStudent> CreateFormerStudentParameterResolver();
    }
}
