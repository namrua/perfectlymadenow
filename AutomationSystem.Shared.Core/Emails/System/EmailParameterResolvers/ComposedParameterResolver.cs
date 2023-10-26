using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Shared.Core.Emails.System.EmailParameterResolvers
{
    /// <summary>
    /// Composed parameter resolver - aggregates collection of parameter resolvers
    /// </summary>
    public class ComposedParameterResolver : IEmailParameterResolver
    {

        // private fields      
        private readonly List<IEmailParameterResolver> resolvers;

        // constructor
        public ComposedParameterResolver(params IEmailParameterResolver[] resolvers)
        {
            this.resolvers = resolvers.ToList();
        }

        // determines whether parameter name is supported in resolver
        public bool IsSupportedParameters(string parameterNameWithBrackets)
        {
            var result = resolvers.Any(x => x.IsSupportedParameters(parameterNameWithBrackets));
            return result;
        }

        // gets value
        public string GetValue(LanguageEnum languageId, string parameterNameWithBrackets)
        {
            var resolver = resolvers.FirstOrDefault(x => x.IsSupportedParameters(parameterNameWithBrackets));
            var result = resolver?.GetValue(languageId, parameterNameWithBrackets);
            return result;
        }

    }

}
