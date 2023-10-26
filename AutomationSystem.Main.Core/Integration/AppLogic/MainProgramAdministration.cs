using System;
using System.Linq;
using System.Threading.Tasks;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Integration.AppLogic;
using AutomationSystem.Main.Contract.Integration.AppLogic.Models.Programs;
using AutomationSystem.Main.Core.Integration.AppLogic.Convertors;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.System.Extensions;
using CorabeuControl.Components;
using PerfectlyMadeInc.WebEx.Contract.Programs;
using PerfectlyMadeInc.WebEx.Contract.Programs.Models;

namespace AutomationSystem.Main.Core.Integration.AppLogic
{
    /// <summary>
    /// Provides program administration
    /// </summary>
    public class MainProgramAdministration : IMainProgramAdministration
    {

        private readonly IProgramAdministration programAdministration;
        private readonly IProfileDatabaseLayer profileDb;
        private readonly IIdentityResolver identityResolver;
        private readonly IMainProgramConvertor programConvertor;

        // constructor
        public MainProgramAdministration(
            IProgramAdministration programAdministration,
            IProfileDatabaseLayer profileDb,
            IIdentityResolver identityResolver,
            IMainProgramConvertor programConvertor)
        {
            this.programAdministration = programAdministration;
            this.profileDb = profileDb;
            this.identityResolver = identityResolver;
            this.programConvertor = programConvertor;
        }


        // gets list of program details
        public MainProgramListPageModel GetMainProgramListPageModel(MainProgramFilter filter, bool search = false)
        {
            var result = new MainProgramListPageModel(filter);
            result.WasSearched = search;

            // loads profiles
            var profileFilter = identityResolver.GetGrantedProfilesForEntitle(Entitle.WebExPrograms);
            var profiles = profileDb.GetProfilesByFilter(profileFilter);
            result.Profiles = profiles.Select(x => DropDownItem.Item(x.ProfileId, x.Name)).ToList();

            // executes searching
            if (search)
            {
                var profileMap = profiles.ToDictionary(x => x.ProfileId);
                var origFilter = programConvertor.ConvertToProgramFilter(filter, profileFilter);
                var programs = programAdministration.GetProgramsForList(origFilter, true);
                result.Items = programs.Items.Select(x => programConvertor.ConvertToMainProgramListItem(x, profileMap)).ToList();
            }
            return result;
        }

        // gets new webex program model
        public NewProgramModel GetNewProgramModel(long profileId)
        {
            var result = programAdministration.GetNewProgramModel(UserGroupTypeEnum.MainProfile, profileId);
            return result;
        }

        // gets webex program list by account
        public Task<NewProgramList> GetNewProgramsForList(long accountId)
        {
            return programAdministration.GetNewProgramsForList(accountId);
        }

        // gets program detail by id
        public MainProgramDetail GetProgramById(long programId)
        {
            var origDetail = programAdministration.GetProgramById(programId);
            var profile = profileDb.GetProfileById(origDetail.UserGroupId ?? 0);
            if (profile == null)
                throw new ArgumentException($"There is no Profile with id {origDetail.UserGroupId}.");
            var result = programConvertor.ConvertToMainProgramDetail(origDetail, profile);
            return result;
        }

        // saves program from WebEx to db
        public Task<long> SaveProgramFromWebEx(long programOuterId, long accountId)
        {
            return programAdministration.SaveProgramFromWebEx(programOuterId, accountId);
        }

        // delete program
        public void DeleteProgram(long programId)
        {
            programAdministration.DeleteProgram(programId);
        }

    }

}
