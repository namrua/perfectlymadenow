using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.MaterialDistribution.AppLogic.MaterialRecipientIntegrations;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic
{
    /// <summary>
    /// Provides material recipient integration
    /// </summary>
    public class MaterialRecipientIntegrationProvider : IMaterialRecipientIntegrationProvider
    {
        private readonly IDictionary<EntityTypeEnum, IMaterialRecipientIntegration> integrationsMap;

        public MaterialRecipientIntegrationProvider(IEnumerable<IMaterialRecipientIntegration> materialRecipientIntegrations)
        {
            integrationsMap = materialRecipientIntegrations.ToDictionary(x => x.TypeId);
        }

        public List<IMaterialRecipientIntegration> GetAllRecipientIntegratons()
        {
            return integrationsMap.Values.ToList();
        }

        public IMaterialRecipientIntegration GetRecipientIntegrationByTypeId(EntityTypeEnum recipientTypeId)
        {
            if (!integrationsMap.TryGetValue(recipientTypeId, out var result))
            {
                throw new ArgumentException($"There is no material recipient integration for recipient type {recipientTypeId}.");
            }

            return result;
        }
    }
}
