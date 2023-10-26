using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Model
{
    /// <summary>
    /// Extends TimeZone entity
    /// </summary>
    public partial class TimeZone : IEnumItem
    {

        // entity id
        public int Id
        {
            get => (int)TimeZoneId;
            set => TimeZoneId = (TimeZoneEnum)value;
        }

    }
}


