using System.Collections.Generic;
using AutomationSystem.Main.Model;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base
{
    /// <summary>
    /// Encapsulates classes for list
    /// </summary>
    public class ClassesForList
    {
        public ClassFilter Filter { get; set; }
        public List<ClassListItem> Items { get; set; } = new List<ClassListItem>();
        public List<DropDownItem> Profiles { get; set; } = new List<DropDownItem>();
        public List<ClassState> ClassStates { get; set; } = new List<ClassState>();
        public List<ClassCategory> ClassCategories { get; set; } = new List<ClassCategory>();
        public bool WasSearched { get; set; }

        // constructor
        public ClassesForList(ClassFilter filter = null)
        {
            Filter = filter ?? new ClassFilter();
        }
    }
}