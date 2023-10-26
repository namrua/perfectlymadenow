using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Reports
{
    /// <summary>
    /// Class report setting form
    /// </summary>
    public class ClassReportSettingForm
    {
        [HiddenInput]
        public long ClassId { get; set; }
        
        [MaxLength(64)]
        [DisplayName("Venue name")]
        public string VenueName { get; set; }

        [Required]
        [MaxLength(16)]
        [DisplayName("CRF Location code")]
        public string LocationCode { get; set; }

        [DisplayName("Location info")]
        [PickInputOptions(ControlType = PickControlType.TypeaheadDropDownInput, Placeholder = "select location info")]
        public long? LocationInfoId { get; set; }
    }
}