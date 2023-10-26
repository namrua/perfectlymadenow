using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Integration.System.Models;

namespace AutomationSystem.Main.Core.Integration.System
{
    public class GenericIntegrationDataProvider : IGenericIntegrationDataProvider
    {
        private readonly Dictionary<IntegrationTypeEnum, IIntegrationDataProvider> dataProviders;

        public GenericIntegrationDataProvider(IEnumerable<IIntegrationDataProvider> integrationDataProviders)
        {
            dataProviders = new Dictionary<IntegrationTypeEnum, IIntegrationDataProvider>();
            RegisterIntegrationDataProviders(integrationDataProviders);
        }

        public List<EventLocationInfo> GetEventLocationInfo(IntegrationTypeEnum integrationTypeId, long? integrationEntityId)
        {
            var result = GetProvider(integrationTypeId).GetEventLocationInfo(integrationEntityId);
            return result;
        }

        #region private methods

        private IIntegrationDataProvider GetProvider(IntegrationTypeEnum typeId)
        {
            if (!dataProviders.TryGetValue(typeId, out var result))
            {
                throw new InvalidOperationException($"There is no entity integration data provider for IntegrationType {typeId}.");
            }

            return result;
        }

        private void RegisterIntegrationDataProviders(IEnumerable<IIntegrationDataProvider> integrationDataProviders)
        {
            foreach (var integrationDataProvider in integrationDataProviders)
            {
                dataProviders.Add(integrationDataProvider.IntegrationTypeId, integrationDataProvider);
            }
        }

        #endregion
    }
}
