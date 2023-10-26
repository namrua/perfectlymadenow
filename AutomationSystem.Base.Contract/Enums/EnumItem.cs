
namespace AutomationSystem.Base.Contract.Enums
{
    /// <summary>
    /// Generic enum item
    /// </summary>
    public class EnumItem : IEnumItem
    {
        
        // public properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // constructor
        public EnumItem() { }
        public EnumItem(int id, string name = null, string description = null)
        {
            Id = id;
            Name = name;
            Description = description;
        }

    }

}