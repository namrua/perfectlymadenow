using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfectlyMadeInc.WebEx.Contract.Webinars
{
    public class NewWebinarListItem
    {
        [DisplayName("ID")]
        public string Id { get; set; }

        [DisplayName("Name")]
        public string Title { get; set; }

        public string WebLink { get; set; }
    }
}
