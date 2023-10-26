using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// Encapsulates registration pages style
    /// </summary>
    public class RegistrationPageStyle
    {
        public long? HeaderPictureId { get; set; }
        public RegistrationColorSchemeEnum ColorSchemeId { get; set; } = RegistrationColorSchemeEnum.Limet;
        public string HomepageUrl { get; set; }
    }
}
