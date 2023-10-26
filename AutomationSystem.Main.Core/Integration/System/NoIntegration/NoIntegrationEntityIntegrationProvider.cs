using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Integration;

namespace AutomationSystem.Main.Core.Integration.System.NoIntegration
{
    /// <summary>
    /// Provides no integration of entity
    /// </summary>
    public class NoIntegrationEntityIntegrationProvider : IEntityIntegrationProvider
    {
        public IntegrationTypeEnum Type => IntegrationTypeEnum.NoIntegration;

        public List<Tuple<long, string>> GetActiveIntegrationEntities(long? currentIntegrationEntity, UserGroupTypeEnum? userGroupTypeId, long? userGroupId)
        {
            var result = new List<Tuple<long, string>>();
            result.Add(new Tuple<long, string>(0, "No integration"));
            return result;
        }

        public string GetIntegrationEntityNameById(long? integrationEntityId)
        {
            return null;
        }

        public void AttachEntity(EntityTypeEnum entityTypeId, long entityId, long? integrationEntityId, bool detachOthers)
        {
        }

        public void DetachEntity(EntityTypeEnum entityTypeId, long entityId)
        {
        }
    }
}
