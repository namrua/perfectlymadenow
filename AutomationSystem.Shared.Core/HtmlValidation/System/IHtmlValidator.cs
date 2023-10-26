using System.Collections.Generic;

namespace AutomationSystem.Shared.Core.HtmlValidation.System
{
    /// <summary>
    /// Html validator
    /// </summary>
    public interface IHtmlValidator
    {

        // validates html string, returns list of error messages
        List<string> Validate(string html, bool checkDoctypeHtml = false);

    }

}
