using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Model
{
    /// <summary>
    /// Extends Country entity
    /// </summary>
    public partial class Country : IEnumItem
    {

        // entity id
        public int Id
        {
            get => (int)CountryId;
            set => CountryId = (CountryEnum)value;
        }
        
    }
}
