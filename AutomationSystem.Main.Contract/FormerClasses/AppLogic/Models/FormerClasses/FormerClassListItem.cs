using AutomationSystem.Base.Contract.Enums;
using System;
using System.ComponentModel;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses
{
    public class FormerClassListItem
    {
        [DisplayName("ID")]
        public long FormerClassId { get; set; }

        [DisplayName("Type code")]
        public ClassTypeEnum ClassTypeId { get; set; }

        [DisplayName("Type")]
        public string ClassType { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [DisplayName("Start of class")]
        public DateTime EventStart { get; set; }

        [DisplayName("End of class")]
        public DateTime EventEnd { get; set; }
        

        [DisplayName("Year")] public int Year => EventStart.Year;
        [DisplayName("Date")] public string ClassDate { get; set; }
        [DisplayName("Date")] public string FullClassDate { get; set; }
        [DisplayName("Class")] public string ClassTitle { get; set; }
    }
}
