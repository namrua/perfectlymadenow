using System.Collections.Generic;
using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Preferences
{
    /// <summary>
    /// Class preference detail
    /// </summary>
    public class ClassPreferenceDetail
    {

        [DisplayName("Profile ID")]
        public long ProfileId { get; set; }

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

        [DisplayName("Venue name")]
        public string VenueName { get; set; }

        [DisplayName("CRF Location code")]
        public string LocationCode { get; set; }

        [DisplayName("Location info")]
        public string LocationInfo { get; set; }

        [DisplayName("Currency")]
        public string Currency { get; set; }

        [DisplayName("Currency code")]
        public string CurrencyCode { get; set; }
        
        [DisplayName("Expenses")]
        public List<ExpenseDetail> Expenses { get; set; } = new List<ExpenseDetail>();

    }
}
