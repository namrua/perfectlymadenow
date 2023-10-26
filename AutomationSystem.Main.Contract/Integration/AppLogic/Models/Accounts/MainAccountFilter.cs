using System.ComponentModel;
using System.Web.Mvc;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Integration.AppLogic.Models.Accounts
{
    /// <summary>
    /// Main Account filter 
    /// </summary>
    [Bind(Include = "ProfileId")]
    public class MainAccountFilter
    {

        [DisplayName("Profile")]
        [PickInputOptions(NoItemText = "no profile", Placeholder = "select profile")]
        public long? ProfileId { get; set; }

    }

}
