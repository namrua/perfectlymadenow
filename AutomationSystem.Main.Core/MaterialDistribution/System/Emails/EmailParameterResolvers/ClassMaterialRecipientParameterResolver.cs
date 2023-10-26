using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.MaterialDistribution.System.Emails.EmailParameterResolvers
{
    /// <summary>
    /// Resolves email parameters for ClassMaterialRecipient entity
    /// </summary>
    public class ClassMaterialRecipientParameterResolver : IEmailParameterResolverWithBinding<ClassMaterialRecipient>
    {
        private const string DownloadUrl = "{{Materials.DownloadUrl}}";
        private const string Password = "{{Materials.Password}}";

        private ClassMaterialRecipient materialRecipient;
        private readonly HashSet<string> supportedParameters;
        private readonly IEmailServiceHelper helper;

        // constructor
        public ClassMaterialRecipientParameterResolver(IEmailServiceHelper helper)
        {
            this.helper = helper;
            supportedParameters = new HashSet<string>(new [] { DownloadUrl, Password });
        }

        // bind values for email parameters
        public void Bind(ClassMaterialRecipient data)
        {
            materialRecipient = data;
        }

        // determines whether parameter name is supported in resolver
        public bool IsSupportedParameters(string parameterNameWithBrackets)
        {
            return supportedParameters.Contains(parameterNameWithBrackets);
        }

        // gets value
        public string GetValue(LanguageEnum languageId, string parameterNameWithBrackets)
        {
            if (materialRecipient == null)
                throw new InvalidOperationException("Class registration materials was not binded.");

            object resultObject;
            switch (parameterNameWithBrackets)
            {
                case DownloadUrl:
                    resultObject = $"Materials?request={materialRecipient.RequestCode}";
                    break;

                case Password:
                    resultObject = materialRecipient.Password;
                    break;

                default:
                    return null;
            }
            var result = helper.EmailParameterConvertor.Convert(languageId, parameterNameWithBrackets, resultObject);
            return result;
        }
    }
}
