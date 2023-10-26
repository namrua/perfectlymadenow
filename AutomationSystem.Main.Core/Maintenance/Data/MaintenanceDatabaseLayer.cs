using System;
using System.Data.Entity.Core.Objects;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Maintenance.Data
{
    public class MaintenanceDatabaseLayer : IMaintenanceDatabaseLayer
    {
        private const int ScriptTimeout = 3600;

        public int ClearClassCertificates(DateTime toDate)
        {
            using (var context = new MainEntities())
            {
                context.Database.CommandTimeout = ScriptTimeout;
                var objectParameter = new ObjectParameter("Rowcount", typeof(int));
                context.ClearClassCertificates(toDate, objectParameter);
                return (int)objectParameter.Value;
            }
        }

        public int ClearClassRegistrationCertificates(DateTime toDate)
        {
            using (var context = new MainEntities())
            {
                context.Database.CommandTimeout = ScriptTimeout;
                var objectParameter = new ObjectParameter("Rowcount", typeof(int));
                context.ClearClassRegistrationCertificates(toDate, objectParameter);
                return (int)objectParameter.Value;
            }
        }

        public int ClearClassMaterialFiles(DateTime toDate)
        {
            using (var context = new MainEntities())
            {
                context.Database.CommandTimeout = ScriptTimeout;
                var objectParameter = new ObjectParameter("Rowcount", typeof(int));
                context.ClearClassMaterialFiles(toDate, objectParameter);
                return (int)objectParameter.Value;
            }
        }
    }
}
