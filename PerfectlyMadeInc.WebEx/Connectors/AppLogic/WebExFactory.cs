using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Integration;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using PerfectlyMadeInc.WebEx.Accounts.Data;
using PerfectlyMadeInc.WebEx.Connectors.Integration;
using PerfectlyMadeInc.WebEx.Connectors.Integration.Model;

namespace PerfectlyMadeInc.WebEx.Connectors.AppLogic
{
    /// <summary>
    /// WebEx integration factory
    /// </summary>
    public class WebExFactory : IWebExFactory
    {

        // private fields
        private readonly IConferenceAccountService conferenceService;
        private readonly IAccountDatabaseLayer accountDb;
        private readonly ITracerFactory tracerFactory;

        // constructor
        public WebExFactory(IConferenceAccountService conferenceService, IAccountDatabaseLayer accountDb, ITracerFactory tracerFactory)
        {
            this.conferenceService = conferenceService;
            this.accountDb = accountDb;
            this.tracerFactory = tracerFactory;
        }

        // get webEx provider by conference account id
        public IWebExProvider CreateWebExProvider(long accountId)
        {
            // gets account
            var account = accountDb.GetAccountById(accountId);
            if (account == null)
                throw new ArgumentException($"There is no WebEx account with id {accountId}.");

            // gets conference account
            var conferenceAccount = conferenceService.GetConferenceAccountByTypeAndSettingsId(ConferenceAccountTypeEnum.WebEx, accountId);          
            
            // assembles settings and creates provider
            var settings = new WebExSettingInfo();
            settings.Login = account.Login;
            settings.Password = account.Password;
            settings.SiteName = account.SiteName;
            settings.ServiceUrl = account.ServiceUrl;
            settings.Enabled = conferenceAccount.Active;
            var result = new WebExProvider(settings);
            return result;
        }

        // get webEx service by conference account id
        public IIntegrationService CreateIntegrationService(long accountId)
        {
            var provider = CreateWebExProvider(accountId);
            var result = new IntegrationService(provider, tracerFactory);
            return result;
        }

    }

}