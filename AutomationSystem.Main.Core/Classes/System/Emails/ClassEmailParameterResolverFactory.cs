using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.System.Emails.EmailParameterResolvers;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.Classes.System.Emails
{
    public class ClassEmailParameterResolverFactory : IClassEmailParameterResolverFactory
    {
        private readonly IClassDatabaseLayer classDb;
        private readonly IEmailServiceHelper helper;
        private readonly IPersonDatabaseLayer personDb;
        private readonly ICoreEmailParameterResolverFactory coreParameterResolverFactory;

        public ClassEmailParameterResolverFactory(
            IClassDatabaseLayer classDb,
            IEmailServiceHelper helper,
            IPersonDatabaseLayer personDb,
            ICoreEmailParameterResolverFactory coreParameterResolverFactory)
        {
            this.classDb = classDb;
            this.helper = helper;
            this.personDb = personDb;
            this.coreParameterResolverFactory = coreParameterResolverFactory;
        }

        public IEmailParameterResolver CreateClassParameterResolver(Class cls)
        {
            var commonResolver = new MainCommonParameterResolver(helper);
            var classResolver = new ClassParameterResolver(helper, personDb, classDb);
            var result = coreParameterResolverFactory.CreateComposedParameterResolver(commonResolver, classResolver);

            classResolver.Bind(cls);
            return result;
        }
    }
}
