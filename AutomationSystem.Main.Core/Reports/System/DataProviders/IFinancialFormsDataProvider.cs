using AutomationSystem.Main.Core.Reports.System.Models.FinancialForms;

namespace AutomationSystem.Main.Core.Reports.System.DataProviders
{
    /// <summary>
    /// Financial forms data provider
    /// </summary>
    public interface IFinancialFormsDataProvider
    {
        FoiRoyaltyForm GetFoiRoyaltyFormModel();

        FaClosingStatement GetFaClosingStatementModel();
        
        GuestInstructorStatement GetGuestInstructorStatementModel();
    }
}
