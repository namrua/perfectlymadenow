using System.Collections.Generic;
using AutomationSystem.Main.Core.Certificates.System.Models;

namespace AutomationSystem.Main.Core.Certificates.System
{
    public interface ICertificateDocumentCreator
    {
        byte[] CreateCertificateDocument(string rootPath, string definitionName, CertificateInfo certificate);

        byte[] CreateMultiCertificateDocument(string rootPath, string definitionName, List<CertificateInfo> certificates);
    }
}
