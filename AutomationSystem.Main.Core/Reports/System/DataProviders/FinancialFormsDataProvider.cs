using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Persons.AppLogic;
using AutomationSystem.Main.Core.Reports.System.Models.FinancialBusiness;
using AutomationSystem.Main.Core.Reports.System.Models.FinancialForms;
using AutomationSystem.Main.Core.Reports.System.Models.ReportService;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.Helpers.Structures;

namespace AutomationSystem.Main.Core.Reports.System.DataProviders
{
    /// <summary>
    /// Financial forms data provider
    /// </summary>
    public class FinancialFormsDataProvider : IFinancialFormsDataProvider
    {
        private readonly IClassDataProvider data;
        private readonly IClassFinancialBusinessLogic logic;

        public FinancialFormsDataProvider(IClassDataProvider classDataProvider, IClassFinancialBusinessLogic financialBusinessLogic)
        {
            data = classDataProvider;
            logic = financialBusinessLogic;
        }

        public FoiRoyaltyForm GetFoiRoyaltyFormModel()
        {
            var result = new FoiRoyaltyForm
            {
                GeneralInfo = GetGeneralInfo(),
                ReimbursementForPrinting = new ReimbursementForPrinting
                {
                    Rate = data.Business.PrintReimbursement ?? 0.0m,
                    Number = logic.ReimbursementForPrintingQuantity
                },
                RoyaltyFeeItems = GetRoyaltyFeeItems()
            };            
            return result;
        }

        public FaClosingStatement GetFaClosingStatementModel()
        {
            var result = new FaClosingStatement
            {
                GeneralInfo = GetGeneralInfo(),
                CrfDetail = GetFaCrfDetail(),
                ProgramExpenses = GetFaProgramExpanses(),
                ProgramRevenueItems = GetProgramRevenueItems()
                
            };
            return result;
        }
        
        public GuestInstructorStatement GetGuestInstructorStatementModel()
        {
            var result = new GuestInstructorStatement
            {
                GeneralInfo = GetGeneralInfo(),
                USCurrencyInfo = GetUSCurrencyInfo()
            };

            return result;
        }

        #region private methods FOI Royalty Form
        
        private List<RoyaltyFeeItems> GetRoyaltyFeeItems()
        {
            var orderedRateTypes = new[]
            {
                RoyaltyFeeRateTypeEnum.NewStudent,
                RoyaltyFeeRateTypeEnum.NewChild,
                RoyaltyFeeRateTypeEnum.ReviewStudent,
                RoyaltyFeeRateTypeEnum.ReviewChild
            };
            var result = orderedRateTypes.Select(GetRoyaltyFeeItemByRoyaltyFeeRateTypeId).ToList();
            return result;
        }

        private RoyaltyFeeItems GetRoyaltyFeeItemByRoyaltyFeeRateTypeId(RoyaltyFeeRateTypeEnum royaltyFeeRateTypeId)
        {
            var royaltyFee = logic.RoyaltyFees.GetItem(royaltyFeeRateTypeId);
            var item = new RoyaltyFeeItems
            {
                Rate = royaltyFee.Rate,
                Number = royaltyFee.Quantity
            };
            return item;
        }

        #endregion

        #region private methods Guest Instructor Statement

        private UsCurrencyInfo GetUSCurrencyInfo()
        {           
            // computes result
            var result = new UsCurrencyInfo
            {
                ClassRevenue = logic.ClassRevenue,
                PaidStudentsCount = logic.TotalPaidStudentsForClassAndWwa,
                GuestInstructorFee = logic.GuestInstructorFee
            };            
            return result;
        }

        #endregion

        #region private methods FA Closing Statement

        private FaCrfDetail GetFaCrfDetail()
        {
            var result = new FaCrfDetail
            {
                PaidStudentsCount = logic.TotalPaidStudentsForClassAndWwa,
                ProgramRevenue = logic.IsGuestInstructorRevenueAvailable ? (decimal?)null : 0.0m
            };
            return result;
        }

        private List<FaProgramExpenseItem> GetFaProgramExpanses()
        {
            // set mapper to provide correct default values
            var mapper = new IdMapper<int, ProgramExpenseItem>(logic.ProgramExpenses, x => x.Index, 
                y => new ProgramExpenseItem { Index = y, Text = FinancialFormConstants.OriginalExpenses[y - 1], Value = 0.0m});

            // converts ProgramExpenseItem to FaProgramExpenseItem
            var result = new List<FaProgramExpenseItem>();
            for(var i = 1; i <= 20; i++)
            {
                var expense = mapper.TryGetItem(i);
                var faExpense = new FaProgramExpenseItem
                {
                    Text = $"{expense.Index}. {expense.Text}",
                    Value = expense.Value
                };
                result.Add(faExpense);
            }
            return result;
        }

        private List<FaProgramRevenueItem> GetProgramRevenueItems()
        {
            var orderedRateTypes = new[]
            {
                ProgramRevenueType.NewAdultPreRegistration,
                ProgramRevenueType.NewAdultWeekOfClass,
                ProgramRevenueType.NewChild,
                ProgramRevenueType.ReviewedAdult,
                ProgramRevenueType.ReviewedChild,
                ProgramRevenueType.WorldWideAbsentee,
                ProgramRevenueType.ApprovedGuestAndStaffs,
                ProgramRevenueType.Lecture
            };
            var result = orderedRateTypes.Select(GetProgramRevenueItemByProgramRevenueType).ToList();
            return result;
        }

        private FaProgramRevenueItem GetProgramRevenueItemByProgramRevenueType(ProgramRevenueType type)
        {
            var programRevenueItem = logic.ProgramRevenues.GetItem(type);
            var item = new FaProgramRevenueItem
            {
                Quantity = programRevenueItem.Quantity,
                Rate = programRevenueItem.Rate
            };
            return item;
        }

        #endregion

        #region common methods

        private GeneralInfo GetGeneralInfo()
        {
            var cls = data.Class;
            var personsMapper = new PersonIdMapper(data.Persons);
            var coordinator = personsMapper.GetPersonById(cls.CoordinatorId);
            var coordAddress = coordinator.Address;

            // assembles list of instructors           
            var result = new GeneralInfo
            {
                EventName = cls.ClassType.Description,
                EventDates = $"{MainTextHelper.GetEventDate(cls.EventStart, cls.EventEnd)}, {cls.EventStart.Year}",
                Coordinator = GetPersonName(coordinator),
                Address = MainTextHelper.GetAddressStreet(coordAddress.Street, coordAddress.Street2),
                Phone = coordinator.Phone,
                Email = coordinator.Email,
                Instructors = GetInstructorsAsText(personsMapper, cls),
                GuestInstructor = personsMapper.GetPersonNameById(cls.GuestInstructorId),
                EventLocation = cls.Location,
                EventSite = "ONLINE",
                ReportDate = MainTextHelper.GetDotDate(DateTime.Now),
            };

            return result;
        }

        public string GetInstructorsAsText(PersonIdMapper personsMapper, Class cls)
        {
            var instructors = GetInstructors(personsMapper, cls);
            var result = String.Join(", ", instructors);
            return result;
        }

        public string GetPersonName(Person person)
        {
            var result = MainTextHelper.GetFullName(person.Address.FirstName, person.Address.LastName);
            return result;
        }

        public List<string> GetInstructors(PersonIdMapper personsMapper, Class cls)
        {
            var instructorComposer = new PersonDistinctComposer<string>(x => personsMapper.GetPersonNameById(x));
            instructorComposer.AddPerson(cls.GuestInstructorId);
            instructorComposer.AddClassPersonsWithRole(cls.ClassPersons, PersonRoleTypeEnum.Instructor);
            var result = instructorComposer.Pop();
            return result;
        }

        #endregion
    }
}
