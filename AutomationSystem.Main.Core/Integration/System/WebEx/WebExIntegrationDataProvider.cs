using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Integration.System.Models;
using PerfectlyMadeInc.WebEx.Contract.Programs;

namespace AutomationSystem.Main.Core.Integration.System.WebEx
{
    public class WebExIntegrationDataProvider : IIntegrationDataProvider
    {
        private readonly IEventDataProvider eventDataProvider;

        public IntegrationTypeEnum IntegrationTypeId => IntegrationTypeEnum.WebExProgram;

        public WebExIntegrationDataProvider(IEventDataProvider eventDataProvider)
        {
            this.eventDataProvider = eventDataProvider;
        }

        public List<EventLocationInfo> GetEventLocationInfo(long? integrationEntityId)
        {
            if (!integrationEntityId.HasValue)
            {
                throw new ArgumentNullException(nameof(integrationEntityId));
            }

            var events = eventDataProvider.GetEventInfoByProgramId(integrationEntityId.Value);
            var result = events.Select(
                 x => new EventLocationInfo
                 {
                     EventName = x.Name,
                     EventLocation = x.SessionId.ToString()
                 }).ToList();

            return result;
        }
    }
}
