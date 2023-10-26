using System.Collections.Generic;
using AutomationSystem.Main.Core.Integration.System.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Reports.System.DataProviders
{
    /// <summary>
    /// Provides class data for business purposes
    /// </summary>
    public interface IClassDataProvider
    {
        Class Class { get; }

        List<Person> Persons { get; }

        ClassBusiness Business { get; }

        ClassReportSetting ReportSetting { get; }

        List<ClassRegistration> ApprovedRegistrations { get; }

        List<ClassRegistration> ApprovedRegistrationsOfAssociatedLecture { get; }

        List<EventLocationInfo> EventLocationInfo { get; }

        List<RoyaltyFeeRate> RoyaltyFeeRates { get; }

        PriceList ClassPriceList { get; }

        PriceList AssociatedLecturePriceList { get; }
    }
}