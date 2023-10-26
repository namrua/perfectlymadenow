using System.Collections.Generic;
using System.Linq;

namespace AutomationSystem.Shared.Core.HtmlValidation.System.Models
{
    /// <summary>
    /// Definition of html validation (white list of tags and attributes)
    /// </summary>
    public class HtmlValidationDefinition
    {

        // public properties
        public Dictionary<string, HtmlTagValidationDefinition> TagWhiteList { get; set; }
        public HashSet<string> AttributeWhiteList { get; set; }

        // constructor
        public HtmlValidationDefinition()
        {
            TagWhiteList = new Dictionary<string, HtmlTagValidationDefinition>();
            AttributeWhiteList = new HashSet<string>();
        }

        #region statics

        // creates new item
        public static HtmlValidationDefinition New()
        {
            return new HtmlValidationDefinition();
        }

        #endregion

        #region extensions

        // adds html tag
        public HtmlValidationDefinition AddTags(params string[] tags)
        {
            foreach (var tagName in tags.Select(x => x.ToLower().Trim()))
                if (!TagWhiteList.ContainsKey(tagName))
                    TagWhiteList[tagName] = new HtmlTagValidationDefinition(tagName);
            return this;
        }

        // adds html tag with attributes
        public HtmlValidationDefinition AddTagWithAttributes(string tagName, params string[] attributes)
        {
            // adds html tag
            tagName = tagName.ToLower().Trim();
            HtmlTagValidationDefinition tagDefinition;
            if (!TagWhiteList.TryGetValue(tagName, out tagDefinition))
                TagWhiteList[tagName] = tagDefinition = new HtmlTagValidationDefinition(tagName);

            // adds attributes
            foreach (var attribute in attributes.Select(x => x.ToLower().Trim()))
                tagDefinition.AttributeWhiteList.Add(attribute);
            return this;
        }

        // adds attributes to whitelist
        public HtmlValidationDefinition AddAttributes(params string[] attributes)
        {
            AttributeWhiteList.UnionWith(attributes.Select(x => x.ToLower().Trim()));
            return this;
        }

        #endregion

    }

}
