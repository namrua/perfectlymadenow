using System.Collections.Generic;

namespace AutomationSystem.Shared.Contract.WordWorkflows.Integration
{
    /// <summary>
    /// Manages word creating workflow
    /// </summary>
    public interface IWordWorkflow
    {

        // initializes word document form template
        byte[] Initialize(string templatePath);

        // executes open xml action
        byte[] ExecuteOpenXmlAction(byte[] document, IOpenXmlAction action);

        // fills parameters by data
        List<byte[]> FillParameters<T>(byte[] mainDocument, List<T> parameterObjects);

        // fills parameters by data
        byte[] FillParameters<T>(byte[] mainDocument, T parameterObject);

        // merges documents to one document
        byte[] Merge(List<byte[]> documents);

        // merges documents and save
        void Save(byte[] document, string outputPath);

    }

}
