using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// ClassMaterialFile detail
    /// </summary>
    public class ClassMaterialFileDetail
    {
        [DisplayName("ID")]
        public long ClassMaterialFileId { get; set; }

        [DisplayName("Name")]
        public string DisplayName { get; set; }

        [DisplayName("Language code")]
        public LanguageEnum LanguageId { get; set; }

        [DisplayName("Language")]
        public string Language { get; set; }

        [DisplayName("File ID")]
        public long FileId { get; set; }

        [DisplayName("Downloads count")]
        public int DownloadsCount { get; set; }
    }
}
