using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base
{
    /// <summary>
    /// Registration filter
    /// </summary>
    [Bind(Include = "ClassId, RegistrationState, RegistrationTypeIds")]
    public class RegistrationFilter
    {
        

        [DisplayName("Class ID")]
        public long? ClassId { get; set; }

        [DisplayName("Registration state")]
        [PickInputOptions(NoItemText = "all states", Placeholder = "all states")]
        public RegistrationState? RegistrationState { get; set; }

        [DisplayName("Registration types")]
        [PickInputOptions(ControlType = PickControlType.DropDownListSetInput, Placeholder = "add registration type")]
        public List<int> RegistrationTypeIds { get; set; }
        

        public List<RegistrationTypeEnum> ExcludedRegistrationTypeIds { get; set; }
        public bool? IsApproved { get; set; }        
        
        public List<RegistrationTypeEnum> RegistrationTypeIdsEnum
        {
            get => RegistrationTypeIds.Cast<RegistrationTypeEnum>().ToList();
            set => RegistrationTypeIds = value.Cast<int>().ToList();
        }
        
        public RegistrationFilter()
        {
            RegistrationTypeIds = new List<int>();
            ExcludedRegistrationTypeIds = new List<RegistrationTypeEnum>();
        }

    }

}
