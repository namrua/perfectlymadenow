using System.Collections.Generic;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Contract.Persons.AppLogic.Models
{
    /// <summary>
    /// Persons list page model
    /// </summary>
    public class PersonListPageModel
    {

        public PersonFilter Filter { get; set; }
        public List<DropDownItem> ProfilesForInsert { get; set; } = new List<DropDownItem>();
        public List<DropDownItem> ProfilesForFilter { get; set; } = new List<DropDownItem>();
        public bool WasSearched { get; set; }
        public List<PersonListItem> Items = new List<PersonListItem>();

        // constructor
        public PersonListPageModel(PersonFilter filter = null)
        {
            Filter = filter ?? new PersonFilter();
        }

    }

}
