using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using CorabeuControl.Components;
using System.Collections.Generic;

namespace AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base
{
    public class DistanceClassTemplateForEdit
    {
        public DistanceClassTemplateForm Form { get; set; } = new DistanceClassTemplateForm();

        public List<DropDownItem> Translations { get; set; } = new List<DropDownItem>();

        public List<IEnumItem> ClassTypes { get; set; } = new List<IEnumItem>();

        public IPersonHelper Persons { get; set; } = new EmptyPersonHelper();
    }
}
