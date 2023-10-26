using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Model
{

    /// <summary>
    /// Extends Language entity
    /// </summary>
    public partial class Language : IEnumItem
    {

        // entity id
        public int Id
        {
            get => (int)LanguageId;
            set => LanguageId = (LanguageEnum)value;
        }

    }

}
