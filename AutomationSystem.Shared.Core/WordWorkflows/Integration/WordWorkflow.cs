using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutomationSystem.Shared.Contract.WordWorkflows.Integration;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;

namespace AutomationSystem.Shared.Core.WordWorkflows.Integration
{
    /// <summary>
    /// Manages word creating workflow
    /// </summary>
    public class WordWorkflow : IWordWorkflow
    {

        // initialies document
        public byte[] Initialize(string templatePath)
        {
            var document = new WmlDocument(templatePath, true);
            return document.DocumentByteArray;

        }

        // exectures open xml action
        public byte[] ExecuteOpenXmlAction(byte[] document, IOpenXmlAction action)
        {
            using (var mem = new MemoryStream())
            {
                mem.Write(document, 0, document.Length);
                using (var doc = WordprocessingDocument.Open(mem, true))
                {
                    action.Execute(doc);
                }
                return mem.ToArray();
            }
        }


        // fills parameters by data      
        public List<byte[]> FillParameters<T>(byte[] mainDocument, List<T> parameterObjects)
        {
            var result = new List<byte[]>();
            foreach (var parameter in parameterObjects)
            {
                var document = FillParameters(mainDocument, parameter);
                result.Add(document);
            }
            return result;
        }

        // fills parameters by data
        public byte[] FillParameters<T>(byte[] mainDocument, T parameterObject)
        {
            var objectsMap = BindData(parameterObject);
            var newDoc = new WmlDocument("", mainDocument);
            foreach (var objectEntry in objectsMap)
                newDoc = TextReplacer.SearchAndReplace(newDoc, objectEntry.Key, objectEntry.Value, true);
            return newDoc.DocumentByteArray;
        }


        // merges documents to one document
        public byte[] Merge(List<byte[]> documents)
        {
            var sources = GetSources(documents);
            var document = DocumentBuilder.BuildDocument(sources);
            return document.DocumentByteArray;
        }

        // merges documents and save
        public void Save(byte[] document, string outputPath)
        {
            var docToSave = new WmlDocument("", document);
            docToSave.SaveAs(outputPath);
        }


        #region private methods

        // gets list of sources
        private List<Source> GetSources(List<byte[]> documents)
        {
            var result = documents.Select(x => new Source(new WmlDocument("", x))).ToList();
            return result;
        }

        // binds data to dictionary
        private Dictionary<string, string> BindData<T>(T data)
        {
            var result = new Dictionary<string, string>();
            var t = data.GetType();
            var properties = t.GetProperties();
            foreach (var propEntry in properties)
            {
                var attrs = propEntry.GetCustomAttributes(true);
                foreach (var attribute in attrs.Select(x => x as WordWorkflowParameterAttribute).Where(x => x != null))
                {
                    var value = (string)propEntry.GetValue(data);
                    result.Add(attribute.Name, value);
                }
            }
            return result;
        }

        #endregion

    }

}
