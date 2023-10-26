using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutomationSystem.Main.Core.Certificates.System.Models;
using AutomationSystem.Main.Core.Certificates.System.OpenXmlActions;
using AutomationSystem.Shared.Contract.WordWorkflows.Integration;

namespace AutomationSystem.Main.Core.Certificates.System
{
    public class CertificateDocumentCreator : ICertificateDocumentCreator
    {
        private readonly IWordWorkflowFactory wordWorkflowFactory;

        public CertificateDocumentCreator(IWordWorkflowFactory wordWorkflowFactory)
        {
            this.wordWorkflowFactory = wordWorkflowFactory;
        }

        public byte[] CreateCertificateDocument(string rootPath, string definitionName, CertificateInfo certificate)
        {
            var workflow = wordWorkflowFactory.CreateWordWorkflow();
            var origin = workflow.Initialize(Path.Combine(rootPath, definitionName));
            var result = workflow.FillParameters(origin, certificate);
            return result;
        }

        public byte[] CreateMultiCertificateDocument(string rootPath, string definitionName, List<CertificateInfo> certificates)
        {
            var workflow = wordWorkflowFactory.CreateWordWorkflow();
            var origin = workflow.Initialize(Path.Combine(rootPath, definitionName));
            var document = workflow.ExecuteOpenXmlAction(origin, new PrepareForMerge());
            var newDocuments = GetAllDocuments(workflow, origin, document, certificates);
            var result = workflow.Merge(newDocuments);
            return result;
        }

        #region private methods

        private List<byte[]> GetAllDocuments(IWordWorkflow workflow, byte[] documentOrigin, byte[] documentNew, List<CertificateInfo> certificates)
        {
            var documents = new List<byte[]>();
            if (certificates.Count <= 0) return documents;

            foreach (var person in certificates.Take(certificates.Count - 1))
                documents.Add(workflow.FillParameters(documentNew, person));
            documents.Add(workflow.FillParameters(documentOrigin, certificates.Last()));
            return documents;
        }

        #endregion
    }
}
