using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;

namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// registration type list item
    /// </summary>
    public class FormerStudentForReviewListItem
    {

        [DisplayName("ID")]
        public long FormerStudentId { get; set; }

        [EmailAddress]
        [DisplayName("Email")]
        [LocalisedText("Metadata", "Email")]
        public string Email { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Address")]
        [LocalisedText("Metadata", "Address")]
        public AddressDetail Address { get; set; }


        [DisplayName("Class ID")]
        public long FormerClassId { get; set; }       

        [DisplayName("Type")]
        [LocalisedText("Metadata", "ClassType")]
        public string ClassType { get; set; }

        [DisplayName("Location")]
        [LocalisedText("Metadata", "Location")]
        public string Location { get; set; }

        [DisplayName("Start of class")]
        public DateTime EventStart { get; set; }

        [DisplayName("End of class")]
        public DateTime EventEnd { get; set; }

        // helpers
        [DisplayName("Year")] public int Year => EventStart.Year;

        [DisplayName("Date")]
        [LocalisedText("Metadata", "Date")]
        public string FullClassDate { get; set; }

        [DisplayName("Class")]
        [LocalisedText("Metadata", "Class")]
        public string ClassTitle { get; set; }


        // constructor
        public FormerStudentForReviewListItem()
        {
            Address = new AddressDetail();
        }
    }
}