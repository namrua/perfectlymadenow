using CorabeuControl.Helpers;

namespace AutomationSystem.Shared.Core.Emails.AppLogic.ParameterValidation
{
    /// <summary>
    /// Encapsulates information about email parameter
    /// </summary>
    public class EmailParameterInfo
    {

        // public properties
        public string Name { get; set; }
        public string NameWithBrackets { get; set; }
        public bool IsRequired { get; set; }

        // constructor
        public EmailParameterInfo(string name, bool isRequired)
        {
            Name = name;
            NameWithBrackets = TextHelper.DoubleBrackets(name);
            IsRequired = isRequired;
        }

    }

}
