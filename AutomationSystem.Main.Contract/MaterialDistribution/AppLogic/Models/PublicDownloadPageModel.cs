using System.Collections.Generic;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Encapsulates model for public download page 
    /// </summary>
    public class PublicDownloadPageModel
    {
        public string RequestCode { get; set; }
        public List<ClassMaterialToDownload> Materials { get; set; } = new List<ClassMaterialToDownload>();
    }
}
