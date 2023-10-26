using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutomationSystem.Shared.Core.Emails.AppLogic.ParameterValidation
{
    /// <summary>
    /// Validates email input for email parametes
    /// </summary>
    public class EmailParameterValidator : IEmailParameterValidator
    {

        // private fields
        private readonly string regexPattern;
        private readonly List<EmailParameterInfo> parameters;
        
        // constructor
        public EmailParameterValidator(string regexPattern, List<EmailParameterInfo> parameters)
        {
            this.regexPattern = regexPattern;
            this.parameters = parameters;            
        }


        // validates text for specified validation
        public List<string> Validate(EmailInputType type, string text, EmailValidationType validationType)
        {
            var result = new List<string>();
            var parametersInText = GetParametersInText(text);
            if (validationType.HasFlag(EmailValidationType.Allowed))
                result.AddRange(GetInvalidParameters(parametersInText)
                    .Select(x => $"{type} contains invalid parameter {x}"));
            if (validationType.HasFlag(EmailValidationType.Required))
                result.AddRange(GetMissingRequiredParameters(parametersInText)
                    .Select(x => $"{type} does not contain required parameter {x}"));
            return result;
        }


        #region validators

        // gets invalid parameters
        private List<string> GetInvalidParameters(HashSet<string> parameteresInText)
        {
            var allowedParams = new HashSet<string>(parameters.Select(x => x.NameWithBrackets));
            var result = parameteresInText.Where(x => !allowedParams.Contains(x)).ToList();
            return result;
        }

        // get missing required parameters
        private List<string> GetMissingRequiredParameters(HashSet<string> parameteresInText)
        {
            var result = parameters
                .Where(x => x.IsRequired && !parameteresInText.Contains(x.NameWithBrackets))
                .Select(x => x.NameWithBrackets).ToList();
            return result;
        }

        #endregion


        #region private helpers

        // gets parameters in text
        private HashSet<string> GetParametersInText(string text)
        {
            var regex = new Regex(regexPattern);
            var result = new HashSet<string>();
            var match = regex.Match(text);
            while (match.Success)
            {
                result.Add(match.Value);
                match = match.NextMatch();
            }
            return result;
        }

        #endregion

    }

}
