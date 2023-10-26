using System.Collections.Generic;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Reports.AppLogic.Models
{
    /// <summary>
    /// WWA CRF Report for edit
    /// </summary>
    public class WwaCrfReportForEdit
    {
        public WwaCrfReportForm Form = new WwaCrfReportForm();
        public List<PersonMinimized> DistanceCoordinators { get; set; } = new List<PersonMinimized>();
    }
}