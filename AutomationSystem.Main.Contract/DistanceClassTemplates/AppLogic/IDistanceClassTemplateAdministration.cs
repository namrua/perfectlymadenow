using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
namespace AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic
{
    public interface IDistanceClassTemplateAdministration
    {
        // gets distance class template page model
        DistanceClassTemplatePageModel GetDistanceClassTemplatePageModel(DistanceClassTemplateFilter filter, bool search);

        // gets distance class template detail by id
        DistanceClassTemplateDetail GetDistanceClassTemplateDetailById(long id);

        // gets new distance class template
        DistanceClassTemplateForEdit GetNewDistanceClassTemplateForEdit();

        // gets distance class template for edit by id
        DistanceClassTemplateForEdit GetDistanceClassTemplateForEditById(long id);

        // gets distance class template for edit by form
        DistanceClassTemplateForEdit GetDistanceClassTemplateForEditByForm(DistanceClassTemplateForm form);

        // validates distance class template form
        DistanceClassTemplateValidationResult ValidateDistanceClassTemplateForm(DistanceClassTemplateForm form);

        // approves distance class template
        void ApproveDistanceClassTemplate(long id);

        // saves distance class template
        long SaveDistanceClassTemplate(DistanceClassTemplateForm form);

        // delete distance class template
        void DeleteDistanceClassTemplate(long id);
    }
}
