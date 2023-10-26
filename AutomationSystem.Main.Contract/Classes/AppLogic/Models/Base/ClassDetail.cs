using System;
using System.Collections.Generic;
using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base
{
    /// <summary>
    /// Class detail
    /// </summary>
    public class ClassDetail
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

        [DisplayName("Coordinator")]
        public string Coordinator { get; set; }

        [DisplayName("Guest instructor")]
        public string GuestInstructor { get; set; }

        [DisplayName("Instructors")]
        public List<string> Instructors { get; set; }

        [DisplayName("Approved staffs")]
        public List<string> ApprovedStaffs { get; set; }

        [DisplayName("WWA allowed")]
        public bool IsWwaFormAllowed { get; set; }

        [DisplayName("Price list")]
        public string PriceList { get; set; }

        [DisplayName("Currency")]
        public string Currency { get; set; }

        [DisplayName("PayPal key")]
        public string PayPalKey { get; set; }

        [DisplayName("Integration type code")]
        public IntegrationTypeEnum IntegrationTypeId { get; set; }     
        
        [DisplayName("Integration type")]
        public string IntegrationType { get; set; }

        [DisplayName("Integration ID")]
        public long? IntegrationEntityId { get; set; }

        [DisplayName("Integrated with")]
        public string IntegrationEntityName { get; set; }                       
    
        [DisplayName("Approved registrations")]
        public int ApprovedRegistrations { get; set; }

        [DisplayName("Environment")]
        public EnvironmentTypeEnum EnvironmentTypeId { get; set; }

        [DisplayName("Profile ID")]
        public long ProfileId { get; set; }

        [DisplayName("Profile")]
        public string Profile { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }

        public bool ShowLink { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public ClassFormConfiguration FormConfiguration { get; set; }

        // constructor
        public ClassDetail()
        {
            Instructors = new List<string>();
            ApprovedStaffs = new List<string>();
            FormConfiguration = new ClassFormConfiguration();
        }
    }
}