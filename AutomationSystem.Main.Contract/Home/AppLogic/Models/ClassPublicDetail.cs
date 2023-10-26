using System;
using System.Collections.Generic;
using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;

namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// class detail for public using
    /// </summary>
    public class ClassPublicDetail
    {

        [DisplayName("ID")]
        public long ClassId { get; set; }

        [DisplayName("Class type code")]
        public ClassTypeEnum ClassTypeId { get; set; }

        [DisplayName("Class type")]
        public string ClassType { get; set; }

        [DisplayName("Is distance class")]
        public ClassCategoryEnum ClassCategoryId { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [DisplayName("TimeZone")]
        public string TimeZone { get; set; }

        [DisplayName("Language")]
        public string OriginLanguage { get; set; }

        [DisplayName("Translation")]
        public string TransLanguage { get; set; }

        [DisplayName("WWA allowed")]
        public bool IsWwaFormAllowed { get; set; }

        [DisplayName("Start of registration")]
        [LocalisedText("Metadata", "RegistrationStart")]
        public DateTime RegistrationStart { get; set; }

        [DisplayName("End of registration")]
        [LocalisedText("Metadata", "RegistrationEnd")]
        public DateTime RegistrationEnd { get; set; }

        [DisplayName("Start of class")]
        public DateTime EventStart { get; set; }

        [DisplayName("End of class")]
        public DateTime EventEnd { get; set; }

        [DisplayName("Environment")]
        public EnvironmentTypeEnum EnvironmentTypeId { get; set; }

        [DisplayName("Instructors")]
        [LocalisedText("Metadata", "Instructors")]
        public List<string> Instructors { get; set; }

        [DisplayName("Year")] public int Year => EventStart.Year;

        [DisplayName("Class")]
        [LocalisedText("Metadata", "Class")]
        public string Title { get; set; }
        
        public bool MarkedAsWwa { get; set; }

        // constructor
        public ClassPublicDetail()
        {
            Instructors = new List<string>();
        }

        public ClassPublicDetail Clone()
        {
            return (ClassPublicDetail)MemberwiseClone();
        }
    }
}