namespace AutomationSystem.Main.Core.Certificates.System
{
    public interface ICertificateService
    {
        void GenerateCertificates(string rootPath, long classId);

        void GenerateCertificate(string rootPath, long registrationId);
    }
}
