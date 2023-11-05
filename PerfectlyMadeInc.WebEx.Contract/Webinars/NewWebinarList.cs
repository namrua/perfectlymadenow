using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfectlyMadeInc.WebEx.Contract.Webinars
{
    public class NewWebinarList
    {
        public long AccountId { get; set; }
        public List<NewWebinarListItem> Items { get; set; }

        // constructor
        public NewWebinarList()
        {
            Items = new List<NewWebinarListItem>();
        }
    }
}
