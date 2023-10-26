using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Shared.Core.Emails.System.EmailParameterResolvers
{
    /// <summary>
    /// Resolves TextMap.XXX parameters from dictionary
    /// </summary>
    public class TextMapParameterResolver : IEmailParameterResolverWithBinding<IDictionary<string, object>>
    {

        // constants
        private const string ParameterNamePattern = "{{{{TextMap.{0}}}}}";

        // private fields
        private Dictionary<string, object> textMapValues;        
        private HashSet<string> supportedParameters;
        private readonly IEmailServiceHelper helper;

        // constructor
        public TextMapParameterResolver(IEmailServiceHelper helper)
        {
            this.helper = helper;
            supportedParameters = new HashSet<string>();
            textMapValues = new Dictionary<string, object>();
        }


        // binds value to email parameters
        public void Bind(IDictionary<string, object> data)
        {
            supportedParameters = new HashSet<string>(data.Keys.Select(x => string.Format(ParameterNamePattern, x)));
            textMapValues = data.ToDictionary(x => string.Format(ParameterNamePattern, x.Key), y => y.Value);
        }


        // determines whether parameter name is supported in resolver
        public bool IsSupportedParameters(string parameterNameWithBrackets)
        {
            return supportedParameters.Contains(parameterNameWithBrackets);
        }


         // gets value
        public string GetValue(LanguageEnum languageId, string parameterNameWithBrackets)
        {
            if (!textMapValues.TryGetValue(parameterNameWithBrackets, out var resultObject))
                return null;
            var result = helper.EmailParameterConvertor.Convert(languageId, parameterNameWithBrackets, resultObject);
            return result;
        }

    }

}
