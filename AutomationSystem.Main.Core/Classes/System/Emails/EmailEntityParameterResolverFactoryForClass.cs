using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.Classes.System.Emails
{
    public class EmailEntityParameterResolverFactoryForClass : IEmailEntityParameterResolverFactoryForEntity
    {
        private readonly IClassDatabaseLayer classDb;
        private readonly IClassEmailParameterResolverFactory classEmailParameterResolverFactory;

        public EntityTypeEnum SupportedEntityType => EntityTypeEnum.MainClass;

        public EmailEntityParameterResolverFactoryForClass(
            IClassDatabaseLayer classDb,
            IClassEmailParameterResolverFactory classEmailParameterResolverFactory)
        {
            this.classDb = classDb;
            this.classEmailParameterResolverFactory = classEmailParameterResolverFactory;
        }

        public IEmailParameterResolver CreateParameterResolver(long entityId)
        {
            var cls = classDb.GetClassById(entityId);
            if (cls == null)
            {
                throw new ArgumentException($"There is no Class with id {entityId}.");
            }

            var result = classEmailParameterResolverFactory.CreateClassParameterResolver(cls);
            return result;
        }
    }
}
