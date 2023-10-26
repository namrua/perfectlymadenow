using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Integration;
using AutomationSystem.Shared.Core.ConferenceAccounts.Data;

namespace AutomationSystem.Shared.Core.ConferenceAccounts.System
{
    /// <summary>
    /// Service for conference account
    /// </summary>
    public class ConferenceAccountService : IConferenceAccountService
    {

        private readonly IConferenceAccountDatabaseLayer conferenceDb;

        private readonly IConferenceAccountConvertor conferenceConvertor;


        // constructor
        public ConferenceAccountService(IConferenceAccountDatabaseLayer conferenceDb)
        {
            this.conferenceDb = conferenceDb;
            conferenceConvertor = new ConferenceAccountConvertor();
        }


        // gets list of conference accounts by filter
        public List<ConferenceAccountInfo> GetConferenceAccountsByFilter(ConferenceAccountFilter filter, bool includeDeleted = false)
        {
            var conAccounts = conferenceDb.GetConferrenceAccountsByFilter(filter, includeDeleted);
            var result = conAccounts.Select(conferenceConvertor.ConvertToConferenceAccountInfo).ToList();
            return result;
        }

        // gets conference account by type and account settings Id
        public ConferenceAccountInfo GetConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum typeId, long accountSettingsId, bool includeDeleted = false)
        {
            var conAccount = conferenceDb.GetConferenceAccountByTypeAndSettingsId(typeId, accountSettingsId, includeDeleted);
            if (conAccount == null)
                throw new ArgumentException($"There is no Conference account with type {typeId} and settings id {accountSettingsId}.");

            var result = conferenceConvertor.ConvertToConferenceAccountInfo(conAccount);
            return result;
        }        


        // save conference account
        // keep in mind that upper layer should check "account steeling" attack
        public long SaveConferenceAccount(ConferenceAccountInfo conferenceAccount, int ownerId)
        {           
            var conAccount = conferenceConvertor.ConvertToConferenceAccount(conferenceAccount);
            var result = conAccount.ConferenceAccountId;
            if (result == 0)
            {
                conAccount.OwnerId = ownerId;               
                result = conferenceDb.InsertConferenceAccount(conAccount);
            }
            else
            {
                conferenceDb.UpdateConferenceAccount(conAccount);                
            }
            return result;
        }


        // delete conference account by type and account settings Id
        public void DeleteConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum typeId, long accountSettingsId)
        {            
            conferenceDb.DeleteConferenceAccountByTypeAndSettingsId(typeId, accountSettingsId);
        }        

    }

}


