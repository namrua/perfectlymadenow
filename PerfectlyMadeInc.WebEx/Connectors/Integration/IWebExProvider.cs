using System.Collections.Generic;
using System.Threading.Tasks;
using PerfectlyMadeInc.WebEx.Connectors.Integration.Model;

namespace PerfectlyMadeInc.WebEx.Connectors.Integration
{
    /// <summary>
    /// WebEx integrator
    /// </summary>
    public interface IWebExProvider
    {

        // gets list of persons in session
        Task<List<WebExPersonExtended>> GetPersonsInSession(long sessionId);

        // gets person by id
        Task<WebExPersonExtended> GetPersonById(long sessionId, long attendeeId);

        // gets list of programs
        Task<List<WebExProgramInfo>> GetPrograms();

        Task<WebexWebinarInfo> GetWebinar();

        // gets list of events by program id
        Task<List<WebExEventInfo>> GetsEventsByProgramId(long programId);

        // gets program by id
        Task<WebExProgramInfo> GetProgramById(long programId);
     
        // add or update person into/in WebEx session - returns attendeeId
        Task<long> SavePerson(long sessionId, WebExPersonExtended person);

        // delete person from session by attendeeId - returns true whether person was deleted
        Task<bool> DeletePersonByAttendeeId(long attendeeId);    

    }

}
