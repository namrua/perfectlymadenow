using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Localisation.AppLogic.Models
{
    /// <summary>
    /// Enum type list item
    /// </summary>
    public class EnumTypeListItem
    {

        // public properties
        [DisplayName("Enum type ID")]
        public EnumTypeEnum EnumTypeId { get; set; }

        [DisplayName("Enum type code")]
        public string EnumTypeCode { get; set; }

        [DisplayName("Enum type description")]
        public string EnumType { get; set; }

    }

}
