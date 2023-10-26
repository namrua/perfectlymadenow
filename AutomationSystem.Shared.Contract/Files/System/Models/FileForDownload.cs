using System.Web.Mvc;

namespace AutomationSystem.Shared.Contract.Files.System.Models
{
    /// <summary>
    /// Encapsulates information about report file
    /// </summary>
    public class FileForDownload
    {

        // public properties
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public byte[] Content { get; set; }

        // constructor
        public FileForDownload() { }
        public FileForDownload(byte[] content, string mimeType, string fileName)
        {
            Content = content;
            MimeType = mimeType;
            FileName = fileName;
        }

        // gets file action result
        public FileContentResult GetFileActionResult()
        {
            var result = new FileContentResult(Content, MimeType);
            result.FileDownloadName = FileName;           
            return result;
        }

    }

}
