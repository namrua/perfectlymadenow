using System;
using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using PerfectlyMadeInc.WebEx.Connectors.AppLogic;
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
    /// Program and event integration request handler
    /// </summary>
    public class IntegrationRequestHandler : IIntegrationRequestHandler
    {
       
        private readonly IProgramDatabaseLayer programDb;
        private readonly IIntegrationDatabaseLayer integrationDb;
        private readonly IWebExFactory webExFactory;
        private readonly ITracerFactory tracerFactory;
        
        private readonly IConsistencyResolver consistencyResolver;
        private readonly IIntegrationStateConvertor integrationStateConvertor;

        // input parameters and object fields  
        private bool wasInitialized;
        private EntityTypeEnum entityTypeId;
        private long entityId;
        private AsyncRequestTypeEnum eventTypeId;
        private IntegrationState systemState;        
        private ITracer tracer;

        private IIntegrationService service;


        //constructor
        public IntegrationRequestHandler(IWebExFactory webExFactory, IProgramDatabaseLayer programDb, IIntegrationDatabaseLayer integrationDb, ITracerFactory tracerFactory)
        {           
            this.webExFactory = webExFactory;
            this.programDb = programDb;
            this.integrationDb = integrationDb;
            this.tracerFactory = tracerFactory;
            
            integrationStateConvertor = new IntegrationStateConvertor(integrationDb);
            consistencyResolver = new ConsistencyResolver(integrationStateConvertor);
        }


        // initializes input data before operation
        public void Initialize(AsyncRequestTypeEnum _eventTypeId, IntegrationStateDto _systemState)
        {
            eventTypeId = _eventTypeId;
            systemState = integrationStateConvertor.ConvertToIntegrationState(_systemState);
            entityTypeId = _systemState.EntityTypeId;
            entityId = systemState.EntityId;
            tracer = tracerFactory.CreateTracer<IntegrationRequestHandler>($"{entityTypeId}({entityId})");
            wasInitialized = true;
        }


        // handles integration event for WebEx program
        public void HandleForProgram(long programId)
        {
            if (!wasInitialized)
                throw new InvalidOperationException("IntegrationRequestHandler must be initialized first.");

            // loads program 
            var program = programDb.GetProgramById(programId, ProgramIncludes.Events);
            if (program == null)
                throw new ArgumentException($"There is no Program with id {programId}");

            // initializes WebEx service and process all events
            service = webExFactory.CreateIntegrationService(program.AccountId);
            foreach (var webExEvent in program.Events)
            {
                ProcessEvent(webExEvent);
            }
        }

        // handles integration event for WebEx event
        public void HandleForEvent(long eventId)
        {
            throw new NotImplementedException("This functionality is not implemented yet.");
        }
        

        #region private fields        

        // handles integration event for WebEx event - common logic
        private void ProcessEvent(Event webExEvent)
        {
            // initializes current state and other varialbes
            tracer.Info($"Processing of WebEx event with id {webExEvent.EventId}");
            var currentState = integrationDb.GetIntegrationStateByEntityId(entityTypeId, entityId, webExEvent.EventId);
            var attendeeId = currentState?.AttendeeId;
            var integrationStateId = currentState?.IntegrationStateId;
            var sessionId = webExEvent.SessionId;
            var webExEventId = webExEvent.EventId;

            switch (eventTypeId)
            {
                // adds or updates person - sends WebEx invitation
                case AsyncRequestTypeEnum.AddToOuterSystem:
                case AsyncRequestTypeEnum.SendOuterSystemInvitation:
                    if (systemState.IntegrationStateTypeId != IntegrationStateTypeEnum.InWebEx)
                        throw new InvalidOperationException("Entity cannot be added to WebEx because its system state prevents this operation.");
                    var forceInvitation = eventTypeId == AsyncRequestTypeEnum.SendOuterSystemInvitation;
                    var newState = AddWebExPerson(systemState, currentState, sessionId, forceInvitation);
                    SaveWebExIntegrationState(newState, integrationStateId, webExEventId);
                    break;

                // removes person from webex
                case AsyncRequestTypeEnum.RemoveFromOuterSystem:
                    if (systemState.IntegrationStateTypeId != IntegrationStateTypeEnum.NotInWebEx)
                        throw new InvalidOperationException("Entity cannot be removed from WebEx because its system state prevents this operation.");
                    newState = RemoveWebExPerson(attendeeId, sessionId);
                    SaveWebExIntegrationState(newState, integrationStateId, webExEventId);
                    break;

                // updates webex database state
                case AsyncRequestTypeEnum.UpdateOuterSystemState:
                    newState = GetWebExState(attendeeId, sessionId);
                    SaveWebExIntegrationState(newState, integrationStateId, webExEventId);
                    break;

                // synchronises webex state
                case AsyncRequestTypeEnum.SyncOuterSystemState:
                    newState = GetWebExState(attendeeId, sessionId);
                    var consistencyResult = consistencyResolver.Compare(systemState, newState);

                    // executes operation
                    switch (consistencyResult.OperationType)
                    {
                        case SyncOperationType.Save:
                            tracer.Info("Executes AddWebExPerson sync operation");
                            newState = AddWebExPerson(systemState, currentState, sessionId);
                            break;
                        case SyncOperationType.Remove:
                            tracer.Info("Executes RemoveWebExPerson sync operation");
                            newState = RemoveWebExPerson(attendeeId, sessionId);
                            break;
                    }

                    // saves new state
                    SaveWebExIntegrationState(newState, integrationStateId, webExEventId);
                    break;
            }
        }


        // gets webex state
        private IntegrationState GetWebExState(long? attendeeId, long webExSessionId)
        {
            string errorMessage = null;
            if (attendeeId == null)
                errorMessage = WebExTextHelper.AttendeeIdMissingMessage;
            var result = service.GetWebExState(attendeeId, webExSessionId, errorMessage);
            return result;
        }

        // adds WebEx person and returns state
        private IntegrationState AddWebExPerson(IntegrationState newSate, IntegrationState historyState, long webExSessionId,
            bool forceInvitation = false)
        {
            var addResult = service.AddUpdatePerson(newSate, historyState, webExSessionId, forceInvitation);
            var result = service.GetWebExState(addResult.Result, webExSessionId, addResult.ErrorMessage);
            return result;
        }

        // remove WebEx person and returns state
        private IntegrationState RemoveWebExPerson(long? atendeeId, long webExSessionId)
        {
            if (!atendeeId.HasValue)
                throw new InvalidOperationException("There is no information about attendee ID and therefore entity cannot be removed from WebEx.");
            var removeResult = service.RemovePerson(atendeeId.Value);
            var result = service.GetWebExState(atendeeId, webExSessionId, removeResult.ErrorMessage);
            return result;
        }


        // saves new webex state
        private void SaveWebExIntegrationState(IntegrationState state, long? integrationStateId, long webExEventId)
        {
            integrationStateConvertor.FillIntegrationState(state, integrationStateId ?? 0, entityTypeId, entityId, webExEventId);
            if (!integrationStateId.HasValue)
                integrationDb.InsertIntegrationState(state);
            else
                integrationDb.UpdateIntegrationState(state);
        }

        #endregion

    }

}
