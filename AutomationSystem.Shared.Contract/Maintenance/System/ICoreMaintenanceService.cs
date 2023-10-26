using System;

namespace AutomationSystem.Shared.Contract.Maintenance.System
{
    public interface ICoreMaintenanceService
    {
        int ClearDatabaseFiles(DateTime toDate, int maxItems);
    }
}
