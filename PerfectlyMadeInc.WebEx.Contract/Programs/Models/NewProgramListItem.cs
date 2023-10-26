using System.ComponentModel;

namespace PerfectlyMadeInc.WebEx.Contract.Programs.Models
{
    /// <summary>
    /// Program partial list item
    /// </summary>
    public class NewProgramListItem
    {

        [DisplayName("ID")]
        public long ProgramOuterId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        public string ProgramUrl { get; set; }

    }
}
