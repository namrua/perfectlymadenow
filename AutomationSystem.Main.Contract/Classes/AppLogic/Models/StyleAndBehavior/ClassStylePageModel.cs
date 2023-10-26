using System.ComponentModel;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.StyleAndBehavior
{
    /// <summary>
    /// Class style and behavior page model
    /// </summary>
    public class ClassStylePageModel
    {
        [DisplayName("Class")]
        public ClassShortDetail Class { get; set; } = new ClassShortDetail();

        [DisplayName("Style and behavior")]
        public ClassStyleDetail Style { get; set; } = new ClassStyleDetail();

        public bool AreStylesAndBehaviorDisabled { get; set; }
        public string StylesAndBehaviorDisabledMessage { get; set; }
    }
}