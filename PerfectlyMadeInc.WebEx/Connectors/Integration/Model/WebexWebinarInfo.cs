using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfectlyMadeInc.WebEx.Connectors.Integration.Model
{
    public class WebexWebinarInfo
    {
        // public properties
        public List<ItemInfo> Items { get; set; }
        public class ItemInfo
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string WebLink { get; set; }
        }
    }
}
