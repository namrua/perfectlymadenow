using System.ComponentModel;
using System.Web.Mvc;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Integration.AppLogic.Models.Programs
{
    /// <summary>
    /// Main Program filter
    /// </summary>
    [Bind(Include = "IncludeUsed, ProfileId")]
    public class MainProgramFilter
    {

        [DisplayName("Profile")]
        [PickInputOptions(NoItemText = "no profile", Placeholder = "select profile")]
        public long? ProfileId { get; set; }

        [DisplayName("Include used")]
        public bool IncludeUsed { get; set; }

    }


}
