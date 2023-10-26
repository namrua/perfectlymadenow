using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Certificates;

namespace AutomationSystem.Main.Contract.Classes.AppLogic
{
    public interface IClassCertificateAdministration
    {
        ClassCertificatesPageModel GetClassCertificatesPageModel(long classId);

        long GenerateCertificates(long classId);
    }
}
