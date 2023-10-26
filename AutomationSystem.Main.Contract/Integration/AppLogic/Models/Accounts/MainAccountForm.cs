using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AutomationSystem.Main.Contract.Integration.AppLogic.Models.Accounts
{
    /// <summary>
    /// Main Account form
    /// </summary>
    public class MainAccountForm
    {

        [HiddenInput]
        public long ConferenceAccountId { get; set; }
        [HiddenInput]
        public long AccountId { get; set; }
        [HiddenInput]
        public long ProfileId { get; set; }

        [DisplayName("Name")]
        [MaxLength(128)]
        public string Name { get; set; }

        [DisplayName("Site name")]
        [MaxLength(128)]
        public string SiteName { get; set; }

        [DisplayName("Login")]
        [MaxLength(64)]
        public string Login { get; set; }

        [DisplayName("Password")]
        [MaxLength(64)]
        public string Password { get; set; }

        [DisplayName("Service URL")]
        [MaxLength(128)]
        public string ServiceUrl { get; set; }

        [DisplayName("Is active")]
        public bool Active { get; set; }

        // todo: move to ForEdit class when there will be some
        [HiddenInput]
        public bool CanDelete { get; set; }

    }

}
