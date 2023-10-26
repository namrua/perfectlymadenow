using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Incidents.System.Emails.EmailParameterResolvers
{
    /// <summary>
    /// Resolves email parameters from incident entity
    /// </summary>
    public class IncidentParameterResolver : IEmailParameterResolverWithBinding<Incident>
    {
        private const string Message = "{{Incident.Message}}";
        private const string Description = "{{Incident.Description}}";
        private const string Occurred = "{{Incident.Occurred}}";       
        private const string DetailUrl = "{{Incident.DetailUrl}}";

        private Incident incident;
        private readonly HashSet<string> supportedParameters;
        private readonly IEmailServiceHelper helper;        

        public IncidentParameterResolver(IEmailServiceHelper helper)
        {
            this.helper = helper;
            supportedParameters = new HashSet<string>(new [] { Message, Description, Occurred, DetailUrl });
        }

        public void Bind(Incident data)
        {
            incident = data;
        }

        public bool IsSupportedParameters(string parameterNameWithBrackets)
        {
            return supportedParameters.Contains(parameterNameWithBrackets);
        }

        public string GetValue(LanguageEnum languageId, string parameterNameWithBrackets)
        {
            if (incident == null)
                throw new InvalidOperationException("Incident was not binded");
            object resultObject;
            switch (parameterNameWithBrackets)
            {
                case Message:
                    resultObject = incident.Message;
                    break;

                case Description:
                    resultObject = incident.Description;
                    break;              

                case Occurred:
                    resultObject = incident.Occurred;
                    break;

                case DetailUrl:
                    resultObject = $"Incident/Detail/{incident.IncidentId}";
                    break;

                default:
                    return null;
            }
            var result = helper.EmailParameterConvertor.Convert(languageId, parameterNameWithBrackets, resultObject);
            return result;
        }
    }
}
