using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;
using System;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.Contacts.System.Emails.EmailParameterResolvers
{
    public class FormerStudentParameterResolver : IEmailParameterResolverWithBinding<FormerStudent>
    {
        public const string FullName = "{{FormerStudent.FullName}}";
        public const string FullNameCapital = "{{FormerStudent.FullNameCapital}}";

        private FormerStudent formerStudent;
        private readonly HashSet<string> supportedParameters;
        private readonly IEmailServiceHelper helper;

        public FormerStudentParameterResolver(IEmailServiceHelper helper)
        {
            this.helper = helper;
            supportedParameters = new HashSet<string>(new[] { FullName, FullNameCapital });
        }

        public bool IsSupportedParameters(string parameterNameWithBrackets)
        {
            return supportedParameters.Contains(parameterNameWithBrackets);
        }

        public void Bind(FormerStudent data)
        {
            formerStudent = data;
        }

        public string GetValue(LanguageEnum languageId, string parameterNameWithBrackets)
        {
            if (formerStudent == null)
            {
                throw new InvalidOperationException("FormerStudent was not binded.");
            }

            object resultObject;
            switch (parameterNameWithBrackets)
            {
                case FullName:
                    resultObject = MainTextHelper.GetFullName(formerStudent.Address.FirstName, formerStudent.Address.LastName);
                    break;

                case FullNameCapital:
                    resultObject = MainTextHelper.GetFullName(formerStudent.Address.FirstName, formerStudent.Address.LastName).ToUpper();
                    break;

                default:
                    return null;
            }

            var result = helper.EmailParameterConvertor.Convert(languageId, parameterNameWithBrackets, resultObject);
            return result;
        }
    }
}
