using System.Collections.Generic;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using CorabeuControl.Helpers;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base
{
    /// <summary>
    /// Registrations for list
    /// </summary>
    public class RegistrationsForList
    {
        
        public RegistrationFilter Filter { get; set; }
        public ClassShortDetail Class { get; set; }
        public List<RegistrationListItem> Items { get; set; }        
        public List<RegistrationState> RegistratonState { get; set; }
        public IPickerItemHelper<int> RegistrationTypes { get; set; }
        public bool WasSearched { get; set; }
        
        public RegistrationsForList(RegistrationFilter filter = null)
        {
            Filter = filter ?? new RegistrationFilter();
            Class = new ClassShortDetail();
            Items = new List<RegistrationListItem>();
            RegistratonState = new List<RegistrationState>();
            RegistrationTypes = new PickerItemHelper<int>();
        }

    }

}
