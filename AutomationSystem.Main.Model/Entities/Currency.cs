using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Model
{
    /// <summary>
    /// Extends Currency entity
    /// </summary>
    public partial class Currency : IEnumItem
    {

        // entity id
        public int Id
        {
            get => (int)CurrencyId;
            set => CurrencyId = (CurrencyEnum)value;
        }

    }
}

