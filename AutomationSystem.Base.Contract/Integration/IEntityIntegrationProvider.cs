using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Base.Contract.Integration
{

    /// <summary>
    /// Encapsulates class integration logic for integration entities of one type
    /// </summary>
    public interface IEntityIntegrationProvider
    {

        // gets integratino type
        IntegrationTypeEnum Type { get; }

        // gets list of tuples <integration entityId, integration entity name>
        List<Tuple<long, string>> GetActiveIntegrationEntities(long? currentIntegrationEntity, UserGroupTypeEnum? userGroupTypeId, long? userGroupId);

        // gets integration entity name
        string GetIntegrationEntityNameById(long? integrationEntityId);

        // attach entity to integration entity
        void AttachEntity(EntityTypeEnum entityTypeId, long entityId, long? integrationEntityId, bool detachOthers);

        // detach entity from integration entity
        void DetachEntity(EntityTypeEnum entityTypeId, long entityId);

    }

}
