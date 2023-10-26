using System.Collections.Generic;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Files.AppLogic.Models;
using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Certificates
{
    /// <summary>
    /// Class certificates page model
    /// </summary>
    public class ClassCertificatesPageModel
    {
        public ClassShortDetail Class { get; set; }

        public List<EntityFileDetail> StaffsAndComposed { get; set; } = new List<EntityFileDetail>();
        public List<EntityFileDetail> Students { get; set; } = new List<EntityFileDetail>();
        public List<EntityFileDetail> WwaStudents { get; set; } = new List<EntityFileDetail>();

        public List<AsyncRequestDetail> GeneratingRequests { get; set; } = new List<AsyncRequestDetail>();

        public bool AreCertificatesDisabled { get; set; }
        public string CertificatesDisabledMessage { get; set; }
    }
}
