using System;

namespace AutomationSystem.Main.Core.Maintenance.Data
{
    public interface IMaintenanceDatabaseLayer
    {
        int ClearClassCertificates(DateTime toDate);
        int ClearClassRegistrationCertificates(DateTime toDate);
        int ClearClassMaterialFiles(DateTime toDate);
    }
}
