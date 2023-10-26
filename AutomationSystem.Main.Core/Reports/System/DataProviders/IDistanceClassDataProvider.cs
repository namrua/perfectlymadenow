using System.Collections.Generic;
using AutomationSystem.Main.Core.Reports.System.Models.DistanceReportService;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Reports.System.DataProviders
{
    /// <summary>
    /// Provides data for distance reports
    /// </summary>
    public interface IDistanceClassDataProvider
    {
        DistanceCrfReportParameters Parameters { get; }

        Person DistanceCoordinator { get; }

        List<ClassRegistration> RegistrationsWithClasses { get; }
    }
}