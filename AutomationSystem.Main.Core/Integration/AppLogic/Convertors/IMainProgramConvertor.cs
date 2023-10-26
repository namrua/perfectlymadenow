using System.Collections.Generic;
using AutomationSystem.Main.Contract.Integration.AppLogic.Models.Programs;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.WebEx.Contract.Programs.Models;

namespace AutomationSystem.Main.Core.Integration.AppLogic.Convertors
{
    /// <summary>
    /// Converts program related objects to and from main objects
    /// </summary>
    public interface IMainProgramConvertor
    {

        // converts MainProgramFilter to ProgramFilter
        ProgramFilter ConvertToProgramFilter(MainProgramFilter filter, ProfileFilter profileFilter);

        // converts ProgramListItem to MainProgramListItem
        MainProgramListItem ConvertToMainProgramListItem(ProgramListItem item, Dictionary<long, Profile> profileMap);

        // converts ProgramDetail to MainProgramDetail
        MainProgramDetail ConvertToMainProgramDetail(ProgramDetail detail, Profile profile);

    }

}
