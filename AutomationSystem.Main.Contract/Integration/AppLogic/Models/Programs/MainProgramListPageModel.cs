using System.Collections.Generic;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Contract.Integration.AppLogic.Models.Programs
{
    /// <summary>
    /// Main program list page model
    /// </summary>
    public class MainProgramListPageModel
    {

        public MainProgramFilter Filter { get; set; }
        public bool WasSearched { get; set; }
        public List<DropDownItem> Profiles { get; set; } = new List<DropDownItem>();
        public List<MainProgramListItem> Items { get; set; } = new List<MainProgramListItem>();

        // constructor
        public MainProgramListPageModel(MainProgramFilter filter)
        {
            Filter = filter ?? new MainProgramFilter();
        }

    }


}
