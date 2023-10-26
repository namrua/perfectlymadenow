using System;
using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base
{
    /// <summary>
    /// Class list item
    /// </summary>
    public class ClassListItem
    {
        [DisplayName("ID")]
        public long ClassId { get; set; }     

        [DisplayName("Class state")]
        public ClassState ClassState { get; set; }

        [DisplayName("Type of class code")]
        public ClassTypeEnum ClassTypeId { get; set; }

        [DisplayName("Type of class")]
        public string ClassType { get; set; }

        [DisplayName("Class category code")]
        public ClassCategoryEnum ClassCategoryId { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [DisplayName("Time zone")]
        public string TimeZone { get; set; }

        [DisplayName("Language")]
        public string OriginLanguage { get; set; }

        [DisplayName("Translation")]
        public string TransLanguage { get; set; }

        [DisplayName("Start of registration")]
        public DateTime? RegistrationStart { get; set; }

        [DisplayName("End of registration")]
        public DateTime RegistrationEnd { get; set; }

        [DisplayName("Start of class")]
        public DateTime EventStart { get; set; }

        [DisplayName("End of class")]
        public DateTime EventEnd { get; set; }

        [DisplayName("Environment")]
        public EnvironmentTypeEnum EnvironmentTypeId { get; set; }

        [DisplayName("Profile ID")]
        public long ProfileId { get; set; }

        [DisplayName("Profile")]
        public string Profile { get; set; }

        [DisplayName("Year")] public int Year => EventStart.Year;      

        [DisplayName("Title")]
        public string Title { get; set; }

        public bool ShowOnlyDate { get; set; }
    }
}