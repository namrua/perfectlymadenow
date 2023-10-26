using System.Collections.Generic;

namespace AutomationSystem.Shared.Core.HtmlValidation.System.Models
{
    /// <summary>
    /// Definition of html tag validation (white list attributes)
    /// </summary>
    public class HtmlTagValidationDefinition
    {

        // public properties
        public string TagName { get; set; }
        public HashSet<string> AttributeWhiteList { get; set; }

        // constructor
        public HtmlTagValidationDefinition(string tagName)
        {
            TagName = tagName;
            AttributeWhiteList = new HashSet<string>();
        }

    }

}
