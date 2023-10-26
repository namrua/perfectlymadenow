using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data.Models;
using AutomationSystem.Main.Model;
using System;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.Data
{
    public interface IDistanceClassTemplateDatabaseLayer
    {
        List<DistanceClassTemplate> GetDistanceClassTemplatesByFilter(DistanceClassTemplateFilter filter, DistanceClassTemplateIncludes includes = DistanceClassTemplateIncludes.None);

        DistanceClassTemplate GetDistanceClassTemplateById(long id, DistanceClassTemplateIncludes includes = DistanceClassTemplateIncludes.None);

        bool PersonOnAnyDistanceClassTemplate(long personId);

        long InsertDistanceClassTemplate(DistanceClassTemplate template);

        void UpdateDistanceClassTemplate(DistanceClassTemplate template);

        void ApproveDistanceClassTemplate(long id);

        void CompleteDistanceClassTemplate(long id);

        void DeleteDistanceClassTemplate(long id);

        void UpdateDistanceClassTemplateCompletionSettings(long id, DateTime? automationTimeCompletion);
    }
}
