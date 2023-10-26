using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Integration;
using AutomationSystem.Shared.Contract.EntityIntegration.System;
using AutomationSystem.Shared.Contract.EntityIntegration.System.Models;
using AutomationSystem.Shared.Core.EntityIntegration.Data;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.Helpers.Contract.Routines;
using PerfectlyMadeInc.Helpers.Routines;

namespace AutomationSystem.Shared.Core.EntityIntegration.System
{
    /// <summary>
    /// Provides integration of some entity (e.g. class) with multiplatform of integration entity (e.g. webexProgram)
    /// </summary>
    public class GenericEntityIntegrationProvider : IGenericEntityIntegrationProvider
    {

        private readonly IIntegrationDatabaseLayer integrationDb;

        private readonly IRelationCoder<IntegrationTypeEnum, long?> integrationRelationCoder;

        private readonly Dictionary<IntegrationTypeEnum, IEntityIntegrationProvider> integrationProviders;


        // constructor
        public GenericEntityIntegrationProvider(
            IIntegrationDatabaseLayer integrationDb,
            IEnumerable<IEntityIntegrationProvider> entityIntegrationProviders)
        {
            this.integrationDb = integrationDb;
            integrationProviders = new Dictionary<IntegrationTypeEnum, IEntityIntegrationProvider>();
            RegisterIntegrationProviders(entityIntegrationProviders);

            // utilizes relation coder for coding and encoding of integration relations
            integrationRelationCoder = new RelationCoder<IntegrationTypeEnum, long?>(10000,
                l => (IntegrationTypeEnum)l, f => (long)f,
                l => l == 0 ? (long?)null : l, s => s ?? 0);
        }

        // gets integration type by id
        public IntegrationType GetIntegrationTypeById(IntegrationTypeEnum integrationTypeId)
        {
            var result = integrationDb.GetIntegrationTypeById(integrationTypeId);
            return result;
        }


        // gets list of tuples <code, integration entity name>
        public List<Tuple<long, string>> GetActiveIntegrationEntities(long? currentIntegrationCode, long profileId)
        {            
            var result = new List<Tuple<long, string>>();
            var currentRelation = GetRelationFromCode(currentIntegrationCode ?? 0);
            foreach (var provider in integrationProviders.Values)
            {
                // if currently selected integration type is in the currently processed integration type, 
                // sets currentEntityId to IntegrationEntityId (for all other type current entity is null)
                var currentEntityId = provider.Type == currentRelation.IntegrationTypeId ? currentRelation.IntegrationEntityId : null;

                // gets active entities and transform them to <code(type, id), name> format
                var activeEntities = provider.GetActiveIntegrationEntities(currentEntityId, UserGroupTypeEnum.MainProfile, profileId);
                var entitiesForResult = activeEntities.Select(x => new Tuple<long, string>(GetCode(provider.Type, x.Item1), x.Item2)).ToList();

                // adds to result range
                result.AddRange(entitiesForResult);
            }
            return result;
        }


        // gets integration relation from integration code
        public EntityIntegrationRelation GetIntegrationRelationByCode(long? integrationCode)
        {
            // todo: it is posible to add some consistency validation here
            var result = GetRelationFromCode(integrationCode ?? 0);
            return result;
        }


        // gets code form integration type and entityId
        public long GetIntegrationCode(IntegrationTypeEnum integrationTypeId, long? integrationEntityId)
        {
            var result = GetCode(integrationTypeId, integrationEntityId);
            return result;
        }


        // gets integration entity name
        public string GetIntegrationEntityName(IntegrationTypeEnum integrationTypeId, long? integrationEntityId)
        {
            var provider = GetProvider(integrationTypeId);
            var result = provider.GetIntegrationEntityNameById(integrationEntityId);
            return result;
        }


        // attach entity to integration entity
        public void AttachEntity(EntityTypeEnum entityTypeId, long entityId, IntegrationTypeEnum integrationTypeId,
            long? integrationEntityId, bool detachOthers)
        {
            var provider = GetProvider(integrationTypeId);

            // detaches other entities
            if (detachOthers)
            {
                foreach (var otherProvider in integrationProviders.Where(x => x.Key != integrationTypeId))
                    otherProvider.Value.DetachEntity(entityTypeId, entityId);
            }

            // attach to determined provider
            provider.AttachEntity(entityTypeId, entityId, integrationEntityId, detachOthers);            
        }


        // detach entity from integration entity
        public void DetachEntity(EntityTypeEnum entityTypeId, long entityId)
        {
            foreach(var provider in integrationProviders.Values)
                provider.DetachEntity(entityTypeId, entityId);
        }

        #region private methods

        // gets provider by type
        private IEntityIntegrationProvider GetProvider(IntegrationTypeEnum typeId)
        {
            if (!integrationProviders.TryGetValue(typeId, out var result))
                throw new InvalidOperationException($"There is no entity integration provider for IntegrationType {typeId}.");
            return result;
        }

        // registers integration provider
        private void RegisterIntegrationProviders(IEnumerable<IEntityIntegrationProvider> entityIntegrationProviders)
        {
            foreach (var entityIntegrationProvider in entityIntegrationProviders)
            {
                integrationProviders.Add(entityIntegrationProvider.Type, entityIntegrationProvider);
            }
        }

        #endregion


        #region private methods - encoding

        // gets relation code 
        private long GetCode(IntegrationTypeEnum integrationTypeId, long? integrationEntityId)
        {
            var result = integrationRelationCoder.GetCode(integrationTypeId, integrationEntityId);
            return result;
        }

        // fills integration relation
        private EntityIntegrationRelation GetRelationFromCode(long code)
        {
            var result = new EntityIntegrationRelation();
            result.IntegrationTypeId = integrationRelationCoder.GetFirst(code);
            result.IntegrationEntityId = integrationRelationCoder.GetSecond(code);
            return result;
        }

        #endregion

    }

}
