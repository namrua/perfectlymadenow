using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Integration;
using PerfectlyMadeInc.WebEx.Contract.Programs.Models;
using PerfectlyMadeInc.WebEx.Programs.Data;

namespace PerfectlyMadeInc.WebEx.Programs
{

    /// <summary>
    /// Provides integration of entity with WebEx programs
    /// </summary>
    public class ProgramEntityIntegrationProvider : IEntityIntegrationProvider
    {

        private readonly IProgramDatabaseLayer programDb;
        private readonly IConferenceAccountService conferenceAccountService;


        // gets integration type
        public IntegrationTypeEnum Type => IntegrationTypeEnum.WebExProgram;


        // constructor
        public ProgramEntityIntegrationProvider(IProgramDatabaseLayer programDb, IConferenceAccountService conferenceAccountService)
        {
            this.programDb = programDb;
            this.conferenceAccountService = conferenceAccountService;
        }


        // gets list of tuples <integration entityId, integration entity name>
        public List<Tuple<long, string>> GetActiveIntegrationEntities(long? currentIntegrationEntity, UserGroupTypeEnum? userGroupTypeId, long? userGroupId)
        {
            // loads active conference accounts of user group
            var conferenceAccountFilter = new ConferenceAccountFilter
            {
                ConferenceAccountTypeId = ConferenceAccountTypeEnum.WebEx,
                UserGroupTypeId = userGroupTypeId,
                UserGroupId = userGroupId
            };
            var conferenceAccounts = conferenceAccountService.GetConferenceAccountsByFilter(conferenceAccountFilter);

            // loads programs and returns result
            var programFilter = new ProgramFilter
            {
                IncludeUsed = false,
                AllowedAccountsIds = conferenceAccounts.Select(x => x.AccountSettingsId)
            };
            var programs = programDb.GetActivePrograms(programFilter, currentIntegrationEntity);
            var result = programs.Select(x => new Tuple<long, string>(x.ProgramId, x.Name)).ToList();
            return result;
        }

        // gets integration entity name
        public string GetIntegrationEntityNameById(long? integrationEntityId)
        {
            var program = programDb.GetProgramById(integrationEntityId ?? 0);
            if (program == null)
                throw new ArgumentException($"There is no WebEx program with id {integrationEntityId}");
            return program.Name;
        }

        // attach entity to integration entity
        public void AttachEntity(EntityTypeEnum entityTypeId, long entityId, long? integrationEntityId, bool detachOthers)
        {
            programDb.SetProgramEntity(integrationEntityId ?? 0, entityTypeId, entityId, detachOthers);
        }

        // detach entity from integration entity
        public void DetachEntity(EntityTypeEnum entityTypeId, long entityId)
        {
            programDb.DetachProgramEntity(entityTypeId, entityId);
        }

    }

}
