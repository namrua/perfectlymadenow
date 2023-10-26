using System.ComponentModel;
using System.Web.Mvc;

namespace PerfectlyMadeInc.WebEx.Contract.Programs.Models
{
    /// <summary>
    /// Event list item
    /// </summary>
    public class EventListItem
    {
        [HiddenInput]
        public long Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("SessionId")]
        public long SessionId { get; set; }
    }
}
