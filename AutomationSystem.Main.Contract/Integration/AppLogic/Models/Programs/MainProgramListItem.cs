using System.ComponentModel;

namespace AutomationSystem.Main.Contract.Integration.AppLogic.Models.Programs
{
    /// <summary>
    /// Main Program list item
    /// </summary>
    public class MainProgramListItem
    {
        public long Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Account")]
        public string AccountName { get; set; }

        [DisplayName("Profile")]
        public string Profile { get; set; }

    }


}
