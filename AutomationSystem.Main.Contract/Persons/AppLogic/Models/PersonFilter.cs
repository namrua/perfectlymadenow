using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Persons.AppLogic.Models
{
    /// <summary>
    /// Person filter
    /// </summary>
    [Bind(Include = "Name, Contact, ProfileId")]
    public class PersonFilter
    {

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Contact")]
        public string Contact { get; set; }

        [DisplayName("Profile")]
        [PickInputOptions(NoItemText = "no selection", Placeholder = "select profile")]
        public long? ProfileId { get; set; }

        // filtering by list of profile ids - null, no filter
        public List<long> ProfileIds { get; set; }

        // determines whether default profile should be included into selection
        // by default it is true to not affect the selection by default
        public bool IncludeDefaultProfile { get; set; } = true;

    }

}
