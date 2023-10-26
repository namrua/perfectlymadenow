using System.ComponentModel;

namespace AutomationSystem.Main.Contract.Profiles.AppLogic.Models
{
    /// <summary>
    /// Profile list item
    /// </summary>
    public class ProfileListItem
    {

        [DisplayName("ID")]
        public long ProfileId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Moniker")]
        public string Moniker { get; set; }
       
    }


}
