using System.Collections.Generic;
using System.ComponentModel;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Actions
{
    /// <summary>
    /// Class action list
    /// </summary>
    public class ClassActionPageModel
    {
        [DisplayName("Class")]
        public ClassShortDetail Class { get; set; }

        [DisplayName("Class action types")]
        public List<PickerItem> ClassActionTypes { get; set; }

        [DisplayName("Actions")]
        public List<ClassActionListItem> ClassActions { get; set; }

        // constructor
        public ClassActionPageModel()
        {
            Class = new ClassShortDetail();
            ClassActions = new List<ClassActionListItem>();
            ClassActionTypes = new List<PickerItem>();
        }
    }
}