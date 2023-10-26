using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract.Classes.AppLogic;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Actions;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using CorabeuControl.Components;
using System;
using System.Linq;

namespace AutomationSystem.Main.Core.Classes.AppLogic
{
    public class ClassActionAdministration : IClassActionAdministration
    {
        private readonly IClassDatabaseLayer classDb;
        private readonly IEmailIntegration emailIntegration;
        private readonly IIdentityResolver identityResolver;
        private readonly IClassActionConvertor classActionConvertor;
        private readonly IClassActionService classActionService;
        private readonly IClassOperationChecker classOperationChecker;
        private readonly IClassTypeResolver classTypeResolver;

        public ClassActionAdministration(
            IClassDatabaseLayer classDb,
            IEmailIntegration emailIntegration,
            IIdentityResolver identityResolver,
            IClassActionConvertor classActionConvertor,
            IClassActionService classActionService,
            IClassOperationChecker classOperationChecker,
            IClassTypeResolver classTypeResolver)
        {
            this.classDb = classDb;
            this.emailIntegration = emailIntegration;
            this.identityResolver = identityResolver;
            this.classActionConvertor = classActionConvertor;
            this.classActionService = classActionService;
            this.classOperationChecker = classOperationChecker;
            this.classTypeResolver = classTypeResolver;
        }

        public ClassActionPageModel GetClassActionPageModel(long classId)
        {           
            // loads class and class action types
            var cls = GetClassById(classId, ClassIncludes.ClassType | ClassIncludes.ClassActionsClassActionType);
            identityResolver.CheckEntitleForClass(cls);
            var classActionTypes = classDb.GetClassActionTypes();

            // assembles page model
            var classDetail = ClassConvertor.ConvertToClassShortDetial(cls);
            var classDomain = classTypeResolver.GetClassDomainInfo(cls);
            var result = new ClassActionPageModel
            {
                Class = classDetail,
                ClassActions = cls.ClassActions.Select(classActionConvertor.ConvertToClassActionListItem).ToList(),
                ClassActionTypes = classActionTypes
                    .Where(x => classOperationChecker.IsOperationAllowed(classOperationChecker.ConvertToClassOperation(x.ClassActionTypeId), classDetail.ClassState))
                    .Where(x => classActionConvertor.IsActionAllowedForClassDomain(x.ClassActionTypeId, classDomain))
                    .Select(x => PickerItem.Item(x.ClassActionTypeId, x.Description)).ToList()
            };
            return result;
        }

        public ClassActionDetail GetClassActionDetail(long classActionId)
        {
            var classAction = GetClassActionById(classActionId, ClassActionIncludes.ClassActionType | ClassActionIncludes.Class);
            identityResolver.CheckEntitleForClass(classAction.Class);

            // create object
            var result = new ClassActionDetail
            {
                ClassState = ClassConvertor.GetClassState(classAction.Class),
                ClassAction = classActionConvertor.ConvertToClassActionListItem(classAction),
                EmailTemplates = emailIntegration.GetEmailTemplateListItemsByEntity(new EmailTemplateEntityId(EntityTypeEnum.MainClassAction, classActionId))
            };
            result.CanProcess = !result.ClassAction.IsProcessed
                && classOperationChecker.IsOperationAllowed(classOperationChecker.ConvertToClassOperation(result.ClassAction.ClassActionTypeId), result.ClassState);
            result.CanDelete = !result.ClassAction.IsProcessed;
            return result;

        }

        public long CreateClassAction(long classId, ClassActionTypeEnum classActionTypeId)
        {
            // gets class and checks operation state
            var cls = GetClassById(classId);
            identityResolver.CheckEntitleForClass(cls);
            classOperationChecker.CheckOperation(classOperationChecker.ConvertToClassOperation(classActionTypeId), cls);
            classActionConvertor.CheckActionForClassDomain(classActionTypeId, cls);

            var classActionId = classActionService.CreateClassAction(cls, classActionTypeId);
            return classActionId;
        }

        public void ProcessClassAction(long classActionId)
        {
            // loads class action and checks operation
            var classAction = GetClassActionById(classActionId, ClassActionIncludes.ClassClassStyle);
            identityResolver.CheckEntitleForClass(classAction.Class);

            var actionTypeId = classAction.ClassActionTypeId;
            classOperationChecker.CheckOperation(classOperationChecker.ConvertToClassOperation(actionTypeId), classAction.Class);
            classActionConvertor.CheckActionForClassDomain(actionTypeId, classAction.Class);

            // process action
            classActionService.ProcessClassAction(classAction);
        }

        public long? DeleteClassAction(long classActionId)
        {
            var classAction = GetClassActionById(classActionId, ClassActionIncludes.Class);
            identityResolver.CheckEntitleForClass(classAction.Class);

            emailIntegration.DeleteEmailTemplatesByEntity(new EmailEntityId(EntityTypeEnum.MainClassAction, classActionId));
            var classId = classDb.DeleteClassAction(classActionId, ClassOperationOption.CheckOperation);
            return classId;
        }

        #region private methods

        public Class GetClassById(long classId, ClassIncludes includes = ClassIncludes.None)
        {
            var result = classDb.GetClassById(classId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Class with id {classId}.");
            }

            return result;
        }

        private ClassAction GetClassActionById(long classActionId, ClassActionIncludes includes = ClassActionIncludes.None)
        {
            var classAction = classDb.GetClassActionById(classActionId, includes);
            if (classAction == null)
            {
                throw new ArgumentException($"There is no ClassAction with id {classActionId}.");
            }

            return classAction;
        }

        #endregion
    }
}