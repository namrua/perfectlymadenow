using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Integration.System.Models;

namespace AutomationSystem.Main.Core.Integration.System
{
    public interface IGenericIntegrationDataProvider
    {
        List<EventLocationInfo> GetEventLocationInfo(IntegrationTypeEnum integrationTypeId, long? integrationEntityId);
    }
}
