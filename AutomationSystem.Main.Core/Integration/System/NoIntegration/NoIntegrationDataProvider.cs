using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Integration.System.Models;

namespace AutomationSystem.Main.Core.Integration.System.NoIntegration
{
    public class NoIntegrationDataProvider : IIntegrationDataProvider
    {
        public IntegrationTypeEnum IntegrationTypeId => IntegrationTypeEnum.NoIntegration;
        public List<EventLocationInfo> GetEventLocationInfo(long? integrationEntityId)
        {
            return new List<EventLocationInfo>();
        }
    }
}
