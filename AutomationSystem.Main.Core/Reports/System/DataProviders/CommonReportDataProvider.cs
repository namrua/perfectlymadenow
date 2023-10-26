using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Integration.System.Models;
using AutomationSystem.Main.Core.Persons.AppLogic;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Main.Core.Reports.System.Models.CountryReport;
using AutomationSystem.Main.Core.Reports.System.Models.Crf;
using AutomationSystem.Main.Core.Reports.System.Models.WwaCrf;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Localisation.System;
using AutomationSystem.Shared.Contract.Localisation.System.Models;

namespace AutomationSystem.Main.Core.Reports.System.DataProviders
{
    /// <summary>
    /// Report convertor 
    /// </summary>
    public class CommonReportDataProvider : ICommonReportDataProvider
    {
        private const string checkMark = "x";
        private const int crfReportInstructorSpace = 3;

        private readonly ILocalisationService localisationService;
        private readonly IClassDataProvider data;
        private readonly IRegistrationTypeResolver registrationTypeResolver;

        public CommonReportDataProvider(
            ILocalisationService localisationService,
            IClassDataProvider data,
            IRegistrationTypeResolver registrationTypeResolver)
        {
            this.localisationService = localisationService;
            this.data = data;
            this.registrationTypeResolver = registrationTypeResolver;
        }

        public CrfReport GetCrfReportModel()
        {
            var personsMapper = new PersonIdMapper(data.Persons);

            // resolves instructors and approved staff
            var staffComposer = new PersonDistinctComposer<Person>(x => personsMapper.GetPersonById(x));
            staffComposer.AddPerson(data.Class.GuestInstructorId);
            staffComposer.AddClassPersonsWithRole(data.Class.ClassPersons, PersonRoleTypeEnum.Instructor);
            var instructors = staffComposer.Pop();
            staffComposer.AddClassPersonsWithRole(data.Class.ClassPersons, PersonRoleTypeEnum.ApprovedStaff);
            var approvedStaffs = staffComposer.Pop();

            // gets coordinator
            var coordinator = personsMapper.GetPersonById(data.Class.CoordinatorId);

            // resolve locationInfo contact
            var locationInfoContact = data.ReportSetting.LocationInfoId.HasValue
                ? personsMapper.GetPersonById(data.ReportSetting.LocationInfoId.Value) 
                : null;

            // resolve language
            var language = (data.Class.TransLanguageId.HasValue) 
                ? localisationService.GetLocalisedEnumItemSpecified(EnumTypeEnum.Language, (int)data.Class.TransLanguageId.Value, LocalisationInfo.DefaultLanguageCode).Description.ToUpper() 
                : "";

            // creates crf report
            var approvedRegistrations = data.ApprovedRegistrations
                .Where(x => x.RegistrationTypeId != RegistrationTypeEnum.WWA).ToList();
            var result = new CrfReport
            {
                // general 
                TableTitles = new CrfTableTitles { TransLanguage = language },
                EventInfo = GetCrfEventInfo(coordinator, instructors),
                LocationInfo = GetLocationInfo(locationInfoContact),
                EventsLocationInfo = data.EventLocationInfo.Take(3).Select(ConvertToEventLocationInfo).OrderBy(x => x.EventName).ToList(),
                ClassNumbers = GetClassNumbers(instructors.Count + approvedStaffs.Count, approvedRegistrations),
                FinancialTotals = GetFinancialTotals(),

                // people
                ApprovedStaff = approvedStaffs.Select(ConvertPersonToPersonInfo).ToList(),
                Instructors = instructors.Select(ConvertPersonToPersonInfo).ToList(),
                ApprovedGuest = approvedRegistrations.Where(x => x.RegistrationTypeId == RegistrationTypeEnum.ApprovedGuest).Select(ConvertStudentToPersonInfo).ToList(),
                PaidStudents = approvedRegistrations.Where(x => x.RegistrationTypeId != RegistrationTypeEnum.ApprovedGuest).Select(ConvertStudentToPersonInfo).ToList()
            };
            return result;
        }

        public WwaCrfReport GetWwaReportModel()
        {
            var approvedRegistrations = data.ApprovedRegistrations.Where(x => x.RegistrationTypeId == RegistrationTypeEnum.WWA).ToList();
            var result = new WwaCrfReport
            {
                EventInfo = GetWWAEventInfo(approvedRegistrations.Count),
                Students = approvedRegistrations.Select(ConvertToWWAPersonInfo).ToList(),               
            };
            return result;
        }
        
        public CountriesReportModel GetCountriesReport()
        {
            var result = new CountriesReportModel
            {
                ClassTitle = ClassConvertor.GetClassTitle(data.Class),
                Countries = data.ApprovedRegistrations
                    .GroupBy(x => x.StudentAddress.CountryId, y => y.StudentAddress.Country)
                    .Select(x => new CountryReportItem { Country = x.First().Description, Count = x.Count() })
                    .OrderBy(x => x.Country)
                    .ToList()
            };
            return result;
        }

        public string GetRegistrationListText()
        {
            // selects approved non-WWA registrations                      
            var registrationWithoutWwa = data.ApprovedRegistrations.Where(x => x.RegistrationTypeId != RegistrationTypeEnum.WWA).ToList();

            // assembles registration list           
            var result = "There is no approved registration yet.";
            if (registrationWithoutWwa.Any())
            {
                var registrationList = registrationWithoutWwa.Select(x => MainTextHelper.GetFullName(x.StudentAddress.FirstName, x.StudentAddress.LastName));
                result = string.Join("\n", registrationList);
            }
            return result;
        }

        #region private methods WWA

        private WwaCrfEventInfo GetWWAEventInfo(int totalAbsentee)
        {
            var personMapper = new PersonIdMapper(data.Persons);
            var coordinator = personMapper.GetPersonById(data.Class.CoordinatorId);           

            var result = new WwaCrfEventInfo
            {
                EventName = data.Class.ClassType.Description,
                EventDate = $"{MainTextHelper.GetEventDate(data.Class.EventStart, data.Class.EventEnd)}, {data.Class.EventStart.Year}",
                EventLocation = data.Class.Location,
                CoordinatorId = coordinator.CoordinatorNumber?.ToString() ?? "",
                ClassCoordinator = MainTextHelper.GetFullName(coordinator.Address.FirstName, coordinator.Address.LastName),
                Instructors = GetInstructorsAsText(personMapper, data.Class),
                TotalAbsentee = totalAbsentee
            };
            return result;
        }

        private WwaCrfPersonsInfo ConvertToWWAPersonInfo(ClassRegistration person)
        {
            var participant = person.StudentAddress;
            var registrant = person.RegistrantAddress;
            var payment = person.ClassRegistrationPayment;
            var result = new WwaCrfPersonsInfo()
            {
                FirstNameRegistrant = registrant.FirstName,
                LastNameRegistrant = registrant.LastName,
                CompleteAddressRegistrant = MainTextHelper.GetCompleteAddress(registrant),
                Email = person.RegistrantEmail,
                CheckNumber = payment.CheckNumber,
                TransactionNumber = payment.TransactionNumber,
                PayPalFee = payment.PayPalFee,
                TotalPayPal = payment.TotalPayPal,
                NetPayPal = payment.TotalPayPal - payment.PayPalFee,
                TotalCheck = payment.TotalCheck,
                TotalCash = payment.TotalCash,
                TotalRevenue = CommonFinancialBusinessLogic.GetTotalRevenue(payment),
                TotalCreditCard = payment.TotalCreditCard,
                FirstNameParticipant = participant.FirstName,
                LastNameParticipant = AddressConvertor.ToLogicString(participant.LastName),
                CompleteAddressParticipant = MainTextHelper.GetCompleteAddress(participant),
                CountryParticipant = participant.Country.Description
            };
            return result;
        }

        #endregion

        #region private methods CRF

        private CrfEventInfo GetCrfEventInfo(Person coordinator, List<Person> instructors)
        {            
            var cellInstructors = GetInstructorsForCells(instructors);            

            var result = new CrfEventInfo
            {
                EventName = data.Class.ClassType.Description,
                EventDate = $"{MainTextHelper.GetEventDate(data.Class.EventStart, data.Class.EventEnd)}, {data.Class.EventStart.Year}",
                EventLocation = data.Class.Location,
                Coordinators = GetPersonName(coordinator),
                InstructorsFirstCell = cellInstructors[0],
                InstructorsSecondCell = cellInstructors[1],
                InstructorsThirdCell = cellInstructors[2]
            };

            return result;
        }

        private List<string> GetInstructorsForCells(List<Person> instructors)
        {
            var result = new List<string>();
            var insCellCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(instructors.Count) / crfReportInstructorSpace));

            var skippedCells = 0;
            for (var i = 0; i < crfReportInstructorSpace; i++)
            {
                insCellCount = Math.Min(instructors.Count - skippedCells, insCellCount);
                var instructorsInCell = instructors
                    .Skip(skippedCells).Take(insCellCount)
                    .Select(GetPersonName).ToList();
                result.Add(String.Join(", ", instructorsInCell));
                skippedCells += insCellCount;
            }
            return result;
        }

        private CrfLocationInfo GetLocationInfo(Person locationInfoContact)
        {
            var result = new CrfLocationInfo
            {
                VenueName = data.ReportSetting.VenueName,
            };

            if (locationInfoContact == null)
            {
                return result;
            }

            var address = locationInfoContact.Address;

            result.ContactNames = MainTextHelper.GetFullName(locationInfoContact.Address.FirstName, locationInfoContact.Address.LastName);
            result.Address = MainTextHelper.GetCompleteAddress(address, address.Country.Description);
            result.ContactPhone = locationInfoContact.Phone;

            return result;
        }

        private CrfEventLocationInfo ConvertToEventLocationInfo(EventLocationInfo eventLocationInfo)
        {
            var result = new CrfEventLocationInfo
            {
                EventName = eventLocationInfo.EventName,
                EventLocation = eventLocationInfo.EventLocation
            };
            return result;
        }

        private CrfClassNumbers GetClassNumbers(int approvedStaffCount, List<ClassRegistration> approvedRegistrations)
        {            
            var result = new CrfClassNumbers
            {
                NewStudents = GetStudentsCount(approvedRegistrations, x => !registrationTypeResolver.IsReviewedRegistration(x) && x != RegistrationTypeEnum.ApprovedGuest),
                ReviewStudents = GetStudentsCount(approvedRegistrations, registrationTypeResolver.IsReviewedRegistration),
                ApprovedGuests = GetStudentsCount(approvedRegistrations, x => x == RegistrationTypeEnum.ApprovedGuest),
                ApprovedStaff = approvedStaffCount
            };
            result.TotalRoom = result.NewStudents + result.ReviewStudents + result.ApprovedGuests + result.ApprovedStaff;

            return result;

        }

        private CrfFinancialTotals GetFinancialTotals()
        {
            var result = new CrfFinancialTotals
            {
                TotalApproved = data.Business.ApprovedBudget
            };
            return result;
        }

        private CrfPersonInfo ConvertStudentToPersonInfo(ClassRegistration student)
        {
            var address = student.StudentAddress;
            var payment = student.ClassRegistrationPayment;                        

            var result = new CrfPersonInfo()
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                AddressLine1 = address.Street,
                AddressLine2 = address.Street2,
                City = address.City,
                State = address.State,
                Zip = address.ZipCode,
                Country = address.Country.Description,
                Email = student.StudentEmail,
                Phone = student.StudentPhone,
                CheckNumber = payment.CheckNumber,
                TransactionNumber = payment.TransactionNumber,
                PayPalFee = payment.PayPalFee,
                TotalPayPal = payment.TotalPayPal,
                NetPayPal = payment.TotalPayPal - payment.PayPalFee,
                TotalCheck = payment.TotalCheck,
                TotalCash = payment.TotalCash,
                TotalCreditCard = payment.TotalCreditCard,
                TotalRevenue = CommonFinancialBusinessLogic.GetTotalRevenue(payment),
                StatCode = GetStatusCode(student.RegistrationTypeId),
                IsTransLanguage = GetCheckMark(student.LanguageId != LocalisationInfo.DefaultLanguage),
                Absentee = GetCheckMark(payment.IsAbsentee),
                PaidPmi = GetCheckMark(payment.IsPaidPmi)
            };
            return result;
        }

        private CrfPersonInfo ConvertPersonToPersonInfo(Person person)
        {
            var address = person.Address;
            var result = new CrfPersonInfo()
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                AddressLine1 = address.Street,
                AddressLine2 = address.Street2,
                City = address.City,
                State = address.State,
                Zip = address.ZipCode,
                Country = address.Country.Description,
                Phone = person.Phone,
                Email = person.Email,
                StatCode = GetStatusCode()

            };
            return result;
        }

        private int GetStudentsCount(List<ClassRegistration> approvedRegistrations, Func<RegistrationTypeEnum, bool> studentSelector)
        {
            var result = approvedRegistrations.Count(x => studentSelector(x.RegistrationTypeId));
            return result;
        }

        private string GetStatusCode(RegistrationTypeEnum? type = null)
        {
            if (type == null)
                return "AS";
            return registrationTypeResolver.GetRegistrationTypeCode(type.Value);          
        }

        #endregion

        #region general methods       
        
        private List<string> GetInstructors(PersonIdMapper personsMapper, Class cls)
        {
            var instructorComposer = new PersonDistinctComposer<string>(x => personsMapper.GetPersonNameById(x));
            instructorComposer.AddPerson(cls.GuestInstructorId);
            instructorComposer.AddClassPersonsWithRole(cls.ClassPersons, PersonRoleTypeEnum.Instructor);
            var result = instructorComposer.Pop();
            return result;
        }

        private string GetInstructorsAsText(PersonIdMapper personsMapper, Class cls)
        {
            var instructors = GetInstructors(personsMapper, cls);
            var result = String.Join(", ", instructors);
            return result;
        }

        private string GetPersonName(Person person)
        {
            var result = MainTextHelper.GetFullName(person.Address.FirstName, person.Address.LastName);
            return result;
        }             

        #endregion

        #region macros

        private string GetCheckMark(bool condition)
        {
            var result = condition ? checkMark : null;
            return result;
        }

        #endregion
    }
}
