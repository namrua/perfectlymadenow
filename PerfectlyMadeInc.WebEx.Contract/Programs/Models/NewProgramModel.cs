using System.Collections.Generic;
using CorabeuControl.Components;

namespace PerfectlyMadeInc.WebEx.Contract.Programs.Models
{
    /// <summary>
    /// New Webex programs for list
    /// </summary>
    public class NewProgramModel
    {

        public List<PickerItem> Accounts { get; set; }

        // constructor
        public NewProgramModel()
        {
            Accounts = new List<PickerItem>();
        }

    }
}
