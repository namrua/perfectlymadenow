using System.Collections.Generic;
using PerfectlyMadeInc.WebEx.Contract.Programs.Models;

namespace PerfectlyMadeInc.WebEx.Contract.Programs
{
    public interface IEventDataProvider
    {
        List<EventDto> GetEventInfoByProgramId(long programId);
    }
}
