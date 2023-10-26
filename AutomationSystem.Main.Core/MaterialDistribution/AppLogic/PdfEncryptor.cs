using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Security;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic
{

    /// <summary>
    /// Encrypts pdf files
    /// </summary>
    public class PdfEncryptor : IPdfEncryptor
    {

        // encrypts pdf file
        public byte[] Encrypt(byte[] content, string ownerPassword, string userPassword)
        {
            var inputStream = new MemoryStream(content);
            var outputStream = Encrypt(inputStream, ownerPassword, userPassword);
            var result = outputStream.ToArray();
            return result;
        }

        // encrypts pdf file
        public MemoryStream Encrypt(Stream content, string ownerPassword, string userPassword)
        {
            PdfDocument document = PdfReader.Open(content, PdfDocumentOpenMode.Modify);
            PdfSecuritySettings securitySettings = document.SecuritySettings;

            // Setting one of the passwords automatically sets the security level to 
            // PdfDocumentSecurityLevel.Encrypted128Bit.
            securitySettings.OwnerPassword = ownerPassword;
            securitySettings.UserPassword = userPassword;

            // Don't use 40 bit encryption unless needed for compatibility reasons
            //securitySettings.DocumentSecurityLevel = PdfDocumentSecurityLevel.Encrypted40Bit;

            // Restrict some rights.
            securitySettings.PermitAccessibilityExtractContent = false;
            securitySettings.PermitAnnotations = false;
            securitySettings.PermitAssembleDocument = false;
            securitySettings.PermitExtractContent = false;
            securitySettings.PermitFormsFill = false;
            securitySettings.PermitFullQualityPrint = false;
            securitySettings.PermitModifyDocument = false;
            securitySettings.PermitPrint = false;

            // Save the document...
            var result = new MemoryStream();
            document.Save(result);
            return result;
        }

    }

}
