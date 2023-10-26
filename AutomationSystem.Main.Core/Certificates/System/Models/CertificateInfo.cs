using AutomationSystem.Shared.Contract.WordWorkflows.Integration;

namespace AutomationSystem.Main.Core.Certificates.System.Models
{
    public class CertificateInfo
    {
        [WordWorkflowParameter("@NAME")]
        public string Name { get; set; }

        [WordWorkflowParameter("@REGPAR")]
        public string RegistrantParticipant { get; set; }

        [WordWorkflowParameter("@REGISTRANT")]
        public string Registrant { get; set; }

        [WordWorkflowParameter("@DESCRIPTION")]
        public string Description { get; set; }

        public bool UseWwaTemplate { get; set; }
    }
}