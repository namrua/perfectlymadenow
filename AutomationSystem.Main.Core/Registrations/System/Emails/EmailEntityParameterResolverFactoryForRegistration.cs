using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.Registrations.System.Emails
{
    public class EmailEntityParameterResolverFactoryForRegistration : IEmailEntityParameterResolverFactoryForEntity
    {
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IRegistrationEmailParameterResolverFactory registrationEmailParameterResolverFactory;

        public EntityTypeEnum SupportedEntityType => EntityTypeEnum.MainClassRegistration;

        public EmailEntityParameterResolverFactoryForRegistration(
            IRegistrationDatabaseLayer registrationDb,
            IRegistrationEmailParameterResolverFactory registrationEmailParameterResolverFactory)
        {
            this.registrationDb = registrationDb;
            this.registrationEmailParameterResolverFactory = registrationEmailParameterResolverFactory;
        }

        public IEmailParameterResolver CreateParameterResolver(long entityId)
        {
            var registration = registrationDb.GetClassRegistrationById(entityId,
                ClassRegistrationIncludes.Class | ClassRegistrationIncludes.Addresses | ClassRegistrationIncludes.ClassRegistrationLastClass);
            if (registration == null)
            {
                throw new ArgumentException($"There is no Class registration with id {entityId}.");
            }

            var result = registrationEmailParameterResolverFactory.CreateRegistrationParameterResolver(registration);
            return result;
        }
    }
}
