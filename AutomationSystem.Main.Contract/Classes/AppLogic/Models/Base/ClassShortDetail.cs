using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base
{
    /// <summary>
    /// Serves as base class page model
    /// </summary>
    public class ClassShortDetail
    {
        [DisplayName("ID")]
        public long ClassId { get; set; }

        [DisplayName("Class state")]
        public ClassState ClassState { get; set; }

        [DisplayName("Class category code")]
        public ClassCategoryEnum ClassCategoryId { get; set; }

        [DisplayName("Title")]
        public string ClassTitle { get; set; }
    }
}