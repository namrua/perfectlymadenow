using System.Xml;
using System.Xml.Linq;

namespace PerfectlyMadeInc.WebEx.Connectors.Integration
{
    /// <summary>    
    /// Wraps response root element and namespace manager
    /// </summary>
    public class XmlResponseWrapper
    {

        // public properties
        public XElement Root { get; set; }
        public XmlNamespaceManager NamespaceManager { get; set; }

    }
}
