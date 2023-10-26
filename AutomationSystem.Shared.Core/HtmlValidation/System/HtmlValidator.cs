using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using AutomationSystem.Shared.Core.HtmlValidation.System.Models;

namespace AutomationSystem.Shared.Core.HtmlValidation.System
{
    /// <summary>
    /// Html validator
    /// </summary>
    public class HtmlValidator : IHtmlValidator
    {

        // constants
        private const string docTypeHtmlString = "<!doctype html>";

        // private fields
        private readonly HtmlValidationDefinition definition;

        // constructor
        public HtmlValidator(HtmlValidationDefinition definition)
        {
            this.definition = definition;
        }


        // validates html string, returns list of error messages
        public List<string> Validate(string html, bool checkDoctypeHtml = false)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(html))
                return result;

            html = html.Trim().ToLower();
            if (checkDoctypeHtml)
                html = RemoveDocType(html);
            html = $"<root>{html}</root>";
            XElement rootElem;
            try
            {
                rootElem = XElement.Parse(html);
            }
            catch (XmlException)
            {
                result.Add("There is invalid syntax of HTML.");
                return result;
            }
            ValidateInnerTags(rootElem, result);
            return result;
        }

        #region private fields

        // validates inner tags
        private void ValidateInnerTags(XElement elem, List<string> errorMessages)
        {
            foreach (var innerElem in elem.Elements())
            {
                var elemName = innerElem.Name.LocalName;
                HtmlTagValidationDefinition tagDefinition;
                if (!definition.TagWhiteList.TryGetValue(elemName, out tagDefinition))                
                    errorMessages.Add($"Tag <{elemName}> is not allowed.");
                                 
                ValidateAttributes(innerElem, tagDefinition, errorMessages);
                ValidateInnerTags(innerElem, errorMessages);
            }
        }

        // validates tag attributes
        private void ValidateAttributes(XElement elem, HtmlTagValidationDefinition tagDefinition, List<string> errorMessages)
        {
            foreach (var attribute in elem.Attributes())
            {
                var attrName = attribute.Name.LocalName;
                if (definition.AttributeWhiteList.Contains(attrName) || tagDefinition.AttributeWhiteList.Contains(attrName)) continue;
                errorMessages.Add($"Attribute '{attrName}' of tag <{elem.Name.LocalName}> is not allowed.");
            }
        }

        // removes doctype fragment
        private string RemoveDocType(string html)
        {
            return html.StartsWith(docTypeHtmlString) ? html.Substring(docTypeHtmlString.Length).Trim() : html;
        }

        #endregion

    }

}
