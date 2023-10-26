using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Persons.AppLogic;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Localisation.System;

namespace AutomationSystem.Main.Core.Home.AppLogic.Convertors
{

    /// <summary>
    /// Converts home page entities
    /// </summary>
    public class HomeConvertor : IHomeConvertor
    {

        private readonly ILocalisationService localisationService;
        private readonly IAddressConvertor addressConvertor;

        // constructor
        public HomeConvertor(
            ILocalisationService localisationService,
            IAddressConvertor addressConvertor)
        {
            this.localisationService = localisationService;
            this.addressConvertor = addressConvertor;
        }


        // converts class to class public detail with instructors
        public ClassPublicDetail ConvertToClassPublicDetailWithInstructors(Class cls, PersonHelper personHelper)
        {
            if (cls.ClassPersons == null)
                throw new InvalidOperationException("ClassPersons is not included into Class object.");
            
            var result = ConvertToClassPublicDetail(cls);
            var instructorComposer = new PersonDistinctComposer<string>(x => personHelper.GetPersonNameById(x));
            instructorComposer.AddPerson(cls.GuestInstructorId);
            instructorComposer.AddClassPersonsWithRole(cls.ClassPersons, PersonRoleTypeEnum.Instructor);
            result.Instructors = instructorComposer.Pop();           
            return result;
        }


        // converts class to class public detail 
        public ClassPublicDetail ConvertToClassPublicDetail(Class cls)
        {
            var classType = localisationService.GetLocalisedEnumItem(EnumTypeEnum.MainClassType, (int) cls.ClassTypeId);
            var timeZone = localisationService.GetLocalisedEnumItem(EnumTypeEnum.TimeZone, (int) cls.TimeZoneId);

            var result = new ClassPublicDetail
            {
                ClassId = cls.ClassId,
                ClassTypeId = cls.ClassTypeId,
                ClassType = classType.Description,
                ClassCategoryId = cls.ClassCategoryId,
                Location = cls.Location,
                TimeZone = timeZone.Name,
                IsWwaFormAllowed = cls.IsWwaFormAllowed,
                RegistrationStart = cls.RegistrationStart ?? default(DateTime),
                RegistrationEnd = cls.RegistrationEnd,
                EventStart = cls.EventStart,
                EventEnd = cls.EventEnd,
                EnvironmentTypeId = cls.EnvironmentTypeId,
            };
            result.Title = MainTextHelper.GetEventOneLineHeader(result.EventStart, result.EventEnd, result.Location, result.ClassType, CultureInfo.CurrentCulture);

            // process languages
            result.OriginLanguage = localisationService.GetLocalisedEnumItem(EnumTypeEnum.Language, (int)cls.OriginLanguageId).Description;
            if (cls.TransLanguageId.HasValue)
                result.TransLanguage = localisationService.GetLocalisedEnumItem(EnumTypeEnum.Language, (int) cls.TransLanguageId.Value).Description;

            return result;
        }

        // converts registration type to registration type list item
        public RegistrationTypeListItem ConvertToRegistrationTypeListItem(RegistrationTypeEnum registrationTypeId,
            IEnumerable<PriceListItem> priceListItems)
        {
            var registrationType = localisationService.GetLocalisedEnumItem(EnumTypeEnum.MainRegistrationType, (int)registrationTypeId);
            var priceListItem = priceListItems.FirstOrDefault(x => x.RegistrationTypeId == registrationTypeId);

            var result = new RegistrationTypeListItem
            {
                RegistrationTypeId = registrationTypeId,
                RegistrationType = registrationType.Description,
                Price = priceListItem?.Price
            };
            return result;
        }        


        // gets price of registration
        public decimal GetRegistrationPrice(RegistrationTypeEnum registrationTypeId, IEnumerable<PriceListItem> priceListItems)
        {
            var priceListItem = priceListItems.FirstOrDefault(x => x.RegistrationTypeId == registrationTypeId);
            if (priceListItem == null)
                throw new InvalidOperationException($"Price is not defined for registration type {registrationTypeId} and Price list with id {priceListItems.FirstOrDefault()?.PriceListId}.");
            return priceListItem.Price;
        }


        // convert to FormerStudentForReviewListItem
        public FormerStudentForReviewListItem ConvertToFormerStudentForReviewListItem(FormerStudent formerStudent)
        {
            if (formerStudent.Address == null)
                throw new InvalidOperationException("Address is not included into former student object");
            if (formerStudent.FormerClass == null)
                throw new InvalidOperationException("FormerClass is not included into former student object");

            var result = new FormerStudentForReviewListItem
            {
                FormerStudentId = formerStudent.FormerStudentId,
                Email = formerStudent.Email,
                Phone = formerStudent.Phone,
                Address = addressConvertor.ConvertToAddressDetail(formerStudent.Address),
                ClassType = localisationService.GetLocalisedEnumItem(EnumTypeEnum.MainClassType, (int)formerStudent.FormerClass.ClassTypeId).Description,
                FormerClassId = formerStudent.FormerClass.FormerClassId,                               
                Location = formerStudent.FormerClass.Location,
                EventStart = formerStudent.FormerClass.EventStart,
                EventEnd = formerStudent.FormerClass.EventEnd
            };
            result.FullClassDate = MainTextHelper.GetEventDate(result.EventStart, result.EventEnd, CultureInfo.CurrentCulture);
            result.ClassTitle = MainTextHelper.GetEventOneLineHeader(result.EventStart, result.EventEnd, result.Location, result.ClassType, CultureInfo.CurrentCulture);
            result.Address.Country = localisationService.GetLocalisedEnumItem(EnumTypeEnum.Country, (int) formerStudent.Address.CountryId).Description;            
            return result;
        }


        // converts ClassStyle to RegistrationPageStyle
        public RegistrationPageStyle ConvertToRegistrationPageStyle(ClassStyle classStyle, ClassCategoryEnum classCategoryId, string profileMoniker)
        {
            var result = new RegistrationPageStyle
            {
                HomepageUrl = classStyle.HomepageUrl ?? string.Format(classCategoryId == ClassCategoryEnum.DistanceClass 
                                  ? ProfileConstants.ProfileDistanceHomepage 
                                  : ProfileConstants.ProfileHomepage, profileMoniker),
                ColorSchemeId = classStyle.RegistrationColorSchemeId,
                HeaderPictureId = classStyle.HeaderPictureId
            };
            return result;
        }

    }

}
