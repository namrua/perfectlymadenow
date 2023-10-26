using System.IO;
using AutomationSystem.Main.Core.Reports.System.Models.CountryReport;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace AutomationSystem.Main.Core.Reports.System.ReportGenerators
{
    /// <summary>
    /// Creates word documents
    /// </summary>
    public class WordDocumentCreator : IWordDocumentCreator
    {
        public byte[] GetCountriesReport(CountriesReportModel countriesReport)
        {
            var stream = new MemoryStream();
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document, true))
            {
                // initialize
                wordDoc.AddMainDocumentPart();
                wordDoc.MainDocumentPart.Document = new Document();
                wordDoc.MainDocumentPart.Document.Body = GetBodyOfCountriesReport(countriesReport, wordDoc);
            }        
            var result = stream.ToArray();
            return result;
        }

        #region Countries report 

        private Body GetBodyOfCountriesReport(CountriesReportModel countriesReport, WordprocessingDocument wordDoc)
        {

            var body = wordDoc.MainDocumentPart.Document.Body = new Body();
            Table table = new Table();
            TableRow tr = new TableRow();

            var runProp = SetRunPropertie();
            var paragraphProp = SetParagraph();

            foreach (var country in countriesReport.Countries)
            {
                var tableRow = GetCountryReportItem(country);
                table.Append(tableRow);
            }

            // add all elements
            body.Append(new Paragraph(paragraphProp, new Run(runProp, new Text(countriesReport.ClassTitle))));
            body.Append(table);

            return body;
        }

        private TableRow GetCountryReportItem(CountryReportItem country)
        {
            //first cell
            TableRow tr = new TableRow();
            TableCell tc1 = new TableCell();
            tc1.Append(
                new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2500" }));
            tc1.Append(new Paragraph(new Run(new Text(country.Country))));
            tr.Append(tc1);

            //second cell
            TableCell tc2 = new TableCell();
            tc2.Append(
                new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2500" }));
            tc2.Append(new Paragraph(new Run(new Text(country.Count + "x"))));
            tr.Append(tc2);

            return tr;
        }

        #endregion

        #region common

        private RunProperties SetRunPropertie()
        {
            RunProperties runProp = new RunProperties();
            RunFonts runFont = new RunFonts() { Ascii = "Calibri Light" };
            runProp.Append(runFont);
            runProp.Append(new Bold());
            runProp.Append(new FontSize() { Val = "45" });
            return runProp;
        }

        private ParagraphProperties SetParagraph()
        {
            ParagraphProperties paragraphProp = new ParagraphProperties();
            Justification CenterHeading = new Justification() { Val = JustificationValues.Center };
            paragraphProp.Append(CenterHeading);

            return paragraphProp;
        }

        #endregion
    }
}
