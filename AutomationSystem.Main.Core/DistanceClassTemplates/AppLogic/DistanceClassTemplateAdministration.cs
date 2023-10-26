using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.Models;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.Models.Events;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data.Models;
using AutomationSystem.Main.Core.Persons.AppLogic;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using System;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic
{
    public class DistanceClassTemplateAdministration : IDistanceClassTemplateAdministration
    {
        private readonly IDistanceClassTemplateDatabaseLayer distanceTemplateDb;
        private readonly IMainMapper mainMapper;
        private readonly IPersonDatabaseLayer personDb;
        private readonly IDistanceClassTemplateHelper templateHelper;
        private readonly IDistanceClassTemplateFactory factory;
        private readonly IEventDispatcher eventDispatcher;
        private readonly IDistanceClassTemplateOperationChecker templateOperationChecker;

        public DistanceClassTemplateAdministration(
            IDistanceClassTemplateDatabaseLayer distanceTemplateDb,
            IMainMapper mainMapper,
            IPersonDatabaseLayer personDb,
            IDistanceClassTemplateHelper templateHelper,
            IDistanceClassTemplateFactory factory,
            IEventDispatcher eventDispatcher,
            IDistanceClassTemplateOperationChecker templateOperationChecker)
        {
            this.distanceTemplateDb = distanceTemplateDb;
            this.mainMapper = mainMapper;
            this.personDb = personDb;
            this.templateHelper = templateHelper;
            this.factory = factory;
            this.eventDispatcher = eventDispatcher;
            this.templateOperationChecker = templateOperationChecker;
        }

        // gets distance class template page model
        public DistanceClassTemplatePageModel GetDistanceClassTemplatePageModel(DistanceClassTemplateFilter filter, bool search)
        {
            var model = new DistanceClassTemplatePageModel(filter);
            model.WasSearched = search;
            
            // sets filter
            model.TemplateStates = new List<DistanceClassTemplateState>
            {
                DistanceClassTemplateState.New,
                DistanceClassTemplateState.Approved,
                DistanceClassTemplateState.Completed
            };

            if (search)
            {
                var templates = distanceTemplateDb.GetDistanceClassTemplatesByFilter(
                    filter,
                    DistanceClassTemplateIncludes.ClassType
                    | DistanceClassTemplateIncludes.DistanceClassTemplatePersons);

                model.Items = MapToDistanceClassTemplateListItems(templates);
            }

            return model;
        }

        // gets distance class template detail by id
        public DistanceClassTemplateDetail GetDistanceClassTemplateDetailById(long id)
        {
            var template = GetDistanceClassTemplate(id, DistanceClassTemplateIncludes.ClassType | DistanceClassTemplateIncludes.DistanceClassTemplatePersons);
            var result = mainMapper.Map<DistanceClassTemplateDetail>(template);

            var personIds = templateHelper.GetDistanceClassTemplatePersonIds(template);
            var personHelper = new PersonHelper(personDb.GetMinimizedPersonsByIds(personIds));
            result.GuestInstructor = personHelper.GetPersonNameById(template.GuestInstructorId);
            result.Instructors = templateHelper.GetDistanceClassTemplateInstructors(template, personHelper);
            SetOperationsForDistanceClassTemplate(result);
            return result;
        }

        // gets new distance class template for edit
        public DistanceClassTemplateForEdit GetNewDistanceClassTemplateForEdit()
        {
            var foredit = factory.CreateDistanceClassTemplateForEdit();
            foredit.Form.RegistrationStart = DateTime.Today;
            return foredit;
        }

        // gets distance class template for edit by id
        public DistanceClassTemplateForEdit GetDistanceClassTemplateForEditById(long id)
        {
            var template = GetDistanceClassTemplate(id, DistanceClassTemplateIncludes.DistanceClassTemplatePersons);
            templateOperationChecker.CheckOperation(DistanceClassTemplateOperation.Edit, templateHelper.GetDistanceClassTemplateState(template));

            var forEdit = factory.CreateDistanceClassTemplateForEdit();
            forEdit.Form = mainMapper.Map<DistanceClassTemplateForm>(template);
            return forEdit;
        }

        // gets distance class template for edit by form
        public DistanceClassTemplateForEdit GetDistanceClassTemplateForEditByForm(DistanceClassTemplateForm form)
        {
            var forEdit = factory.CreateDistanceClassTemplateForEdit();
            forEdit.Form = form;

            return forEdit;
        }

        // validates distance class template form
        public DistanceClassTemplateValidationResult ValidateDistanceClassTemplateForm(DistanceClassTemplateForm form)
        {
            var result = new DistanceClassTemplateValidationResult();
            if (form.RegistrationStart.HasValue && form.RegistrationEnd.HasValue && form.RegistrationStart > form.RegistrationEnd)
            {
                return result;
            }

            if (form.RegistrationEnd.HasValue && form.EventStart.HasValue && form.RegistrationEnd > form.EventStart)
            {
                return result;
            }

            if (form.EventStart.HasValue && form.EventEnd.HasValue && form.EventStart > form.EventEnd)
            {
                return result;
            }

            result.IsValid = true;
            return result;
        }

        // approves distance class template
        public void ApproveDistanceClassTemplate(long id)
        {
            var template = GetDistanceClassTemplate(id);
            templateOperationChecker.CheckOperation(DistanceClassTemplateOperation.Approve, templateHelper.GetDistanceClassTemplateState(template));

            eventDispatcher.Dispatch(new DistanceClassTemplateApprovedEvent(id));
            distanceTemplateDb.ApproveDistanceClassTemplate(id);
        }

        // saves DistanceClassTemplate
        public long SaveDistanceClassTemplate(DistanceClassTemplateForm form)
        {
            var template = mainMapper.Map<DistanceClassTemplate>(form);
            var templateId = template.DistanceClassTemplateId;

            if (templateId == 0)
            {
                templateId = distanceTemplateDb.InsertDistanceClassTemplate(template);
            }
            else
            {
                var templateToCheck = GetDistanceClassTemplate(templateId);
                templateOperationChecker.CheckOperation(DistanceClassTemplateOperation.Edit, templateHelper.GetDistanceClassTemplateState(templateToCheck));

                distanceTemplateDb.UpdateDistanceClassTemplate(template);
                eventDispatcher.Dispatch(new DistanceClassTemplateChangedEvent(templateId));
            }

            return templateId;
        }

        // delete distance class template
        public void DeleteDistanceClassTemplate(long id)
        {
            var template = GetDistanceClassTemplate(id);
            templateOperationChecker.CheckOperation(DistanceClassTemplateOperation.Delete, templateHelper.GetDistanceClassTemplateState(template));

            distanceTemplateDb.DeleteDistanceClassTemplate(id);
        }

        #region private methods
        private List<DistanceClassTemplateListItem> MapToDistanceClassTemplateListItems(List<DistanceClassTemplate> templates)
        {
            var personIds = templateHelper.GetDistanceClassTemplatePersonsIds(templates);
            var personHelper = new PersonHelper(personDb.GetMinimizedPersonsByIds(personIds));
            var result = new List<DistanceClassTemplateListItem>();
            foreach (var template in templates)
            {
                var item = mainMapper.Map<DistanceClassTemplateListItem>(template);
                item.Instructors = templateHelper.GetDistanceClassTemplateInstructorsWithGuestInstructor(template, personHelper);
                result.Add(item);
            }
            
            return result;
        }

        private void SetOperationsForDistanceClassTemplate(DistanceClassTemplateDetail detail)
        {
            detail.CanApprove = templateOperationChecker.IsOperationAllowed(DistanceClassTemplateOperation.Approve, detail.TemplateState);
            detail.CanEdit = templateOperationChecker.IsOperationAllowed(DistanceClassTemplateOperation.Edit, detail.TemplateState);
            detail.CanDelete = templateOperationChecker.IsOperationAllowed(DistanceClassTemplateOperation.Delete, detail.TemplateState);
        }

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
