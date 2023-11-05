using System.Threading.Tasks;
using AutomationSystem.Main.Contract.Integration.AppLogic.Models.Programs;
using PerfectlyMadeInc.WebEx.Contract.Programs.Models;
using PerfectlyMadeInc.WebEx.Contract.Webinars;

namespace AutomationSystem.Main.Contract.Integration.AppLogic
{
    /// <summary>
    /// Provides program administration
    /// </summary>
    public interface IMainProgramAdministration
    {
        // gets list of program details
        MainProgramListPageModel GetMainProgramListPageModel(MainProgramFilter filter, bool search = false);

        // gets new webex program model
        NewProgramModel GetNewProgramModel(long profileId);

        // gets webex program list by account
        Task<NewProgramList> GetNewProgramsForList(long accountId);

        Task<NewWebinarList> GetNewWebinarsForList(long accountId);

        // gets program detail by id
        MainProgramDetail GetProgramById(long programId);

        // saves program from WebEx to db
        Task<long> SaveProgramFromWebEx(long programOuterId, long accountId);

        // delete program
        void DeleteProgram(long programId);

    }

}
