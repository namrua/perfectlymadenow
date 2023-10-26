using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Reports.AppLogic.Models
{
    /// <summary>
    /// WWA CRF Report form
    /// </summary>
    public class WwaCrfReportForm
    {
        [Required]
        [Range(typeof(DateTime), "1/1/2000", "1/1/4000")]
        [DisplayName("From date")]
        public DateTime? FromDate { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/2000", "1/1/4000")]
        [DisplayName("To date")]
        public DateTime? ToDate { get; set; }

        [Required]
        [PickInputOptions(NoItemText = "no distance coordinator", Placeholder = "select distance coordinator")]
        [DisplayName("Distance coordinator")]
        public long? DistanceCoordinatorId { get; set; }
    }
}