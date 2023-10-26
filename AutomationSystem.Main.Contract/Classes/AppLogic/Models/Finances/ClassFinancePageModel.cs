using System.ComponentModel;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances
{
    /// <summary>
    /// Class business page model
    /// </summary>
    public class ClassFinancePageModel
    {
        [DisplayName("Class")]
        public ClassShortDetail Class { get; set; } = new ClassShortDetail();

        [DisplayName("Finance")]
        public ClassBusinessDetail Finance { get; set; } = new ClassBusinessDetail();

        public bool AreFinanceDisabled { get; set; }
        public string FinanceDisabledMessage { get; set; }
    }
}