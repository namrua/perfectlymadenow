using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Actions;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.System.Model;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.System.Convertors
{
    /// <summary>
    /// Class action convertor
    /// </summary>
    public interface IClassActionConvertor
    {

        // converts class action to list item
        ClassActionListItem ConvertToClassActionListItem(ClassAction classAction);

        // gets email type by class action type id
        List<EmailTypeEnum> GetEmailTypeIdsByClassActionTypeId(ClassActionTypeEnum classActionTypeId, Class cls);

        // gets registration restrictions by class aciton type id
        ClassActionRegistrationRestriction GetRegistrationRestrictionByClassActionTypeId(ClassActionTypeEnum classActionTypeId);

        bool IsActionAllowedForClassDomain(ClassActionTypeEnum actionTypeId, ClassDomainInfo classDomain);

        void CheckActionForClassDomain(ClassActionTypeEnum actionTypeId, Class cls);
    }
}
