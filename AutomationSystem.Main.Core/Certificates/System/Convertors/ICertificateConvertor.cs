using AutomationSystem.Main.Core.Certificates.System.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Certificates.System.Convertors
{
    /// <summary>
    /// Certificate convertor
    /// </summary>
    public interface ICertificateConvertor
    {
        // converts class registration to certificate info object
        CertificateInfo ConvertToCertificateInfo(Class cls, ClassRegistration registration);

        // converts class and person to certificate info object
        CertificateInfo ConvertToCertificateInfo(Class cls, Person person);
    }
}