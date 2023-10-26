using System.ComponentModel;

namespace AutomationSystem.Main.Contract.Integration.AppLogic.Models.Accounts
{
    /// <summary>
    /// Main Account list item
    /// </summary>
    public class MainAccountListItem
    {

        public long ConferenceAccountId { get; set; }
        public long AccountId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Site name")]
        public string SiteName { get; set; }

        [DisplayName("Login")]
        public string Login { get; set; }

        [DisplayName("Is active")]
        public bool Active { get; set; }

        [DisplayName("Profile")]
        public string Profile { get; set; }

    }

}
