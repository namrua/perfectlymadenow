using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Model
{

    /// <summary>
    /// Extends ClassType entity
    /// </summary>
    public partial class ClassType : IEnumItem
    {

        // entity id
        public int Id
        {
            get => (int)ClassTypeId;
            set => ClassTypeId = (ClassTypeEnum)value;
        }

    }

}
