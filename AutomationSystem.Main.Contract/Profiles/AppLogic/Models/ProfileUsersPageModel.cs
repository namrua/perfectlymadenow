using System.Collections.Generic;
using System.ComponentModel;
using AutomationSystem.Shared.Contract.Identities.AppLogic.Models;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Profiles.AppLogic.Models
{

    /// <summary>
    /// Encapsulates Profile's Users page model
    /// </summary>
    public class ProfileUsersPageModel 
    {

        public long ProfileId { get; set; }
        public List<UserShortDetail> AssignedUsers { get; set; }
        public List<UserShortDetail> UnassignedUsers { get; set; }

        [DisplayName("Search user")]
        [PickInputOptions(ControlType = PickControlType.TypeaheadDropDownInput, Placeholder = "search user")]
        public int? UserId { get; set; }

    }

}
