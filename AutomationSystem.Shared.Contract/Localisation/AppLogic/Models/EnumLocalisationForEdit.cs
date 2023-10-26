using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.Localisation.AppLogic.Models
{
    /// <summary>
    /// Enum item detail
    /// </summary>
    public class EnumLocalisationForEdit
    {

        // public properties
        public IEnumItem Language { get; set; }
        public EnumType EnumType { get; set; }
       
        [DisplayName("System name")]
        public string SystemName { get; set; }

        [DisplayName("System description")]
        public string SystemDescription { get; set; }

        public EnumLocalisationForm Form { get; set; }

        // constructor
        public EnumLocalisationForEdit()
        {
            Form = new EnumLocalisationForm();
        }

    }

}
