using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Completion;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.Models;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data.Models;
using AutomationSystem.Main.Core.MainAsyncRequestManagers.System;
using AutomationSystem.Main.Model;
using System;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic
{
    public class DistanceClassTemplateCompletionAdministration : IDistanceClassTemplateCompletionAdministration
    {
        private readonly IDistanceClassTemplateDatabaseLayer distanceTemplateDb;
        private readonly IMainMapper mainMapper;
        private readonly IMainAsyncRequestManager mainAsyncRequestManager;
        private readonly IDistanceClassTemplateHelper distanceTemplateHelper;
        private readonly IDistanceClassTemplateOperationChecker templateOperationChecker;

        public DistanceClassTemplateCompletionAdministration(
            IDistanceClassTemplateDatabaseLayer distanceTemplateDb,
            IMainMapper mainMapper,
            IMainAsyncRequestManager mainAsyncRequestManager,
            IDistanceClassTemplateHelper distanceTemplateHelper,
            IDistanceClassTemplateOperationChecker templateOperationChecker)
        {
            this.distanceTemplateDb = distanceTemplateDb;
            this.mainMapper = mainMapper;
            this.mainAsyncRequestManager = mainAsyncRequestManager;
            this.distanceTemplateHelper = distanceTemplateHelper;
            this.templateOperationChecker = templateOperationChecker;
        }

        public DistanceClassTemplateCompletionPageModel GetDistanceClassTemplateCompletionPageModel(long distanceTemplateId)
        {
            var template = GetDistanceClassTemplate(distanceTemplateId, DistanceClassTemplateIncludes.ClassType);
            var model = new DistanceClassTemplateCompletionPageModel
            {
                ShortDetail = mainMapper.Map<DistanceClassTemplateCompletionShortDetail>(template),
                GeneratingRequests = mainAsyncRequestManager.GetLastRequestsByEntityAndTypes(EntityTypeEnum.MainDistanceClassTemplate, distanceTemplateId, AsyncRequestTypeEnum.CompleteDistanceClassesForTemplate),
                AutomationCompleteTime = template.AutomationCompleteTime,
                Completed = template.Completed
            };
            model.CanComplete = templateOperationChecker.IsOperationAllowed(DistanceClassTemplateOperation.Complete, model.ShortDetail.TemplateState);
            
            return model;
        }

        public DistanceClassTemplateCompletionForm GetDistanceClassTemplateCompletionFormById(long distanceTemplateId)
        {
            var template = GetDistanceClassTemplate(distanceTemplateId);
            var form = mainMapper.Map<DistanceClassTemplateCompletionForm>(template);
            
            return form;
        }

        public void SaveDistanceClassTemplateCompletionSettings(DistanceClassTemplateCompletionForm form)
        {
            distanceTemplateDb.UpdateDistanceClassTemplateCompletionSettings(form.DistanceClassTemplateId, form.AutomationCompleteTime);
        }

        public long CompleteDistanceClassTemplate(long distanceTemplateId)
        {
            var template = GetDistanceClassTemplate(distanceTemplateId);
            templateOperationChecker.CheckOperation(DistanceClassTemplateOperation.Complete, distanceTemplateHelper.GetDistanceClassTemplateState(template));

            var result = mainAsyncRequestManager.AddCompletionDistanceClassForTemplateAsyncRequest(distanceTemplateId, (int)SeverityEnum.High);
            return result.AsyncRequestId;
        }

        #region private methods
        private DistanceClassTemplate GetDistanceClassTemplate(long distanceTemplateId, DistanceClassTemplateIncludes includes = DistanceClassTemplateIncludes.None)
        {
            var result = distanceTemplateDb.GetDistanceClassTemplateById(distanceTemplateId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no distance class template with id {distanceTemplateId}.");
            }

            return result;
        }
        #endregion
    }
}
