using System.Collections.Generic;
using AutomationSystem.Shared.Core.HtmlValidation.System.Models;

namespace AutomationSystem.Shared.Core.HtmlValidation.System
{
    /// <summary>
    /// Factory for html validators
    /// </summary>
    public class HtmlValidatorFactory : IHtmlValidatorFactory
    {

        // public constants
        public const string AppLocalisationDefinition = "AppLocalisationDefinition";
        public const string EmailTemplateDefinition = "EmailTemplateDefinition";

        // private fields
        private readonly Dictionary<string, HtmlValidationDefinition> definitions;


        // constructor
        public HtmlValidatorFactory()
        {
            definitions = new Dictionary<string, HtmlValidationDefinition>();
            InitializeDefinitions();
        }

        // returns app localisation validator
        public IHtmlValidator GetAppLocalisationValidator()
        {
            return new HtmlValidator(definitions[AppLocalisationDefinition]);
        }

        // gets email template validator
        public IHtmlValidator GetEmailTemplateValidator()
        {
            return new HtmlValidator(definitions[EmailTemplateDefinition]);
        }

        #region private fields

        // initializes definitions
        private void InitializeDefinitions()
        {
            // app localisation definition
            definitions[AppLocalisationDefinition] = HtmlValidationDefinition.New()
                .AddTags("h1", "h2", "h3", "h4", "h5", "h6")
                .AddTags("em", "strong")
                .AddTags("p", "br")
                .AddTagWithAttributes("hr", "class")
                .AddTagWithAttributes("a", "href", "target");

            // email definition
            definitions[EmailTemplateDefinition] = HtmlValidationDefinition.New()
                .AddAttributes("id", "class", "style")                               
                .AddTags("h1", "h2", "h3", "h4", "h5", "h6")
                .AddTags("em", "strong", "b", "i", "mark", "small", "del", "ins", "sub", "sup")
                .AddTags("p", "br", "hr")
                .AddTags("div", "span")               
                .AddTagWithAttributes("a", "href", "target")
                .AddTagWithAttributes("img", "src", "alt", "width", "height")
                .AddTags("table", "tr", "th")
                .AddTagWithAttributes("td", "colspan", "rowspan");
        }        

        #endregion

    }

}
