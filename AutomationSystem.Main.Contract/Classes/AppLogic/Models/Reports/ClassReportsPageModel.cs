using System.Collections.Generic;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Files.AppLogic.Models;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Reports
{
    /// <summary>
    /// Class reports page model
    /// </summary>
    public class ClassReportsPageModel
    {
        public ClassShortDetail Class { get; set; }
        public bool IsSupervisedByMaster { get; set; }   
        public string MasterCoordinatorEmail { get; set; }
    
        public List<ClassReportItem> Reports { get; set; } = new List<ClassReportItem>();
        public List<EntityFileDetail> Files { get; set; } = new List<EntityFileDetail>();
        public List<AsyncRequestDetail> GeneratingRequests { get; set; } = new List<AsyncRequestDetail>();

        public bool AreReportsDisabled { get; set; }
        public string ReportsDisabledMessage { get; set; }
    }
}