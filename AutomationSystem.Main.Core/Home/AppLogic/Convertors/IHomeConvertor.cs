using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Core.Persons.AppLogic;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Home.AppLogic.Convertors
{
    /// <summary>
    /// Converts home page entities
    /// </summary>
    public interface IHomeConvertor
    {
        // converts class to class public detail with instructors
        ClassPublicDetail ConvertToClassPublicDetailWithInstructors(Class cls, PersonHelper personHelper);

        // converts class to class public detail 
        ClassPublicDetail ConvertToClassPublicDetail(Class cls);

        // converts registration type to registration type list item
        RegistrationTypeListItem ConvertToRegistrationTypeListItem(RegistrationTypeEnum registrationTypeId, IEnumerable<PriceListItem> priceListItems);      
        
        // gets price of registration
        decimal GetRegistrationPrice(RegistrationTypeEnum registrationTypeId, IEnumerable<PriceListItem> priceListItems);

        // convert to FormerStudentForReviewListItem
        FormerStudentForReviewListItem ConvertToFormerStudentForReviewListItem(FormerStudent formerStudent);

        // converts ClassStyle to RegistrationPageStyle
        RegistrationPageStyle ConvertToRegistrationPageStyle(ClassStyle classStyle, ClassCategoryEnum classCategoryId, string profileMoniker);
    }
}