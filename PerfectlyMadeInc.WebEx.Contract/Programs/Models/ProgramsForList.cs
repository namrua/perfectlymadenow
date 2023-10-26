using System.Collections.Generic;

namespace PerfectlyMadeInc.WebEx.Contract.Programs.Models
{
    /// <summary>
    /// WebEx program for list
    /// </summary>
    public class ProgramsForList
    {
        public ProgramFilter Filter { get; set; }
        public bool WasSearched { get; set; }
        public List<ProgramListItem> Items { get; set; }

        // constructor
        public ProgramsForList(ProgramFilter filter = null)
        {
            Filter = filter ?? new ProgramFilter();
            Items = new List<ProgramListItem>();
        }
    }
}
