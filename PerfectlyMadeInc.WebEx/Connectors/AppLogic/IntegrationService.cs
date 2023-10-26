using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using PerfectlyMadeInc.WebEx.Connectors.AppLogic.Models;
using PerfectlyMadeInc.WebEx.Connectors.Integration;
using PerfectlyMadeInc.WebEx.Contract.Connectors;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.Connectors.AppLogic
{

    /// <summary>
    /// Provides integration operation with WebEx
    /// </summary>
    public class IntegrationService : IIntegrationService
    {

        private readonly IWebExProvider provider;        
        private readonly ITracer tracer;
        private readonly IWebExPersonConvertor webExConvertor;

        // constructor
        public IntegrationService(IWebExProvider provider, ITracerFactory tracerFactory)
        {
            this.provider = provider;

            tracer = tracerFactory.CreateTracer<IntegrationService>();
            webExConvertor = new WebExPersonConvertor();
        }


        // gets webex state of person by attendee id
        public IntegrationState GetWebExState(long? attendeeId, long sessionId, string errorMessage = null)
        {
            // gets state
            IntegrationState result;
            if (!attendeeId.HasValue)
            {
                result = webExConvertor.ConvertToErrorIntegrationState(null, errorMessage);
            }
            else
            {
                try
                {
                    var webExPerson = provider.GetPersonById(sessionId, attendeeId.Value).Result;
                    result = webExPerson != null
                        ? webExConvertor.ConvertToIntegrationState(webExPerson)
                        : webExConvertor.ConvertToEmptyIntegrationState(attendeeId.Value);
                    result.ErrorMessage = errorMessage;
                }
                catch (WebExException e)
                {
                    tracer.Error(e, "Getting of WebEx state causes exception.");
                    result = webExConvertor.ConvertToErrorIntegrationState(attendeeId, e.Message);
                }
            }
            result.LastChecked = DateTime.Now;            
            return result;
        }


        // gets webex state of persons by event's session id
        public List<IntegrationState> GetWebExStatesBySessionId(long sessionId)
        {
            var persons = provider.GetPersonsInSession(sessionId).Result;
            var result = persons.Select(webExConvertor.ConvertToIntegrationState).ToList();
            return result;
        }


        // Adds or updatates person in webex, returns attendeeId
        public WebExServiceResult<long?> AddUpdatePerson(IntegrationState newState, IntegrationState historyState, long sessionId, bool forceEmailInvitation = false)
        {
            var result = new WebExServiceResult<long?>();
            try
            {
                // converts person, determines whether email invitation should be sent
                var webExPerson = webExConvertor.ConvertToWebExPerson(newState);
                if (forceEmailInvitation || historyState == null 
                                         || historyState.IntegrationStateTypeId != IntegrationStateTypeEnum.InWebEx
                                         || historyState.Email.ToLower() != newState.Email.ToLower())
                    webExPerson.AdditionalInfo.SendEmailInvitation = true; 
                
                // saves person
                result.Result = provider.SavePerson(sessionId, webExPerson).Result;
                tracer.Info($"Person was saved in WebEx event. Result = {result.Result}");

                // deletes old person from webex when new one was created
                if (historyState?.AttendeeId != null && historyState.AttendeeId != result.Result)
                {
                    var deleted = provider.DeletePersonByAttendeeId(historyState.AttendeeId.Value).Result;
                    tracer.Info($"Person {historyState.AttendeeId.Value} was deleted in WebEx event. Result = {deleted}");
                }
                result.IsSuccess = true;
            }
            catch (WebExException e)
            {
                result.Exception = e;
                result.ErrorMessage = e.Message;                
                tracer.Error(e, "Adding of person to WebEx causes exception.");
            }
            return result;
        }


        // Remove person from webex
        public WebExServiceResult<bool> RemovePerson(long attendeeId)
        {
            var result = new WebExServiceResult<bool>();
            try
            {                
                result.Result = provider.DeletePersonByAttendeeId(attendeeId).Result;
                tracer.Info($"Person was deleted in WebEx event. Result = {result.Result}");
                result.IsSuccess = true;

            }
            catch (WebExException e)
            {
                result.Exception = e;
                result.ErrorMessage = e.Message;
                tracer.Error(e, "Removin of person from WebEx causes exception.");
            }
            return result;
        }

    }

}
