using AutomationSystem.Main.Core.Contacts.System.Emails.EmailParameterResolvers;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.Contacts.System.Emails
{
    public class ContactEmailParameterResolverFactory : IContactEmailParameterResolverFactory
    {
        private readonly IEmailServiceHelper helper;

        public ContactEmailParameterResolverFactory(IEmailServiceHelper helper)
        {
            this.helper = helper;
        }

        public IEmailParameterResolverWithBinding<FormerStudent> CreateFormerStudentParameterResolver()
        {
            return new FormerStudentParameterResolver(helper);
        }
    }
}
