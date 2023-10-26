using System;
using System.IO;
using AutomationSystem.Main.Core.MaterialDistribution.AppLogic;
using AutomationSystem.Main.Web.Helpers.Validations.Models;

namespace AutomationSystem.Main.Web.Helpers.Validations
{
    /// <summary>
    /// Helps with pdf validations
    /// </summary>
    public static class PdfValidationHelper
    {

        // determines whether pdfFile is valid pdf document
        public static PdfValidationResult ValidatePdf(Stream pdfFile)
        {
            var result = new PdfValidationResult();
            result.IsValid = false;

            try
            {
                var encryptor = new PdfEncryptor();
                encryptor.Encrypt(pdfFile, "0123", "4567");
                pdfFile.Seek(0, SeekOrigin.Begin);
            }
            catch (Exception)
            {
                result.ValidationMessage = "File is not valid and unprotected PDF file. It is not possible to encrypt it.";
                return result;
            }

            result.IsValid = true;
            return result;
        }

    }

}
