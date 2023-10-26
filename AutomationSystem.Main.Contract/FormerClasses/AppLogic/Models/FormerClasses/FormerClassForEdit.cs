using AutomationSystem.Base.Contract.Enums;
using System.Collections.Generic;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses
{
    /// <summary>
    /// Former class for edit
    /// </summary>
    public class FormerClassForEdit
    {

        public FormerClassForm Form { get; set; }
        public List<IEnumItem> ClassTypes { get; set; }
        public List<DropDownItem> Profiles { get; set; } = new List<DropDownItem>();

        // constructor
        public FormerClassForEdit()
        {
            Form = new FormerClassForm();
            ClassTypes = new List<IEnumItem>();
        }

    }

}
