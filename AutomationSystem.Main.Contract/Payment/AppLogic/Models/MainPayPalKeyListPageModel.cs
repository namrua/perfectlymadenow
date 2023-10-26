using System.Collections.Generic;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Contract.Payment.AppLogic.Models
{
    /// <summary>
    /// PayPal keys list page model
    /// </summary>
    public class MainPayPalKeyListPageModel
    {

        public MainPayPalKeyFilter Filter { get; set; }
        public bool WasSearched { get; set; }
        public List<DropDownItem> Profiles { get; set; } = new List<DropDownItem>();
        public List<MainPayPalKeyListItem> Items { get; set; } = new List<MainPayPalKeyListItem>();


        // constructor
        public MainPayPalKeyListPageModel(MainPayPalKeyFilter filter)
        {
            Filter = filter ?? new MainPayPalKeyFilter();
        }

    }

}
