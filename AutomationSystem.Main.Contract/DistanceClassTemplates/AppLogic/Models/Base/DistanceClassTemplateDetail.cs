using AutomationSystem.Base.Contract.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base
{
    public class DistanceClassTemplateDetail
    {
        [DisplayName("ID")]
        public long DistanceClassTemplateId { get; set; }

        [DisplayName("Template state")]
        public DistanceClassTemplateState TemplateState { get; set; }

        [DisplayName("Type of template code")]
        public ClassTypeEnum ClassTypeId { get; set; }

        [DisplayName("Type of template")]
        public string ClassType { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [DisplayName("Start of class")]
        public DateTime EventStart { get; set; }

        [DisplayName("End of class")]
        public DateTime EventEnd { get; set; }

        [DisplayName("Translation")]
        public string TransLanguage { get; set; }

        [DisplayName("Language")]
        public string OriginLanguage { get; set; }
        [DisplayName("Start of registration")]
        public DateTime RegistrationStart { get; set; }

        [DisplayName("End of registration")]
        public DateTime RegistrationEnd { get; set; }

        [DisplayName("Guest instructor")]
        public string GuestInstructor { get; set; }

        [DisplayName("Instructors")]
        public List<string> Instructors { get; set; } = new List<string>();

        [DisplayName("Title")]
        public string Title { get; set; }

        public bool CanApprove { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

    }
}
