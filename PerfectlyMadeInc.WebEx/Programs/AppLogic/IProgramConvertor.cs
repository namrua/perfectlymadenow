using AutomationSystem.Base.Contract.Integration;
using PerfectlyMadeInc.WebEx.Connectors.Integration.Model;
using PerfectlyMadeInc.WebEx.Contract.Programs.Models;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.Programs.AppLogic
{         
    /// <summary>
    /// WebEx program convertor
    /// </summary>
    public interface IProgramConvertor
    {

        // converts Program to ProgramListItem
        ProgramListItem ConvertToProgramListItem(Program program, ConferenceAccountInfo conferenceAccount);

        // converts Program to ProgramDetail
        ProgramDetail ConvertToProgramDetail(Program program, ConferenceAccountInfo conferenceAccount);

        // converts PartialProgramListItem
        NewProgramListItem ConvertToNewProgramListItem(WebExProgramInfo webExprogram);

        // converts ProgramInfo to Program
        Program ConvertToProgram(WebExProgramInfo webExProgram, long accountId);

        // EventInfo to Event
        Event ConvertToEvent(WebExEventInfo webExEvent, long accountId);
      
    }
}
