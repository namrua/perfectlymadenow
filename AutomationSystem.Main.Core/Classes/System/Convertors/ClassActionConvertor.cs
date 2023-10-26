using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Actions;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.System.Model;
using AutomationSystem.Main.Core.Emails.System;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.System.Convertors
{
    /// <summary>
    /// Class action convertor
    /// </summary>
    public class ClassActionConvertor : IClassActionConvertor
    {
        private readonly IEmailTypeResolver emailTypeResolver;
        private readonly IClassTypeResolver classTypeResolver;

        public ClassActionConvertor(IEmailTypeResolver emailTypeResolver, IClassTypeResolver classTypeResolver)
        {
            this.emailTypeResolver = emailTypeResolver;
            this.classTypeResolver = classTypeResolver;
        }

        // converts class action to list item
        public ClassActionListItem ConvertToClassActionListItem(ClassAction classAction)
        {
            if (classAction.ClassActionType == null)
                throw new InvalidOperationException("ClassActionType is not included into ClassAction object.");

            var result = new ClassActionListItem
            {
                ClassActionId = classAction.ClassActionId,
                ClassId = classAction.ClassId,
                ClassActionTypeId = classAction.ClassActionTypeId,
                ClassActionType = classAction.ClassActionType.Description,
                IsProcessed = classAction.IsProcessed,
                Processed = classAction.Processed
            };
            return result;
        }


        // gets email types by class action type id and class type
        public List<EmailTypeEnum> GetEmailTypeIdsByClassActionTypeId(ClassActionTypeEnum classActionTypeId, Class cls)
        {
            var result = new List<EmailTypeEnum>();
            var originEmailTypeId = GetEmailTypeIdByConversationActionTypeId(classActionTypeId);

            // change notification actions define WWA or coordinated itself
            if (classActionTypeId == ClassActionTypeEnum.Change || classActionTypeId == ClassActionTypeEnum.WwaChange)
            {
                result.Add(originEmailTypeId);
                return result;
            }

            // the rest of actions produces email types by its domain
            var classDomain = classTypeResolver.GetClassDomainInfo(cls);
            var wwaEmailTypeId = emailTypeResolver.GetWwaEmailTypeFrom(originEmailTypeId);
            if (classDomain.IncludesCoordinated)
                result.Add(originEmailTypeId);
            if (classDomain.IncludesWwa)
                result.Add(wwaEmailTypeId);

            return result;
        }


        // gets registration restrictions by class aciton type id
        public ClassActionRegistrationRestriction GetRegistrationRestrictionByClassActionTypeId(ClassActionTypeEnum classActionTypeId)
        {
            switch (classActionTypeId)
            {
                case ClassActionTypeEnum.Completion:
                case ClassActionTypeEnum.Cancelation:
                    return ClassActionRegistrationRestriction.None;
                    
                case ClassActionTypeEnum.Change:
                    return ClassActionRegistrationRestriction.OnlyCoordinated;
                   
                case ClassActionTypeEnum.WwaChange:
                    return ClassActionRegistrationRestriction.OnlyWwa;

                default:
                    throw new ArgumentOutOfRangeException(nameof(classActionTypeId), classActionTypeId, "Unknown ClassActionTypeId");
            }
        }

        // determines whether is class action allowed for class domain
        public bool IsActionAllowedForClassDomain(ClassActionTypeEnum actionTypeId, ClassDomainInfo classDomain)
        {
            switch (actionTypeId)
            {
                case ClassActionTypeEnum.Change when !classDomain.IncludesCoordinated:
                case ClassActionTypeEnum.WwaChange when !classDomain.IncludesWwa:
                    return false;
                default:
                    return true;
            }
        }

        // checks action for class domain
        public void CheckActionForClassDomain(ClassActionTypeEnum actionTypeId, Class cls)
        {
            var classDomain = classTypeResolver.GetClassDomainInfo(cls);
            if (!IsActionAllowedForClassDomain(actionTypeId, classDomain))
                throw new InvalidOperationException($"Class action {actionTypeId} is not allowed for Class with id {cls.ClassId}.");
        }

        #region private methods

        // gets email type by conversation action type id
        private EmailTypeEnum GetEmailTypeIdByConversationActionTypeId(ClassActionTypeEnum classActionTypeId)
        {
            switch (classActionTypeId)
            {
                case ClassActionTypeEnum.Completion:
                    return EmailTypeEnum.ConversationCompleted;
                case ClassActionTypeEnum.Cancelation:
                    return EmailTypeEnum.ConversationCanceled;
                case ClassActionTypeEnum.Change:
                    return EmailTypeEnum.ConversationChanged;
                case ClassActionTypeEnum.WwaChange:
                    return EmailTypeEnum.WwaConversationChanged;
                default:
                    throw new ArgumentOutOfRangeException(nameof(classActionTypeId), classActionTypeId, "Unknown ClassActionTypeId");
            }
        }

        #endregion

    }

}
