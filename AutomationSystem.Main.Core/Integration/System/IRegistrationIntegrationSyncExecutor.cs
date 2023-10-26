using System.Collections.Generic;
using AutomationSystem.Main.Core.Integration.System.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Main.Core.Integration.System
{
    /// <summary>
    /// Executes registration integration synchronization
    /// </summary>
    public interface IRegistrationIntegrationSyncExecutor
    {
        RegistrationIntegrationSyncResult ExecuteSync(JobRun jobRun, Class cls, List<ClassRegistration> approvedClosedRegistrations);
    }
}
