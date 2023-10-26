using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using PerfectlyMadeInc.WebEx.Connectors.AppLogic;
using PerfectlyMadeInc.WebEx.Connectors.AppLogic.Models;
using PerfectlyMadeInc.WebEx.Contract.Connectors;
using PerfectlyMadeInc.WebEx.Contract.Connectors.Models;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;
using PerfectlyMadeInc.WebEx.IntegrationStates.AppLogic;
using PerfectlyMadeInc.WebEx.IntegrationStates.Data;
using PerfectlyMadeInc.WebEx.Model;
using PerfectlyMadeInc.WebEx.Programs.Data;
using PerfectlyMadeInc.WebEx.Programs.Data.Models;

namespace PerfectlyMadeInc.WebEx.Connectors
{

    /// <summary>
    /// Program and event integration sync executor
    /// </summary>
    public class IntegrationSyncExecutor : IIntegrationSyncExecutor
    {

        private readonly IProgramDatabaseLayer programDb;
        private readonly IIntegrationDatabaseLayer integrationDb;
        private readonly IWebExFactory webExFactory;
        private readonly ITracerFactory tracerFactory;

        private readonly IConsistencyResolver consistencyResolver;
        private readonly IIntegrationStateConvertor integrationStateConvertor;
        private readonly IIntegrationStatePairingProvider pairingProvider;
        private readonly IWebExPersonConvertor webExPersonConvertor;


        // input parameters and object fields
        private bool wasInitialized;
        private List<IntegrationState> originSystemStates;                  // note: do not modify, use cloning function
        private ITracer tracer;

        private IIntegrationService service;


        // constructor
        public IntegrationSyncExecutor(IWebExFactory webExFactory, IProgramDatabaseLayer programDb, IIntegrationDatabaseLayer integrationDb, ITracerFactory tracerFactory)
        {
            this.webExFactory = webExFactory;
            this.programDb = programDb;
            this.integrationDb = integrationDb;
            this.tracerFactory = tracerFactory;

            integrationStateConvertor = new IntegrationStateConvertor(integrationDb);
            consistencyResolver = new ConsistencyResolver(integrationStateConvertor);
            pairingProvider = new IntegrationStatePairingProvider();
            webExPersonConvertor = new WebExPersonConvertor();
        }


        // initalizes input data before operation
        public void Initialize(List<IntegrationStateDto> _systemStates)
        {
            originSystemStates = _systemStates.Select(integrationStateConvertor.ConvertToIntegrationState).ToList();
            tracer = tracerFactory.CreateTracer<IntegrationSyncExecutor>(Guid.NewGuid());
            wasInitialized = true;
        }

        // executes sync for WebEx program
        public ProgramSyncResult ExecuteSyncForProgram(long programId)
        {
            if (!wasInitialized)
                throw new InvalidOperationException("IntegrationSyncExecutor must be initialized first.");

            // loads program 
            var program = programDb.GetProgramById(programId, ProgramIncludes.Events);
            if (program == null)
                throw new ArgumentException($"There is no Program with id {programId}");

            // process program
            tracer.Info($"Processing of program with {programId}.");
            var result = new ProgramSyncResult
            {
                ProgramId = programId,
                ProgramName = program.Name
            };
            try
            {
                // initializes WebEx service and process all events
                service = webExFactory.CreateIntegrationService(program.AccountId);
                foreach (var webExEvent in program.Events)
                {
                    var eventResult = SyncEvent(webExEvent);
                    result.EventResults.Add(eventResult);
                }
            }
            catch (Exception e)
            {
                tracer.Error(e, $"Processin of program {programId} causes exception");
                result.Exception = e;
            }
            return result;
        }

        // executes sync WebEx event
        public EventSyncResult ExecuteSyncForEvent(long eventId)
        {
            throw new NotImplementedException("This functionality is not implemented yet.");
        }


        #region event workflow

        // synchronizes event
        private EventSyncResult SyncEvent(Event webExEvent)
        {
            var sessionId = webExEvent.SessionId;
            var eventId = webExEvent.EventId;
            tracer.Info($"Processing of event {eventId}.");
            
            var result = new EventSyncResult
            {
                EventId = webExEvent.EventId,
                EventName = webExEvent.Name
            };
            try
            {
                // Stage 1 - sync of webex stage 1
                tracer.Info("Stage 1 - synchronizing of WebEx");

                // loads db states, joins and finds email duplicities
                var states = CloneAndEnrichInput(eventId);
                result.DuplicitEmails = GetRemoveDuplicitEmails(states)
                    .ToDictionary(
                        x => x.Key,
                        y => y.Value.Select(integrationStateConvertor.ConvertToIntegrationStateDto).ToList());                                               

                // loads WebEx states, find inconsistencies and executes sync operations, updates attendeeIds
                var currentConsistencyResults = ResolveSyncStrategy(states, sessionId);
                result.SyncActions = SyncSystemWithWebEx(currentConsistencyResults, sessionId);

                // Stage 2 - updating of integration state 
                tracer.Info("Stage 2 - updating of integratino state");
               
                // gets WebEx state pairs and computes unknown in webex and inconsistent states
                var systemWebExPairs = GetWebExStatePairs(states, sessionId);               
                result.UnknownInWebEx = systemWebExPairs.Unassigned.Select(integrationStateConvertor.ConvertToIntegrationStateDto).ToList();
                result.Inconsistent = systemWebExPairs.Pairs
                    .Select(x => consistencyResolver.Compare(x.Item1, x.Item2))
                    .Where(x => x.IsInconsistent).ToList();                
             
                // perpares states for saving to database, set errors to result and save states to db
                var statesToSave = GetStatesToSave(systemWebExPairs.Pairs, eventId);                
                result.ErrorStates = statesToSave
                    .Where(x => x.IntegrationStateTypeId == IntegrationStateTypeEnum.Error)
                    .Select(integrationStateConvertor.ConvertToIntegrationStateDto)
                    .ToList();
                SaveStates(statesToSave);               
                
                // writes event sync summary
                tracer.Info($"Event {eventId} sync finished:\n- duplicit emails {result.DuplicitEmails.Count}\n- sync actions {result.SyncActions.Count}\n" +
                    $"- unknown in WebEx {result.UnknownInWebEx.Count}\n- inconsistent {result.Inconsistent.Count}\n- error states {result.ErrorStates.Count}");
            }
            catch (Exception e)
            {
                tracer.Error(e, $"Processin of event {eventId} causes exception");
                result.Exception = e;
            }
            return result;
        }

        

        #endregion

        #region steps

        // Cloning and Enrichment of input
        private List<IntegrationState> CloneAndEnrichInput(long eventId)
        {
            // gets db states and clones system states enriched with attendeeId and integrationStateId
            // note: cloning is important because systemStates input can be used more time for more WebEx event!
            var dbStates = integrationDb.GetIntegrationStatesByEventId(eventId);
            var dbSystemPairs = pairingProvider.PairByEntity(originSystemStates, dbStates);
            var result = dbSystemPairs.Pairs.Select(
                x => integrationStateConvertor.CloneIntegrationState(x.Item1, x.Item2?.AttendeeId, x.Item2?.IntegrationStateId)).ToList();
            return result;
        }


        // removes duplicit emails
        private Dictionary<string, List<IntegrationState>> GetRemoveDuplicitEmails(List<IntegrationState> states)
        {         
            // finds out duplicit emails
            var result = states
                .Where(x => x.IntegrationStateTypeId == IntegrationStateTypeEnum.InWebEx)
                .GroupBy(x => x.Email.ToLower().Trim())
                .Where(x => x.Count() > 1)
                .ToDictionary(x => x.Key, y => y.ToList());  
            
            // removes duplicit emails, keeps only first one
            var toRemove = result.Values.SelectMany(x => x.Skip(1)).ToList();
            toRemove.ForEach(x => states.Remove(x));
            
            return result;
        }


        // loads WebEx state of event, join it with system states and resolve consistecy issues
        private List<ConsistencyResult> ResolveSyncStrategy(List<IntegrationState> systemStates, long sessionId)
        {
            // loads current state of WebEx
            var webExStates = service.GetWebExStatesBySessionId(sessionId);

            // ignores not in webex without attendeeId
            var systemStatesWithoutClosedWithNoAttendeeId = systemStates.Where(x =>
                x.IntegrationStateTypeId != IntegrationStateTypeEnum.NotInWebEx || x.AttendeeId.HasValue).ToList();

            // joins webex states with system states by attendeeId (or email for InSystem states),           
            // substitute unknown with NotInWebEx states, attendeeId can be 0 because in webex states going to be added into WebEx and obtain new attendeeId      
            var systemWebExPairs = pairingProvider.PairByAttendeeIdOrEmailForInSystem(systemStatesWithoutClosedWithNoAttendeeId, webExStates,
                x => webExPersonConvertor.ConvertToEmptyIntegrationState(x.AttendeeId));                           

            // for email joining cases, attendeeId is updated for system states
            foreach (var pairs in systemWebExPairs.Pairs.Where(x => !x.Item1.AttendeeId.HasValue))
                pairs.Item1.AttendeeId = pairs.Item2.AttendeeId;

            // resolve consistency results
            var result = systemWebExPairs.Pairs.Select(x => consistencyResolver.Compare(x.Item1, x.Item2)).ToList();
            return result;
        }


        // syncs system and webex state, returns SyncActionResults, !updates attendeeId of system state!
        private List<SyncActionResult> SyncSystemWithWebEx(List<ConsistencyResult> consistencyResults, long sessionId)
        {
            // process all sync operations excluding None, updates attendeeId of system state
            var result = new List<SyncActionResult>();
            foreach (var syncItem in consistencyResults.Where(x => x.OperationType != SyncOperationType.None))
            {
                var actionResult = new SyncActionResult(syncItem);
                var systemState = syncItem.SystemState;
                try
                {
                    // executes operation
                    switch (syncItem.OperationType)
                    {
                        case SyncOperationType.Save:
                            tracer.Info($"Executes AddWebExPerson sync operation on {systemState.EntityTypeId}({systemState.EntityId}).");
                            var addResult = service.AddUpdatePerson(
                                integrationStateConvertor.ConvertToIntegrationState(syncItem.SystemState),
                                integrationStateConvertor.ConvertToIntegrationState(syncItem.WebExState),
                                sessionId);
                            if (addResult.IsSuccess)
                                systemState.AttendeeId = addResult.Result;
                            else
                                actionResult.Exception = addResult.Exception;
                            break;

                        case SyncOperationType.Remove:
                            tracer.Info($"Executes RemoveWebExPerson sync operation on on {systemState.EntityTypeId}({systemState.EntityId}).");
                            var removeResult = service.RemovePerson(syncItem.SystemState.AttendeeId ?? 0);
                            if (!removeResult.IsSuccess)
                                actionResult.Exception = removeResult.Exception;
                            break;
                    }
                }
                catch (Exception e)
                {
                    tracer.Error(e, $"Sync action on {systemState.EntityTypeId}({systemState.EntityId}) causes exception.");
                    actionResult.Exception = e;
                }
                result.Add(actionResult);
            }
            return result;
        }


        // gets WebEx state pairs
        private PairingResult<IntegrationState> GetWebExStatePairs(List<IntegrationState> states, long sessionId)
        {
            // loads current state of WebEx
            var webExStates = service.GetWebExStatesBySessionId(sessionId);

            // joins webex states with system states by attendeeId, 
            // substitute unknown with NotInWebEx states 
            // substitute unknown with no attendeeId with error state
            var result = pairingProvider.PairByAttendeeId(states, webExStates,
                x => x.AttendeeId.HasValue
                    ? webExPersonConvertor.ConvertToEmptyIntegrationState(x.AttendeeId.Value)
                    : webExPersonConvertor.ConvertToErrorIntegrationState(null, x.IntegrationStateTypeId == IntegrationStateTypeEnum.InWebEx
                        ? "Invalid operation - attendeeId should be known when sync action for InWebEx state was executed."
                        : WebExTextHelper.AttendeeIdMissingMessage));

            return result;
        }


        // prepares integrations states to insert or update
        private List<IntegrationState> GetStatesToSave(List<Tuple<IntegrationState, IntegrationState>> systemWebExPairs, long eventId)
        {
            // prepares states for saving to database
            var result = new List<IntegrationState>();
            var now = DateTime.Now;
            foreach (var pair in systemWebExPairs)
            {
                // gets webex states and fills entity and other relation from the system state
                var systemState = pair.Item1;
                var webExState = pair.Item2;
                integrationStateConvertor.FillIntegrationState(webExState, systemState.IntegrationStateId,
                    systemState.EntityTypeId, systemState.EntityId, eventId);
                webExState.LastChecked = now;
                result.Add(webExState);
            }

            return result;
        }


        // saves integration states
        private void SaveStates(List<IntegrationState> statesToSave)
        {
            // updates integration states in database
            foreach (var stateToSave in statesToSave)
            {
                if (stateToSave.IntegrationStateId == 0)
                    integrationDb.InsertIntegrationState(stateToSave);
                else
                    integrationDb.UpdateIntegrationState(stateToSave);
            }
        }

        #endregion



    }

}
