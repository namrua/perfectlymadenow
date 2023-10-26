using System;
using System.Text;
using System.Text.RegularExpressions;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Shared.Core.Emails.System
{
    /// <summary>
    /// Resolves emailt texts
    /// </summary>
    public class EmailTextResolver : IEmailTextResolver
    {

        // private fields
        private readonly Regex regex;
        private readonly IEmailParameterResolver paramResolver;


        // constructor
        public EmailTextResolver(string regexPattern, IEmailParameterResolver paramResolver)
        {
            regex = new Regex(regexPattern);
            this.paramResolver = paramResolver;
        }

       

        // gets email text
        public string GetText(LanguageEnum languageId, string templateText)
        {            
            int cursor = 0;
            var resultBuilder = new StringBuilder();
            var match = regex.Match(templateText);
            while (match.Success)
            {
                resultBuilder.Append(GetPrefix(templateText, match, ref cursor));
                resultBuilder.Append(GetParamValue(languageId, match.Value));
                match = match.NextMatch();
            }
            resultBuilder.Append(GetSuffix(templateText, cursor));
            var result = resultBuilder.ToString();
            return result;
        }
                
        #region private fields

        // gets unprocessed text prefix before match, set cursor to next text after parameter
        private string GetPrefix(string templateText, Match match, ref int cursor)
        {
            var result = templateText.Substring(cursor, match.Index - cursor);
            cursor = match.Index + match.Length;
            return result;
        }

        // gets param value
        private string GetParamValue(LanguageEnum languageId, string paramName)
        {
            if (!paramResolver.IsSupportedParameters(paramName))
                throw new ArgumentException($"Email parameter {paramName} is not supported");
            var result = paramResolver.GetValue(languageId, paramName) ?? "";
            return result;
        }

        // gets unprocessed text suffix after cursor
        private string GetSuffix(string templateText, int cursor)
        {
            if (cursor >= templateText.Length) return "";
            var result = templateText.Substring(cursor);
            return result;
        }

        #endregion

    }

}
