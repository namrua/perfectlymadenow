using System.ComponentModel;

namespace AutomationSystem.Shared.Contract.Localisation.AppLogic.Models
{
    /// <summary>
    /// Enum localisation list item
    /// </summary>
    public class EnumLocalisationListItem
    {

        // public properties
        [DisplayName("Item ID")]
        public int ItemId { get; set; }

        [DisplayName("Item name")]
        public string Name { get; set; }

        [DisplayName("Item Description")]
        public string Description { get; set; }

        public bool IsNameLocalised { get; set; }
        public bool IsDescriptionLocalised { get; set; }

    }

}
