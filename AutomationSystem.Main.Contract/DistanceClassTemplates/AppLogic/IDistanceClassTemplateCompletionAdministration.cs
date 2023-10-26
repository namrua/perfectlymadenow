using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Completion;

namespace AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic
{
    public interface IDistanceClassTemplateCompletionAdministration
    {
        DistanceClassTemplateCompletionPageModel GetDistanceClassTemplateCompletionPageModel(long distanceTemplateId);

        DistanceClassTemplateCompletionForm GetDistanceClassTemplateCompletionFormById(long distanceTemplateId);

        void SaveDistanceClassTemplateCompletionSettings(DistanceClassTemplateCompletionForm form);

        long CompleteDistanceClassTemplate(long distanceTemplateId);
    }
}
