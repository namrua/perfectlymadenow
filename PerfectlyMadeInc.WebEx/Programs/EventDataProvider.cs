using System;
using System.Collections.Generic;
using System.Linq;
using PerfectlyMadeInc.WebEx.Contract.Programs;
using PerfectlyMadeInc.WebEx.Contract.Programs.Models;
using PerfectlyMadeInc.WebEx.Model;
using PerfectlyMadeInc.WebEx.Programs.Data;
using PerfectlyMadeInc.WebEx.Programs.Data.Models;

namespace PerfectlyMadeInc.WebEx.Programs
{
    public class EventDataProvider : IEventDataProvider
    {
        private readonly IProgramDatabaseLayer programDb;

        public EventDataProvider(IProgramDatabaseLayer programDb)
        {
            this.programDb = programDb;
        }

        public List<EventDto> GetEventInfoByProgramId(long programId)
        {
            var program = programDb.GetProgramById(programId, ProgramIncludes.Events);
            if (program == null)
            {
                throw new ArgumentException($"There is no Program with id {programId}.");
            }

            var result = program.Events.Select(MapToEventInfo).ToList();
            return result;
        }

        #region private methods

        private EventDto MapToEventInfo(Event evnt)
        {
            return new EventDto
            {
                EventId = evnt.EventId,
                Name = evnt.Name,
                SessionId = evnt.SessionId
            };
        }

        #endregion
    }
}
