using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Reports.System.Models.FinancialBusiness;
using PerfectlyMadeInc.Helpers.Contract.Structures;

namespace AutomationSystem.Main.Core.Reports.System.DataProviders
{
    /// <summary>
    /// The class that encapsulates financial business logic
    /// </summary>
    public interface IClassFinancialBusinessLogic
    {
        bool IsGuestInstructorRevenueAvailable { get; }

        int TotalPaidStudentsForClassAndWwa { get; }

        int ReimbursementForPrintingQuantity { get; }

        decimal ClassRevenue { get; }

        decimal GuestInstructorFee { get; }

        IIdMapper<RoyaltyFeeRateTypeEnum, RoyaltyFeeItem> RoyaltyFees { get; }

        IIdMapper<ProgramRevenueType, ProgramRevenueItem> ProgramRevenues { get; }

        List<ProgramExpenseItem> ProgramExpenses { get; }
    }
}
