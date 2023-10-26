using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.StyleAndBehavior
{
    /// <summary>
    /// Class style detail
    /// </summary>
    public class ClassStyleDetail
    {
        [DisplayName("Detached Homepage")]
        public string HomepageUrl { get; set; }

        [DisplayName("Color scheme of registration pages code")]
        public RegistrationColorSchemeEnum RegistrationColorSchemeId { get; set; }

        [DisplayName("Color scheme of registration pages")]
        public string RegistrationColorScheme { get; set; }

        [DisplayName("Header picture")]
        public long? HeaderPictureId { get; set; }

        [DisplayName("Send certificates by Thank you email")]
        public bool SendCertificatesByEmail { get; set; }

        public bool ShowClassBehaviorSettings { get; set; }
    }
}