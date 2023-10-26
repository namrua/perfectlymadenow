using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.StyleAndBehavior
{
    /// <summary>
    /// Class style form
    /// </summary>
    public class ClassStyleForm
    {
        [HiddenInput]
        public long ClassId { get; set; }

        [MaxLength(256)]
        [DisplayName("Detached Homepage")]
        public string HomepageUrl { get; set; }

        [Required]
        [DisplayName("Color scheme of registration pages")]
        [PickInputOptions(NoItemText = "no scheme", Placeholder = "select scheme")]
        public RegistrationColorSchemeEnum? RegistrationColorSchemeId { get; set; }

        [HiddenInput]
        public long? OriginHeaderPictureId { get; set; }

        [DisplayName("Remove current picture")]
        public bool RemoveHeaderPicture { get; set; } = false;

        [DisplayName("Send certificates by Thank you email")]
        public bool SendCertificatesByEmail { get; set; }
    }
}