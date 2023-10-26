using System.Threading.Tasks;
using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.WebEx.Contract.Programs.Models;

namespace PerfectlyMadeInc.WebEx.Contract.Programs
{
    /// <summary>
    /// Provides program administration
    /// </summary>
    public interface IProgramAdministration
    {

        // gets list of program details
        ProgramsForList GetProgramsForList(ProgramFilter filter, bool search = false);

        // gets new webex program model
        NewProgramModel GetNewProgramModel(UserGroupTypeEnum? userGroupTypeId, long? userGroupId);

        // gets webex program list by account
        Task<NewProgramList> GetNewProgramsForList(long accountId);

        // gets program detail by id
        ProgramDetail GetProgramById(long programId);

        // saves program from WebEx to db
        Task<long> SaveProgramFromWebEx(long programOuterId, long accountId);

        // delete program
        void DeleteProgram(long programId);

    }
}
