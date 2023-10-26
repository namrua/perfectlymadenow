using System.Linq;
using System.Xml.Linq;
using AutomationSystem.Shared.Contract.WordWorkflows.Integration;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace AutomationSystem.Main.Core.Certificates.System.OpenXmlActions
{
    public class PrepareForMerge : IOpenXmlAction
    {
        private readonly XNamespace wordNamespace = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        public void Execute(WordprocessingDocument document)
        {
            MainDocumentPart mainPart = document.MainDocumentPart;
            var paragraph = new Paragraph(new Run((new Break() { Type = BreakValues.Page })));
            var listOfParagraphs = mainPart.Document.Body.Elements().Where(x => x.XName == wordNamespace + "p").ToList();
            mainPart.Document.Body.InsertAfter(paragraph, listOfParagraphs.First());
        }
    }
}
