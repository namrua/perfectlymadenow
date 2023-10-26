using System;
using System.Data.Entity.Core.Objects;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Maintenance.Data
{
    public class MaintenanceDatabaseLayer : IMaintenanceDatabaseLayer
    {
        private const int ScriptTimeout = 3600;

        public int ClearFiles(DateTime toDate, int maxItems)
        {
            using (var context = new CoreEntities())
            {
                context.Database.CommandTimeout = ScriptTimeout;
                var objectParameter = new ObjectParameter("Rowcount", typeof(int));
                context.ClearFiles(toDate, maxItems, objectParameter);
                return (int)objectParameter.Value;
            }
        }
    }
}
