using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Localisation.System;
using AutomationSystem.Shared.Model;
using CorabeuControl.Helpers;

namespace AutomationSystem.Shared.Core.Emails.System
{
    /// <summary>
    /// converts parameters values
    /// </summary>
    public class EmailParameterConvertor : IEmailParameterConvertor
    {
      
        // private fields
        private readonly Dictionary<string, EmailParameterTypeEnum> parameterTypeMap;
        private readonly ILanguageInfoPovider languageInfoProvider;
        private readonly string baseAddress;
         

        // constructor
        public EmailParameterConvertor(IEnumerable<EmailParameter> parameters, 
            ILanguageInfoPovider languageInfoProvider)
        {           
            this.languageInfoProvider = languageInfoProvider;
            parameterTypeMap = parameters.ToDictionary(x => TextHelper.DoubleBrackets(x.Name), y => y.EmailParameterTypeId);
            baseAddress = ConfigurationManager.AppSettings["BaseUrlAddress"];           
        }

        // convert parameter value
        public string Convert(LanguageEnum languageId, string parameterNameWithBrackets, object parameterValue)
        {
            var type = GetType(parameterNameWithBrackets);                     
            switch (type)
            {
                // text   
                case EmailParameterTypeEnum.Text:
                    return parameterValue.ToString();    
                
                // multiline
                case EmailParameterTypeEnum.Multiline:
                    // return TextHelper.ReplaceNewLines(parameterValue.ToString()).ToString(); // EMAIL HTML - here is switcher!
                    return parameterValue.ToString();
                    
                // gets date time
                case EmailParameterTypeEnum.DateTime:
                    var dt = GetDateTime(parameterValue);
                    return TextHelper.GetStringDateTime(dt, languageInfoProvider.GetLanguageInfo(languageId).CultureInfo);

                // URL
                case EmailParameterTypeEnum.URL:
                    return baseAddress + parameterValue;

                // invalid type
                default:
                    throw new ArgumentException($"Unknown email parameter type {type}");
            }
        }

        #region private fields

        // gets type
        private EmailParameterTypeEnum GetType(string parameterNameWithBrackets)
        {
            EmailParameterTypeEnum result = 0;
            if(!parameterTypeMap.TryGetValue(parameterNameWithBrackets, out result))
                throw new ArgumentException($"Unknow email parameter {parameterNameWithBrackets}");
            return result;
        }

        // converts to datetime
        private DateTime? GetDateTime(object value)
        {
            if (value == null)
                return null;
            if (value is DateTime)
                return (DateTime) value;
            var stringValue = value.ToString();
            if (stringValue == string.Empty)
                return null;                            
            DateTime dt;
            if (!DateTime.TryParse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                throw new ArgumentException($"Invalid DateTime format {dt} of parameter");
            return dt;
        }               
        
        #endregion

    }

}
