using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Emails.System.Models;
using System.Collections.Generic;

namespace AutomationSystem.Shared.Core.Emails.System
{
    public class EmailTemplateHierarchyResolver : IEmailTemplateHierarchyResolver
    {
        private readonly IDictionary<int, IEmailTemplateHierarchyResolverForEntity> entityMap;

        public EmailTemplateHierarchyResolver(IEnumerable<IEmailTemplateHierarchyResolverForEntity> resolvers)
        {
            entityMap = new Dictionary<int, IEmailTemplateHierarchyResolverForEntity>();
            SetEntityMap(resolvers);
        }

        public EmailTemplateEntityHierarchy GetHierarchyForParent(EmailTemplateEntityId emailTemplateEntityId)
        {
            var result = GetResolver(emailTemplateEntityId.TypeId).GetHierarchyForParent(emailTemplateEntityId.Id);
            return result;
        }

        public EmailTemplateEntityHierarchy GetHierarchy(EmailTemplateEntityId emailTemplateEntityId)
        {
            var result = GetResolver(emailTemplateEntityId.TypeId).GetHierarchy(emailTemplateEntityId.Id);
            return result;
        }

        #region private methods

        private IEmailTemplateHierarchyResolverForEntity GetResolver(EntityTypeEnum? entityTypeId)
        {
            entityMap.TryGetValue(GetResolverKey(entityTypeId), out var result);
            if (result == null)
            {
                throw new InvalidOperationException($"There is no resolver for entity type {entityTypeId}.");
            }

            return result;
        }

        private void SetEntityMap(IEnumerable<IEmailTemplateHierarchyResolverForEntity> resolvers)
        {
            foreach (var resolver in resolvers)
            {
                entityMap.Add(GetResolverKey(resolver.EntityTypeId), resolver);
            }
        }

        private int GetResolverKey(EntityTypeEnum? entityTypeId)
        {
            if (!entityTypeId.HasValue)
            {
                return 0;
            }

            return (int) entityTypeId;
        }

        #endregion
    }
}
