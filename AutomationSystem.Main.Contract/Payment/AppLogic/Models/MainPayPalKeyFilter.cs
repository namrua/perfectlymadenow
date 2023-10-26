using System.ComponentModel;
using System.Web.Mvc;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Payment.AppLogic.Models
{
    /// <summary>
    /// Main PayPalKey filter 
    /// </summary>
    [Bind(Include = "ProfileId")]
    public class MainPayPalKeyFilter    
    {

        [DisplayName("Profile")]
        [PickInputOptions(NoItemText = "no profile", Placeholder = "select profile")]
        public long? ProfileId { get; set; }

    }

}
