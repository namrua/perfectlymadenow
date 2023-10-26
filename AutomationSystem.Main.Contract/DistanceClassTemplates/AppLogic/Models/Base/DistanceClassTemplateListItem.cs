using AutomationSystem.Base.Contract.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base
{
    public class DistanceClassTemplateListItem
    {
        [DisplayName("ID")]
        public long DistanceClassTemplateId { get; set; }

        [DisplayName("Template state")]
        public DistanceClassTemplateState TemplateState { get; set; }

        [DisplayName("Type of distance class code")]
        public ClassTypeEnum ClassTypeId { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [DisplayName("Start of distance class")]
        public DateTime EventStart { get; set; }

        [DisplayName("End of distance class")]
        public DateTime EventEnd { get; set; }
        
        [DisplayName("Translation")]
        public string TransLanguage { get; set; }

        [DisplayName("Language")]
        public string OriginLanguage { get; set; }

        [DisplayName("Start of registration")]
        public DateTime RegistrationStart { get; set; }

        [DisplayName("End of registration")]
        public DateTime RegistrationEnd { get; set; }

        [DisplayName("Instructors")]
        public List<string> Instructors { get; set; } = new List<string>();

        [DisplayName("Title")]
        public string Title { get; set; }
    }
}
