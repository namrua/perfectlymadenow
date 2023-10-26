using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Encapsulates model for language selection public page
    /// </summary>
    public class PublicLanguageSelectionPageModel
    {
        public string RequestCode { get; set; }
        public List<IEnumItem> Languages { get; set; } = new List<IEnumItem>();
    }
}
