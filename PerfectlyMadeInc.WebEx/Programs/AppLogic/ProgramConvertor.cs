using System;
using System.Linq;
using AutomationSystem.Base.Contract.Integration;
using PerfectlyMadeInc.WebEx.Connectors.Integration.Model;
using PerfectlyMadeInc.WebEx.Contract.Programs.Models;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.Programs.AppLogic
{    

    /// <summary>
    /// WebEx program convertors
    /// </summary>
    public class ProgramConvertor : IProgramConvertor
    {

        // converts Program to ProgramListItem
        public ProgramListItem ConvertToProgramListItem(Program program, ConferenceAccountInfo conferenceAccount)
        {
            var result = new ProgramListItem
            {
                Id = program.ProgramId,
                Name = program.Name,                
                AccountName = conferenceAccount.Name,
                UserGroupTypeId = conferenceAccount.UserGroupTypeId,
                UserGroupId = conferenceAccount.UserGroupId
            };
            return result;
        }

        // converts Program to ProgramDetail
        public ProgramDetail ConvertToProgramDetail(Program program, ConferenceAccountInfo conferenceAccount)
        {
            if (program.Events == null)
                throw new InvalidOperationException("Events is not included into Program object.");

            var result = new ProgramDetail
            {
                Id = program.ProgramId,
                ProgramOuterId = program.ProgramOuterId,
                ProgramUrl = program.ProgramUrl,
                Name = program.Name,
                AccountName = conferenceAccount.Name,
                UserGroupTypeId = conferenceAccount.UserGroupTypeId,
                UserGroupId = conferenceAccount.UserGroupId,
                Events = program.Events.Select(ConvertToWebExEventListItem).OrderByDescending(x => x.Name).ToList(),
                IsUsed = program.EntityId.HasValue && program.EntityTypeId.HasValue
            };           

            return result;
        }

        // converts NewProgramListItem
        public NewProgramListItem ConvertToNewProgramListItem(WebExProgramInfo program)
        {
            var result = new NewProgramListItem
            {
                Name = program.Name,
                ProgramOuterId = program.ProgramId,
                ProgramUrl = program.ProgramUrl
            };
            return result;
        }

        // converts WebExProgramInfo to Program
        public Program ConvertToProgram(WebExProgramInfo program, long accountId)
        {
            var result = new Program
            {
                ProgramOuterId = program.ProgramId,
                ProgramUrl = program.ProgramUrl,
                Name = program.Name,
                AccountId = accountId
            };
            return result;
        }

        // WebExEventInfo to Event
        public Event ConvertToEvent(WebExEventInfo webExEvent, long accountId)
        {
            var result = new Event
            {
                Name = webExEvent.SessionName,
                SessionId = webExEvent.SessionId,
                AccountId = accountId
            };
            return result;
        }
        
        #region private methods

        // converts event to EventListItem
        private EventListItem ConvertToWebExEventListItem(Event eventModel)
        {
            var result = new EventListItem
            {
                Id = eventModel.EventId,
                Name = eventModel.Name,
                SessionId = eventModel.SessionId,
            };
            return result;
        }
       
        #endregion

    }

}
