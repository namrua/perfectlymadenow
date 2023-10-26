using System;
using System.ComponentModel;

namespace AutomationSystem.Main.Contract.Files.AppLogic.Models
{
    /// <summary>
    /// Registration files info
    /// </summary>
    public class EntityFileDetail
    {
        [DisplayName("ID")]
        public long Id { get; set; }

        [DisplayName("File ID")]
        public long FileId { get; set; }

        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Name")]
        public string DisplayedName { get; set; }

        [DisplayName("Assigned")]
        public DateTime Assigned { get; set; }
    }
}