using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Integration.System.Models;

namespace AutomationSystem.Main.Core.Integration.System
{
    public interface IIntegrationDataProvider
    {
        IntegrationTypeEnum IntegrationTypeId { get; }
        List<EventLocationInfo> GetEventLocationInfo(long? integrationEntityId);
    }
}
