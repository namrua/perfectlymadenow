using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Shared.Core.Emails.System.EmailParameterResolvers
{
    /// <summary>
    /// Resolves parameters for localised constant texts - connects with localisation service
    /// </summary>
    public class LocalisedParameterResolver : IEmailParameterResolver
    {

        private const string localisationModule = "EmailTemplate";
        private const string parameterNamePattern = "{{{{Localised.{0}}}}}";

        private readonly IEmailServiceHelper helper;       
        private readonly Dictionary<string, string> parameterToLabelMap;

        // constructor
        public LocalisedParameterResolver(IEmailServiceHelper helper, IEnumerable<string> supportedTextsLabels)
        {
            this.helper = helper;
            parameterToLabelMap = supportedTextsLabels.ToDictionary(x => string.Format(parameterNamePattern, x), x => x);          
        }

        // determines whether parameter name is supported in resolver
        public bool IsSupportedParameters(string parameterNameWithBrackets)
        {
            return parameterToLabelMap.ContainsKey(parameterNameWithBrackets);
        }

        // gets value
        public string GetValue(LanguageEnum languageId, string parameterNameWithBrackets)
        {
            if (!parameterToLabelMap.TryGetValue(parameterNameWithBrackets, out var label))
                return null;
            var languageInfo = helper.LanguageInfoProvider.GetLanguageInfo(languageId);
            var resultObject = helper.LocalisationService.GetLocalisedString(localisationModule, label, languageInfo.Name);
            var result = helper.EmailParameterConvertor.Convert(languageId, parameterNameWithBrackets, resultObject);
            return result;
        }

    }

}
