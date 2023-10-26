namespace AutomationSystem.Shared.Contract.Enums.Data.Models
{
    /// <summary>
    /// Enum item filter
    /// </summary>
    public class EnumItemFilter
    {

        // enum item filter
        public int? Id { get; set; }              
        public string Name { get; set; }

        // constructor
        public EnumItemFilter(int? id = null, string name = null)
        {
            Id = id;
            Name = name;
        }

    }


}
