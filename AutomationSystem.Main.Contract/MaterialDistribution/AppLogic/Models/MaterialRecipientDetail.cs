using System;
using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Material recipient detail
    /// </summary>
    public class MaterialRecipientDetail
    {
        [DisplayName("Class material recipient ID")]
        public long ClassMaterialRecipientId { get; set; }

        [DisplayName("Recipient ID")]
        public RecipientId RecipientId { get; set; }

        [DisplayName("Student's password")]
        public string Password { get; set; }

        [DisplayName("Request code")]
        public string RequestCode { get; set; }

        [DisplayName("Language code")]
        public LanguageEnum? LanguageId { get; set; }

        [DisplayName("Language")]
        public string Language { get; set; }

        [DisplayName("Download limit")]
        public int? DownloadLimit { get; set; }

        [DisplayName("Is locked")]
        public bool IsLocked { get; set; }

        [DisplayName("Locked")]
        public DateTime? Locked { get; set; }

        [DisplayName("Last notification")]
        public DateTime? Notified { get; set; }
    }
}
