using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Class materials form
    /// </summary>
    public class ClassMaterialForm
    {

        [HiddenInput]
        public long ClassId { get; set; }

        [Required]
        [MaxLength(32)]
        [DisplayName("Owner's password")]
        public string CoordinatorPassword { get; set; }

        [Range(typeof(DateTime), "1/1/2000", "1/1/4000")]
        [DisplayName("Automation lock")]
        public DateTime? AutomationLockTime { get; set; }

    }
}
