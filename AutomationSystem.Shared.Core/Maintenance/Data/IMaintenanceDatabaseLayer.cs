using System;

namespace AutomationSystem.Shared.Core.Maintenance.Data
{
    public interface IMaintenanceDatabaseLayer
    {
        int ClearFiles(DateTime toDate, int maxItems);
    }
}
