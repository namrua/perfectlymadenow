using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;
using CorabeuControl.Helpers;

namespace AutomationSystem.Main.Core.Registrations.System.Emails.EmailParameterResolvers
{

    /// <summary>
    /// Resolves email parameters from Person to look like Class registration
    /// </summary>
    public class PersonAsRegistrationParameterResolver : IEmailParameterResolverWithBinding<Person>
    {
        
        private const string FullName = "{{Student.FullName}}";
        private const string FullNameCapital = "{{Student.FullNameCapital}}";        
        private const string Email = "{{Student.Email}}";
        private const string Type = "{{Student.Type}}";

        private Person person;
        private readonly HashSet<string> supportedParameters;
        private readonly IEmailServiceHelper helper;


        // constructor
        public PersonAsRegistrationParameterResolver(IEmailServiceHelper helper)
        {
            this.helper = helper;
            supportedParameters = new HashSet<string>(new[] { FullName, FullNameCapital, Email, Type });
        }



        // binds value to email parameters
        public void Bind(Person data)
        {
            person = data;
        }

        // determines whether parameter name is supported in resolver
        public bool IsSupportedParameters(string parameterNameWithBrackets)
        {
            return supportedParameters.Contains(parameterNameWithBrackets);
        }

        // gets value
        public string GetValue(LanguageEnum languageId, string parameterNameWithBrackets)
        {
            if (person == null)
                throw new InvalidOperationException("Person was not binded.");

            object resultObject;
            var langInfo = helper.LanguageInfoProvider.GetLanguageInfo(languageId);
            switch (parameterNameWithBrackets)
            {
                case FullName:
                    resultObject = MainTextHelper.GetFullName(person.Address.FirstName, person.Address.LastName);
                    break;

                case FullNameCapital:
                    resultObject = MainTextHelper.GetFullName(person.Address.FirstName, person.Address.LastName).ToUpper();
                    break;
               
                case Email:
                    resultObject = person.Email;
                    break;

                case Type:
                    resultObject = TextHelper.NoValueText;
                    break;

                default:
                    return null;
            }
            var result = helper.EmailParameterConvertor.Convert(languageId, parameterNameWithBrackets, resultObject);
            return result;
        }

    }

}
