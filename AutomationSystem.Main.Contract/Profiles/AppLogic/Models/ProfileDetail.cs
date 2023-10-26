using System.ComponentModel;

namespace AutomationSystem.Main.Contract.Profiles.AppLogic.Models
{
    /// <summary>
    /// Profile detail
    /// </summary>
    public class ProfileDetail
    {

        [DisplayName("ID")]
        public long ProfileId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Moniker")]
        public string Moniker { get; set; }


        public bool CanDelete { get; set; }

    }


}
