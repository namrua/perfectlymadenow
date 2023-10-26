using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.System;
using CorabeuControl.Helpers;

namespace AutomationSystem.Shared.Core.Emails.System.EmailParameterResolvers
{
    /// <summary>
    /// Resolves default test values of parameters - can be overshadowed by another resolver
    /// </summary>
    public class TestParameterResolver : IEmailParameterResolver
    {

        // private fields
        private readonly IEmailParameterResolver underlyingResolver;
        private readonly IEmailServiceHelper helper;
        private readonly Dictionary<string, string> testValues;

        // constructor
        public TestParameterResolver(IEmailServiceHelper helper, IEmailParameterResolver underlyingResolver = null)
        {
            this.helper = helper;
            this.underlyingResolver = underlyingResolver ?? new EmptyParameterResolver();
            testValues = helper.EmailParameters.ToDictionary(x => TextHelper.DoubleBrackets(x.Name), y => y.TestValue);
        }


        // determines whether parameter name is supported in resolver
        public bool IsSupportedParameters(string paramName)
        {
            var result = underlyingResolver.IsSupportedParameters(paramName) 
                || testValues.ContainsKey(paramName);
            return result;
        }

        // gets value
        public string GetValue(LanguageEnum languageId, string paramName)
        {
            var result = underlyingResolver.GetValue(languageId, paramName);
            if (result != null) return result;

            if (!testValues.TryGetValue(paramName, out var testValue))
                return null;
            result = helper.EmailParameterConvertor.Convert(languageId, paramName, testValue);
            return result;
        }

    }

}
