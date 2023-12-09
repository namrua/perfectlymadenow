using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Extensions;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Base.Contract.Integration;
using CorabeuControl.Components;
using Newtonsoft.Json;
using PerfectlyMadeInc.WebEx.Connectors.AppLogic;
using PerfectlyMadeInc.WebEx.Connectors.Integration;
using PerfectlyMadeInc.WebEx.Connectors.Integration.Model;
using PerfectlyMadeInc.WebEx.Contract.Programs;
using PerfectlyMadeInc.WebEx.Contract.Programs.Models;
using PerfectlyMadeInc.WebEx.Contract.Webinars;
using PerfectlyMadeInc.WebEx.Model;
using PerfectlyMadeInc.WebEx.Programs.AppLogic;
using PerfectlyMadeInc.WebEx.Programs.Data;
using PerfectlyMadeInc.WebEx.Programs.Data.Models;
using static PerfectlyMadeInc.WebEx.Helper.Constants;

namespace PerfectlyMadeInc.WebEx.Programs
{

    /// <summary>
    /// Provides program administration
    /// </summary>
    public class ProgramAdministration : IProgramAdministration
    {

        private readonly IProgramDatabaseLayer programDb;
        private readonly IConferenceAccountService conferenceService;
        private readonly IWebExFactory webExFactory;
        private readonly IIdentityResolver identityResolver;
        private readonly IProgramConvertor programConvertor;

        // constructor
        public ProgramAdministration(IProgramDatabaseLayer programDb, IConferenceAccountService conferenceService,
            IWebExFactory webExFactory, IIdentityResolver identityResolver)
        {
            this.programDb = programDb;
            this.conferenceService = conferenceService;
            this.webExFactory = webExFactory;
            this.identityResolver = identityResolver;
            programConvertor = new ProgramConvertor();
        }

        // gets list of program details
        public ProgramsForList GetProgramsForList(ProgramFilter filter, bool search = false)
        {
            var result = new ProgramsForList(filter);
            result.WasSearched = search;
            if (search)
            {
                var confAccFilter = new ConferenceAccountFilter
                {
                    ConferenceAccountTypeId = ConferenceAccountTypeEnum.WebEx,
                    UserGroupTypeId = filter.UserGroupTypeId,
                    UserGroupId = filter.UserGroupId,
                    UserGroupIds = filter.UserGroupIds,
                };

                // loads conference account map
                var conferenceAccounts = conferenceService
                    .GetConferenceAccountsByFilter(confAccFilter)
                    .ToDictionary(x => x.AccountSettingsId, y => y);

                // todo: #BICH - batch item check

                // loads programs
                filter.AllowedAccountsIds = conferenceAccounts.Values.Select(x => x.AccountSettingsId).ToList();
                var programs = programDb.GetProgramsByFilter(filter);
                result.Items = programs.Select(x => programConvertor.ConvertToProgramListItem(x, conferenceAccounts[x.AccountId])).ToList();
            }
            return result;
        }

        // gets new webex program model
        public NewProgramModel GetNewProgramModel(UserGroupTypeEnum? userGroupTypeId, long? userGroupId)
        {
            // checks access rights
            identityResolver.CheckEntitleForUserGroup(Entitle.WebExPrograms, userGroupTypeId, userGroupId);

            // sets filter for conf accouts of specified user group
            var confAccFilter = new ConferenceAccountFilter
            {
                ConferenceAccountTypeId = ConferenceAccountTypeEnum.WebEx,
                UserGroupTypeId = userGroupTypeId,
                UserGroupId = userGroupId
            };

            // filters conference accounts and returns new program model
            var conferenceAccounts = conferenceService.GetConferenceAccountsByFilter(confAccFilter);
            var result = new NewProgramModel();
            result.Accounts = conferenceAccounts.Where(x => x.Active).Select(x => PickerItem.Item(x.AccountSettingsId, x.Name)).ToList();
            return result;
        }


        // gets webex program list by conference account
        public async Task<NewProgramList> GetNewProgramsForList(long accountId)
        {
            // checks access rights for account
            var confAccount = conferenceService.GetConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum.WebEx, accountId);
            identityResolver.CheckEntitleForConferenceAccountInfo(Entitle.WebExPrograms, confAccount);

            // gets programs from webex                                       
            var provider = webExFactory.CreateWebExProvider(accountId);
            var programsApi = await provider.GetPrograms();

            // gets program ids in database - ALL PROGRAMS THROUGH ALL ACCOUNTS SHOULD BE CONSIDERED 
            // There should not be physical WebEx program shared by two accounts
            var loadedProgramsIds = new HashSet<long>(programDb.GetProgramsByFilter().Select(x => x.ProgramOuterId));

            // assembles result
            var result = new NewProgramList
            {
                AccountId = accountId,
                Items = programsApi
                    .Where(x => !loadedProgramsIds.Contains(x.ProgramId))
                    .Select(programConvertor.ConvertToNewProgramListItem).ToList()
            };
            return result;
        }

        public async Task<NewWebinarList> GetNewWebinarsForList(long accountId)
        {
            // checks access rights for account
            var confAccount = conferenceService.GetConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum.WebEx, accountId);
            identityResolver.CheckEntitleForConferenceAccountInfo(Entitle.WebExPrograms, confAccount);

            IWebExProvider provider = webExFactory.GetWebexAccessToken(accountId);
            var webinars = await provider.GetWebinar();

            var loadedProgramsIds = new HashSet<string>(programDb.GetProgramsByFilter().Select(x => x.ProgramOuterId.ToString()));

            // assembles result
            var result = new NewWebinarList
            {
                AccountId = accountId,
                Items = webinars?.Items
                    .Where(x => !loadedProgramsIds.Contains(x.Id))
                    .Select(m => new NewWebinarListItem
                    {
                        Title = m.Title,
                        Id = m.Id,
                        WebLink = m.WebLink
                    }).ToList()
            };
            return result;
        }

        // gets WebEx program by id
        public ProgramDetail GetProgramById(long programId)
        {
            var program = GetDbProgramById(programId, ProgramIncludes.Events);

            // loads conference account of program
            var conferenceAccount = conferenceService.GetConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum.WebEx, program.AccountId, includeDeleted: true);
            identityResolver.CheckEntitleForConferenceAccountInfo(Entitle.WebExPrograms, conferenceAccount);
            var result = programConvertor.ConvertToProgramDetail(program, conferenceAccount);
            return result;
        }




        // saves program from WebEx to db
        public async Task<long> SaveProgramFromWebEx(long programOuterId, long accountId)
        {
            // checks access rights for account
            var confAccount = conferenceService.GetConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum.WebEx, accountId);
            identityResolver.CheckEntitleForConferenceAccountInfo(Entitle.WebExPrograms, confAccount);

            // gets provider
            var provider = webExFactory.CreateWebExProvider(accountId);

            // loads webex program
            var program = await provider.GetProgramById(programOuterId);
            if (program == null)
                throw new ArgumentException($"There is no WebEx program with id {programOuterId} with WebEx.");
            var events = await provider.GetsEventsByProgramId(programOuterId);

            // converts to db objects
            var dbProgram = programConvertor.ConvertToProgram(program, accountId);
            foreach (var webExEvent in events)
                dbProgram.Events.Add(programConvertor.ConvertToEvent(webExEvent, accountId));

            // save program
            var result = programDb.InsertProgram(dbProgram);
            return result;
        }


        // delete program
        public void DeleteProgram(long id)
        {
            var toDelete = GetDbProgramById(id);
            var confAccount = conferenceService.GetConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum.WebEx, toDelete.AccountId);
            identityResolver.CheckEntitleForConferenceAccountInfo(Entitle.WebExPrograms, confAccount);
            programDb.DeleteProgram(id, true);
        }



        #region private fields

        // loads DB program and checks for existence
        private Program GetDbProgramById(long programId, ProgramIncludes includes = ProgramIncludes.None)
        {
            var program = programDb.GetProgramById(programId, includes);
            if (program == null)
                throw new ArgumentException($"There is no program with id {programId}.");
            return program;
        }

        #endregion

    }

}
