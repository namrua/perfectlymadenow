using System.ComponentModel;
using System.Web.Mvc;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Reports
{
    /// <summary>
    /// Class recipient selection form
    /// </summary>
    public class ClassRecipientSelectionForm
    {
        [HiddenInput]
        public long ClassId { get; set; }

        [HiddenInput]
        public ClassCommunicationType Type { get; set; }

        [DisplayName("Send to")]
        public long[] RecipientIds { get; set; } = new long[0];
    }
}