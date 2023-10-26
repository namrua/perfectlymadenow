using System;
using System.ComponentModel;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Class material detail
    /// </summary>
    public class ClassMaterialDetail
    {

        [DisplayName("Owner's password")]
        public string CoordinatorPassword { get; set; }

        [DisplayName("Automation lock")]
        public DateTime? AutomationLockTime { get; set; }

        [DisplayName("Is locked")]
        public bool IsLocked { get; set; }

        [DisplayName("Locked")]
        public DateTime? Locked { get; set; }

        [DisplayName("Is unlocked")]
        public bool IsUnlocked { get; set; }

        [DisplayName("Unlocked")]
        public DateTime? Unlocked { get; set; }

    }
}
