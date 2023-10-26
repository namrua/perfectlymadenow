using AutomationSystem.Main.Core.Classes.System.Emails;
using AutomationSystem.Main.Core.Registrations.System.Emails.EmailParameterResolvers;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.Registrations.System.Emails
{
    public class RegistrationEmailParameterResolverFactory : IRegistrationEmailParameterResolverFactory
    {
        private readonly IEmailServiceHelper helper;
        private readonly IClassEmailParameterResolverFactory classEmailParameterResolverFactory;
        private readonly ICoreEmailParameterResolverFactory coreParameterResolverFactory;

        public RegistrationEmailParameterResolverFactory(
            IEmailServiceHelper helper,
            IClassEmailParameterResolverFactory classEmailParameterResolverFactory,
            ICoreEmailParameterResolverFactory coreParameterResolverFactory)
        {
            this.helper = helper;
            this.classEmailParameterResolverFactory = classEmailParameterResolverFactory;
            this.coreParameterResolverFactory = coreParameterResolverFactory;
        }

        public IEmailParameterResolver CreateRegistrationParameterResolver(ClassRegistration registration)
        {
            var registrationResolver = new ClassRegistrationParameterResolver(helper);
            var classResolver = classEmailParameterResolverFactory.CreateClassParameterResolver(registration.Class);
            var lastClassResolver = new LastClassParameterResolver(helper);
            var result = coreParameterResolverFactory.CreateComposedParameterResolver(registrationResolver, classResolver, lastClassResolver);
            registrationResolver.Bind(registration);
            lastClassResolver.Bind(registration);
            return result;
        }

        public IEmailParameterResolverWithBinding<ClassRegistration> CreateRegistrationParameterResolver()
        {
            return new ClassRegistrationParameterResolver(helper);
        }

        public IEmailParameterResolverWithBinding<Person> CreatePersonAsRegistrationParameterResolver()
        {
            return new PersonAsRegistrationParameterResolver(helper);
        }

        public IEmailParameterResolverWithBinding<ClassRegistrationInvitation> CreateRegistrationInvitationParameterResolver()
        {
            return new ClassRegistrationInvitationParameterResolver(helper);
        }
    }
}
