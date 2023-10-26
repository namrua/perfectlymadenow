using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.EntityIntegration.System.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.EntityIntegration.System
{
    /// <summary>
    /// Provides integration of some entity (e.g. class) with many of integration entities (e.g. webexProgram)
    /// </summary>
    public interface IGenericEntityIntegrationProvider
    {

        // gets integration type by id
        IntegrationType GetIntegrationTypeById(IntegrationTypeEnum integrationTypeId);

        // gets list of tuples <code, integration entity name>
        List<Tuple<long, string>> GetActiveIntegrationEntities(long? currentIntegrationCode, long profileId);

        // gets integration relation from integration code
        EntityIntegrationRelation GetIntegrationRelationByCode(long? integrationCode);

        // gets code form integration type and entityId
        long GetIntegrationCode(IntegrationTypeEnum integrationTypeId, long? integrationEntityId);

        // gets integration entity name
        string GetIntegrationEntityName(IntegrationTypeEnum integrationTypeId, long? integrationEntityId);

        // attach entity to integration entity
        void AttachEntity(EntityTypeEnum entityTypeId, long entityId, IntegrationTypeEnum integrationTypeId, long? integrationEntityId, bool detachOthers);

        // detach entity from integration entity
        void DetachEntity(EntityTypeEnum entityTypeId, long entityId);

    }

}
