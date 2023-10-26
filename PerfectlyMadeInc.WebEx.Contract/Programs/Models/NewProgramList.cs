using System.Collections.Generic;

namespace PerfectlyMadeInc.WebEx.Contract.Programs.Models
{
    /// <summary>
    /// New WebEx program list
    /// </summary>
    public class NewProgramList
    {

        public long AccountId { get; set; }
        public List<NewProgramListItem> Items { get; set; }

        // constructor
        public NewProgramList()
        {
            Items = new List<NewProgramListItem>();
        }

    }
}
