using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.Registrations.System.Emails
{
    public interface IRegistrationEmailParameterResolverFactory
    {
        IEmailParameterResolver CreateRegistrationParameterResolver(ClassRegistration registration);

        IEmailParameterResolverWithBinding<ClassRegistration> CreateRegistrationParameterResolver();

        IEmailParameterResolverWithBinding<Person> CreatePersonAsRegistrationParameterResolver();
        
        IEmailParameterResolverWithBinding<ClassRegistrationInvitation> CreateRegistrationInvitationParameterResolver();
    }
}
