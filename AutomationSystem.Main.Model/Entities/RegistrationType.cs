using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Model
{

    /// <summary>
    /// Extends RegistrationType entity
    /// </summary>
    public partial class RegistrationType : IEnumItem
    {

        // entity id
        public int Id
        {
            get => (int)RegistrationTypeId;
            set => RegistrationTypeId = (RegistrationTypeEnum)value;
        }

    }

}
