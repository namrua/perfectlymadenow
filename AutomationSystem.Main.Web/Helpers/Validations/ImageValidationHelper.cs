using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using AutomationSystem.Main.Web.Helpers.Validations.Models;

namespace AutomationSystem.Main.Web.Helpers.Validations
{
    /// <summary>
    /// Helps with image validations
    /// </summary>
    public static class ImageValidationHelper
    {

        // validates image for minimal size
        public static ImageValidationResult ValidateJpgSize(Stream image, int minWidth, int minHeight)
        {
            var result = new ImageValidationResult();
            result.IsValid = false;

            // loads image
            Image img;
            try
            {
                img = Image.FromStream(image);
                image.Seek(0, SeekOrigin.Begin);            // go back to the begin of the file
            }
            catch (Exception)
            {
                result.ValidationMessage = "Uploaded file does not have valid image format.";
                return result;
            }

            // validates for file type
            if (!img.RawFormat.Equals(ImageFormat.Jpeg))
            {
                result.ValidationMessage = "Uploaded file is not .jpg image.";
                return result;
            };

            // validates for size
            if (img.Width < minWidth || img.Height < minHeight)
            {
                result.ValidationMessage = $"Size of image ({img.Width}x{img.Height}) is smaller than minimal size ({minWidth}x{minHeight}).";
                return result;
            }

            result.IsValid = true;
            return result;
        }

    }

}
