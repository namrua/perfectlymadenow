using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses
{
    /// <summary>
    /// Encapsulates list of former classes with filter
    /// </summary>
    public class FormerClassesForList
    {

        public FormerClassFilter Filter { get; set; }
        public List<IEnumItem> ClassTypes { get; set; }
        public List<FormerClassListItem> Items { get; set; }
        public bool WasSearched { get; set; }                           // determines whether searching was executed

        public bool CanInsert { get; set; }

        // constructor
        public FormerClassesForList(FormerClassFilter filter)
        {
            Filter = filter ?? new FormerClassFilter();
            ClassTypes = new List<IEnumItem>();
            Items = new List<FormerClassListItem>();
        }

    }

}
