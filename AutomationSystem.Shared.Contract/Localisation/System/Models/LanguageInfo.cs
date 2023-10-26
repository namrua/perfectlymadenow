using System.Globalization;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Localisation.System.Models
{
    /// <summary>
    /// Extended language info
    /// </summary>
    public class LanguageInfo
    {

        // public properties
        public LanguageEnum LanguageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CultureInfo CultureInfo { get; set; }

        // constructor
        public LanguageInfo() { }
        public LanguageInfo(IEnumItem language)
        {
            LanguageId = (LanguageEnum)language.Id;
            Name = language.Name;
            Description = language.Description;
            CultureInfo = new CultureInfo(Name);
        }

    }
}
