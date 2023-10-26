using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Core.Emails.System.EmailParameterResolvers;
using System.Collections.Generic;

namespace AutomationSystem.Shared.Core.Emails.System
{
    // todo: move to share and make modular
    public class EmailEntityParameterResolverFactory : IEmailEntityParameterResolverFactory
    {
        private readonly IDictionary<EntityTypeEnum, IEmailEntityParameterResolverFactoryForEntity> entityMap;

        public EmailEntityParameterResolverFactory(IEnumerable<IEmailEntityParameterResolverFactoryForEntity> entityResolvers)
        {
            entityMap = new Dictionary<EntityTypeEnum, IEmailEntityParameterResolverFactoryForEntity>();
            SetEntityMap(entityResolvers);
        }

        public IEmailParameterResolver CreateResolverByEntity(EmailEntityId emailEntityId)
        {
            entityMap.TryGetValue(emailEntityId.TypeId, out var result);
            if (result == null)
            {
                return new EmptyParameterResolver();
            }
            
            return result.CreateParameterResolver(emailEntityId.Id);
        }

        #region private methods

        private void SetEntityMap(IEnumerable<IEmailEntityParameterResolverFactoryForEntity> resolvers)
        {
            foreach (var resolver in resolvers)
            {
                entityMap.Add(resolver.SupportedEntityType, resolver);
            }
        }

        #endregion
    }
}
