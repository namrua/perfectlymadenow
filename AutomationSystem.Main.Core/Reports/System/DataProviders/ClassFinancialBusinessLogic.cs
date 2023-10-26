using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Preferences.System;
using AutomationSystem.Main.Core.Reports.System.Models.FinancialBusiness;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.Helpers.Contract.Structures;
using PerfectlyMadeInc.Helpers.Structures;

namespace AutomationSystem.Main.Core.Reports.System.DataProviders
{
    /// <summary>
    /// Defines financial business logic for classes (with class type category = class)
    /// </summary>
    public class ClassFinancialBusinessLogic : IClassFinancialBusinessLogic
    {
        private const int guestInstructorCountTrashold = 26;

        private readonly IClassDataProvider data;
        private readonly IMainPreferenceProvider mainPreferences;

        private readonly Lazy<bool> isGuestInstructorRevenueAvailable;
        private readonly Lazy<int> totalPaidStudentsForClassAndWwa;       
        private readonly Lazy<IIdMapper<RoyaltyFeeRateTypeEnum, RoyaltyFeeItem>> royaltyFees;
        private readonly Lazy<IIdMapper<ProgramRevenueType, ProgramRevenueItem>> programRevenues;
        private readonly Lazy<List<ProgramExpenseItem>> programExpenses;

        // todo: move data to database
        private readonly Dictionary<CurrencyEnum, decimal> guestInstructorFeeThresholds;

        public ClassFinancialBusinessLogic(IClassDataProvider classDataProvider, IMainPreferenceProvider mainPreferences)
        {
            this.mainPreferences = mainPreferences;

            isGuestInstructorRevenueAvailable = new Lazy<bool>(GetIsGuestInstructorRevenueAvailable);
            totalPaidStudentsForClassAndWwa = new Lazy<int>(GetTotalPaidStudentsForClassAndWwa);           
            royaltyFees = new Lazy<IIdMapper<RoyaltyFeeRateTypeEnum, RoyaltyFeeItem>>(GetRoyaltyFees);
            programRevenues = new Lazy<IIdMapper<ProgramRevenueType, ProgramRevenueItem>>(GetProgramRevenues);
            programExpenses = new Lazy<List<ProgramExpenseItem>>(GetProgramExpenses);

            guestInstructorFeeThresholds = new Dictionary<CurrencyEnum, decimal>
            {
                {CurrencyEnum.USD, 7750m },
                {CurrencyEnum.MXN, 98172m },
                {CurrencyEnum.RON, 24235m }
            };

            data = classDataProvider;
        }

        #region public data

        public bool IsGuestInstructorRevenueAvailable => isGuestInstructorRevenueAvailable.Value;

        public int TotalPaidStudentsForClassAndWwa => totalPaidStudentsForClassAndWwa.Value;

        public int ReimbursementForPrintingQuantity => 
            RoyaltyFees.TryGetItem(RoyaltyFeeRateTypeEnum.NewChild).Quantity + RoyaltyFees.TryGetItem(RoyaltyFeeRateTypeEnum.NewStudent).Quantity;

        public decimal ClassRevenue => ProgramRevenues.Items.Sum(x => x.Rate * x.Quantity) - ProgramExpenses.Sum(x => x.Value);

        public decimal GuestInstructorFee => IsGuestInstructorRevenueAvailable ? Math.Min(ClassRevenue * 0.06m, GuestInstructorFeeThresholds) : 0.0m;


        public IIdMapper<RoyaltyFeeRateTypeEnum, RoyaltyFeeItem> RoyaltyFees => royaltyFees.Value;

        public IIdMapper<ProgramRevenueType, ProgramRevenueItem> ProgramRevenues => programRevenues.Value;

        public List<ProgramExpenseItem> ProgramExpenses => programExpenses.Value;

        #endregion

        #region computing methods

        private decimal GuestInstructorFeeThresholds
        {
            get
            {
                if (!guestInstructorFeeThresholds.TryGetValue(data.Class.CurrencyId, out var result))
                {
                    return 0m;
                }

                return result;
            }
        }

        private bool GetIsGuestInstructorRevenueAvailable()
        {            
            // check that guest instructor is set and it is not master lead instructor
            var masterLeadInstructor = mainPreferences.GetMasterLeadInstructorId();
            if (!data.Class.GuestInstructorId.HasValue || data.Class.GuestInstructorId.Value == masterLeadInstructor)
                return false;

            // checks that there is at least 26 paid students            
            if (TotalPaidStudentsForClassAndWwa < guestInstructorCountTrashold)
                return false;

            return true;
        }

        private int GetTotalPaidStudentsForClassAndWwa()
        {
            var result = data.ApprovedRegistrations.Count(x => x.RegistrationTypeId != RegistrationTypeEnum.ApprovedGuest);
            return result;
        }

        private IIdMapper<RoyaltyFeeRateTypeEnum, RoyaltyFeeItem> GetRoyaltyFees()
        {
            var frag = FragmentRegistrationsByType(data.ApprovedRegistrations);
            var ratesMap = new IdMapper<RoyaltyFeeRateTypeEnum, RoyaltyFeeRate>(data.RoyaltyFeeRates, x => x.RoyaltyFeeRateTypeId);

            var ratesToRegistrationTypes = new Dictionary<RoyaltyFeeRateTypeEnum, RegistrationTypeEnum[]>
            {
                {
                    RoyaltyFeeRateTypeEnum.NewStudent,
                    new[] { RegistrationTypeEnum.NewAdult, RegistrationTypeEnum.NewAdultWeekOfClass }
                },
                {
                    RoyaltyFeeRateTypeEnum.NewChild,
                    new[] { RegistrationTypeEnum.NewChild }
                },
                {
                    RoyaltyFeeRateTypeEnum.ReviewStudent,
                    new[] { RegistrationTypeEnum.ReviewAdult }
                },
                {
                    RoyaltyFeeRateTypeEnum.ReviewChild,
                    new[] { RegistrationTypeEnum.ReviewChild }
                }
            };

            var resultList = ratesToRegistrationTypes.Select(x => CreateRoyaltyFeeItem(x.Key, x.Value, frag, ratesMap)).ToList();
            var result = new IdMapper<RoyaltyFeeRateTypeEnum, RoyaltyFeeItem>(resultList, x => x.Type, y => new RoyaltyFeeItem {Type = y});
            return result;
        }

        private RoyaltyFeeItem CreateRoyaltyFeeItem(
            RoyaltyFeeRateTypeEnum type,
            RegistrationTypeEnum[] registrationTypes,
            IIdMapper<RegistrationTypeEnum, List<ClassRegistration>> frag,
            IIdMapper<RoyaltyFeeRateTypeEnum, RoyaltyFeeRate> ratesMap)
        {
            var result = new RoyaltyFeeItem
            {
                Type = type,
                Quantity = GetQuantityByRegistrationType(frag, registrationTypes),
                Rate = ratesMap.TryGetItem(type)?.Rate ?? 0.0m
            };
            return result;
        }
        
        private IIdMapper<ProgramRevenueType, ProgramRevenueItem> GetProgramRevenues()
        {
            var frag = FragmentRegistrationsByType(data.ApprovedRegistrations);
            var priceListItems = GetPriceListItems();
            var priceListItemMap = new IdMapper<RegistrationTypeEnum, PriceListItem>(priceListItems, x => x.RegistrationTypeId);

            var ratesToRegistrationTypes = new Dictionary<ProgramRevenueType, RegistrationTypeEnum>
            {
                { ProgramRevenueType.NewAdultPreRegistration, RegistrationTypeEnum.NewAdult },
                { ProgramRevenueType.NewAdultWeekOfClass, RegistrationTypeEnum.NewAdultWeekOfClass },
                { ProgramRevenueType.NewChild, RegistrationTypeEnum.NewChild },
                { ProgramRevenueType.ReviewedAdult, RegistrationTypeEnum.ReviewAdult },
                { ProgramRevenueType.ReviewedChild, RegistrationTypeEnum.ReviewChild },
                { ProgramRevenueType.WorldWideAbsentee, RegistrationTypeEnum.WWA }
            };

            var resultList = ratesToRegistrationTypes.Select(x => CreateProgramRevenueItem(x.Key, x.Value, frag, priceListItemMap)).ToList();
            resultList.Add(new ProgramRevenueItem
            {
                Type = ProgramRevenueType.ApprovedGuestAndStaffs,
                Quantity = GetQuantityByRegistrationType(frag, RegistrationTypeEnum.ApprovedGuest) + GetInstructorAndStaffCount(),
                Rate = 0.0m,
            });
            resultList.Add(new ProgramRevenueItem
            {
                Type = ProgramRevenueType.Lecture,
                Quantity = data.ApprovedRegistrationsOfAssociatedLecture.Count(x => x.RegistrationTypeId == RegistrationTypeEnum.LectureRegistration),
                Rate = priceListItemMap.TryGetItem(RegistrationTypeEnum.LectureRegistration)?.Price ?? 0.0m
            });
            var result = new IdMapper<ProgramRevenueType, ProgramRevenueItem>(resultList, x => x.Type, y => new ProgramRevenueItem { Type = y });
            return result;
        }
        
        private ProgramRevenueItem CreateProgramRevenueItem(
            ProgramRevenueType programRevenueType,
            RegistrationTypeEnum registrationType,
            IIdMapper<RegistrationTypeEnum, List<ClassRegistration>> frag,
            IIdMapper<RegistrationTypeEnum, PriceListItem> priceListItems
            )
        {
            var result = new ProgramRevenueItem
            {
                Type = programRevenueType,
                Quantity = GetQuantityByRegistrationType(frag, new[] { registrationType }),
                Rate = priceListItems.TryGetItem(registrationType)?.Price ?? 0.0m
            };
            return result;
        }

        private List<ProgramExpenseItem> GetProgramExpenses()
        {
            var result = new List<ProgramExpenseItem>();
            foreach (var classExpense in data.Business.ClassExpenses)
            {
                var item = new ProgramExpenseItem
                {
                    Index = classExpense.Order,
                    Text = classExpense.Text,
                    Value = ComputeExpenseValue(classExpense.ClassExpenseTypeId, classExpense.Value)
                };
                result.Add(item);
            }
            return result;
        }

        private List<PriceListItem> GetPriceListItems()
        {
            var result = new List<PriceListItem>();
            result.AddRange(data.ClassPriceList.PriceListItems);
            if (data.AssociatedLecturePriceList != null)
            {
                result.AddRange(data.AssociatedLecturePriceList.PriceListItems);
            }

            return result;
        }

        private decimal ComputeExpenseValue(ClassExpenseTypeEnum type, decimal? value)
        {
            decimal result;
            switch (type)
            {
                // custom value
                case ClassExpenseTypeEnum.Custom:
                    result = value ?? 0.0m;
                    break;
                    

                // paypal fee for Class
                case ClassExpenseTypeEnum.PayPalFeeClass:
                    var classRegistrations = data.ApprovedRegistrations
                        .Where(x => x.RegistrationTypeId != RegistrationTypeEnum.ApprovedGuest && x.RegistrationTypeId != RegistrationTypeEnum.WWA);

                    result = GetTotalPayPalFeeOfRegistrations(classRegistrations);
                    break;


                // paypal fees for WWA and Lecture
                case ClassExpenseTypeEnum.PayPalFeeWwaLecture:
                    var wwaRegistrations = data.ApprovedRegistrations
                        .Where(x => x.RegistrationTypeId == RegistrationTypeEnum.WWA).ToList();
                    var lectureRegistrations = data.ApprovedRegistrationsOfAssociatedLecture
                        .Where(x => x.RegistrationTypeId == RegistrationTypeEnum.LectureRegistration).ToList();

                    result = GetTotalPayPalFeeOfRegistrations(wwaRegistrations);
                    result += GetTotalPayPalFeeOfRegistrations(lectureRegistrations);
                    break;


                // gets foi royalty fee
                case ClassExpenseTypeEnum.FoiRoyaltyFee:
                    result = GetFoiRoyaltyFee();
                    break;


                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return result;
        }       

        private decimal GetTotalPayPalFeeOfRegistrations(IEnumerable<ClassRegistration> registrations)
        {
            var result = registrations.Sum(x => x.ClassRegistrationPayment.PayPalFee ?? 0.0m);
            return result;
        }

        private int GetInstructorAndStaffCount()
        {
            var personIds = new HashSet<long>();
            if (data.Class.GuestInstructorId.HasValue)
                personIds.Add(data.Class.GuestInstructorId.Value);

            var instructorsApprovedStaffIds = data.Class.ClassPersons
                .Where(x => x.RoleTypeId == PersonRoleTypeEnum.Instructor || x.RoleTypeId == PersonRoleTypeEnum.ApprovedStaff)
                .Select(x => x.PersonId).ToList();

            personIds.UnionWith(instructorsApprovedStaffIds);
            var result = personIds.Count;
            return result;
        }

        private decimal GetFoiRoyaltyFee()
        {
            var totalRemittance = RoyaltyFees.Items.Sum(x => x.Quantity * x.Rate);
            var totalStudents = RoyaltyFees.Items.Sum(x => x.Quantity);

            if (totalStudents < 40)
                totalRemittance *= 0.75m;

            var totalReimbursement = ReimbursementForPrintingQuantity * (data.Business.PrintReimbursement ?? 0.0m);
            var result = totalRemittance - totalReimbursement;
            return result;
        }

        #endregion

        #region helper methods

        private IIdMapper<RegistrationTypeEnum, List<ClassRegistration>> FragmentRegistrationsByType(
            IEnumerable<ClassRegistration> registrations)
        {
            var grouped = registrations.GroupBy(x => x.RegistrationTypeId).Select(x => x.ToList());
            var result = new IdMapper<RegistrationTypeEnum, List<ClassRegistration>>(grouped,
                x => x.First().RegistrationTypeId, y => new List<ClassRegistration>());
            return result;
        }

        private int GetQuantityByRegistrationType(IIdMapper<RegistrationTypeEnum, List<ClassRegistration>> fragmented,
            params RegistrationTypeEnum[] types)
        {
            var result = types.Sum(type => fragmented.TryGetItem(type).Count);
            return result;
        }

        #endregion
    }
}
