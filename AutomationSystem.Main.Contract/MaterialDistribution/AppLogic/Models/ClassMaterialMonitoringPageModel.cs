using System.Collections.Generic;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Class materials monitoring page model
    /// </summary>
    public class ClassMaterialMonitoringPageModel
    {
        public long ClassId { get; set; }
        public List<ClassMaterialMonitoringListItem> Recipients { get; set; } = new List<ClassMaterialMonitoringListItem>();
    }
}
