using System.Collections.Generic;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Contract.Integration.AppLogic.Models.Accounts
{
    /// <summary>
    /// Main Account list page model
    /// </summary>
    public class MainAccountListPageModel
    {

        public MainAccountFilter Filter { get; set; }
        public bool WasSearched { get; set; }
        public List<DropDownItem> Profiles { get; set; } = new List<DropDownItem>();
        public List<MainAccountListItem> Items { get; set; } = new List<MainAccountListItem>();

        // constructor
        public MainAccountListPageModel(MainAccountFilter filter)
        {
            Filter = filter ?? new MainAccountFilter();
        }

    }

}
