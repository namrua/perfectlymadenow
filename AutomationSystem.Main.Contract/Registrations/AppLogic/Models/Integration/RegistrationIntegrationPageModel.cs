using System.Collections.Generic;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Integration
{
    /// <summary>
    /// Registration integration page model
    /// </summary>
    public class RegistrationIntegrationPageModel
    {

        // public properties
        public bool IsIntegrationDisabled { get; set; }
        public string IntegrationDisabledMessage { get; set; }
        public long ClassId { get; set; }
        public long ClassRegistrationId { get; set; }        
        public RegistrationState RegistrationState { get; set; }

        public List<IntegrationStateSummary> Attendees { get; set; }

        // constructor
        public RegistrationIntegrationPageModel()
        {
            Attendees = new List<IntegrationStateSummary>();
        }

    }

}
