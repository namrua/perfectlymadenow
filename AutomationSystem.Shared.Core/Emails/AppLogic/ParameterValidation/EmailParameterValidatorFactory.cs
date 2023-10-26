using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Shared.Contract.Emails.System.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Emails.AppLogic.ParameterValidation
{


    /// <summary>
    /// Validation factory creates email parameter validator
    /// </summary>
    public class EmailParameterValidatorFactory : IEmailParameterValidatorFactory
    {

        // gets validation by email template and parameters
        public IEmailParameterValidator GetValidatorByEmailTemplate(EmailTemplate template, IEnumerable<EmailParameter> parameters)
        {
            var paramMap = parameters.ToDictionary(x => x.EmailParameterId, y => y);
            var definition = new List<EmailParameterInfo>();
            foreach (var templateParam in template.EmailTemplateParameters)
            {
                var param = paramMap[templateParam.EmailParameterId];
                var paramDef = new EmailParameterInfo(param.Name, templateParam.IsRequired);
                definition.Add(paramDef);
            }
            var validator = new EmailParameterValidator(EmailConstants.ParameterRegexPattern, definition);
            return validator;
        }


        // gets validation by email template params
        public IEmailParameterValidator GetValidationByEmailTemplateParameters(
            IEnumerable<EmailTemplateParameter> templateParameters)
        {
            var definition = templateParameters.Select(templateParam => 
                new EmailParameterInfo(templateParam.EmailParameter.Name, templateParam.IsRequired)).ToList();
            var validator = new EmailParameterValidator(EmailConstants.ParameterRegexPattern, definition);
            return validator;
        }
        
    }

}
