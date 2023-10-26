using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.WebEx.Connectors.AppLogic;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;
using PerfectlyMadeInc.WebEx.IntegrationStates.AppLogic;
using PerfectlyMadeInc.WebEx.IntegrationStates.Data;
using PerfectlyMadeInc.WebEx.Programs.Data;
using PerfectlyMadeInc.WebEx.Programs.Data.Models;

namespace PerfectlyMadeInc.WebEx.IntegrationStates
{

    /// <summary>
    /// Provides compex operations with WebEx for integration administration pages
    /// </summary>
    public class IntegrationAdministration : IIntegrationAdministration
    {

       


        private readonly IProgramDatabaseLayer programDb;
        private readonly IIntegrationDatabaseLayer integrationDb;

        private readonly IIntegrationStateConvertor integrationConvertor;
        private readonly IConsistencyResolver consistecyResolver;       
        private readonly IWebExPersonConvertor webExPersonConvertor;


        // constructor
        public IntegrationAdministration(IProgramDatabaseLayer programDb, IIntegrationDatabaseLayer integrationDb)
        {
            this.programDb = programDb;
            this.integrationDb = integrationDb;
            integrationConvertor = new IntegrationStateConvertor(integrationDb);
            consistecyResolver = new ConsistencyResolver(integrationConvertor);           
            webExPersonConvertor = new WebExPersonConvertor();
        }


        // gets web ex integration state summary
        public List<IntegrationStateSummary> GetIntegrationStateSummary(IntegrationStateDto systemState, EntityTypeEnum entityTypeId, long entityId, long programId)
        {
            var systemStateConverted = integrationConvertor.ConvertToIntegrationState(systemState);
            var programAndEvents = programDb.GetProgramById(programId, ProgramIncludes.Events);
            var stateByEventId = integrationDb.GetIntegrationStatesByEntityId(entityTypeId, entityId)
                .ToDictionary(x => x.EventId, y => y);


            // resolves state for each event
            var result = new List<IntegrationStateSummary>();
            foreach (var webExEvent in programAndEvents.Events)
            {
                // gets webExState
                if (!stateByEventId.TryGetValue(webExEvent.EventId, out var webExState))
                    webExState = webExPersonConvertor.ConvertToErrorIntegrationState(null, WebExTextHelper.AttendeeIdMissingMessage);

                // compare states and converts to integration state detail
                var consistencySummary = consistecyResolver.Compare(systemStateConverted, webExState);
                var stateSummary = integrationConvertor.ConvertToIntegrationStateSummary(consistencySummary, webExEvent.Name);
                result.Add(stateSummary);
            }
            return result;
        }

    }

}
