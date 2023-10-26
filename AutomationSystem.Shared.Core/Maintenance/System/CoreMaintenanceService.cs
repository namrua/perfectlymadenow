using System;
using AutomationSystem.Shared.Contract.Maintenance.System;
using AutomationSystem.Shared.Core.Maintenance.Data;

namespace AutomationSystem.Shared.Core.Maintenance.System
{
    public class CoreMaintenanceService : ICoreMaintenanceService
    {
        private readonly IMaintenanceDatabaseLayer maintenanceDb;

        public CoreMaintenanceService(IMaintenanceDatabaseLayer maintenanceDb)
        {
            this.maintenanceDb = maintenanceDb;
        }

        public int ClearDatabaseFiles(DateTime toDate, int maxItems)
        {
            var result = maintenanceDb.ClearFiles(toDate, maxItems);
            return result;
        }
    }
}
