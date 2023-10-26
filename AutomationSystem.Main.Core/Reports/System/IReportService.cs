using System.Collections.Generic;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Shared.Contract.Files.System.Models;

namespace AutomationSystem.Main.Core.Reports.System
{
    /// <summary>
    /// Provides reports and certificates
    /// Not intended for usage by controllers
    /// </summary>
    public interface IReportService
    {
        List<ClassReportItem> GetClassReportsItemsByClassId(long classId);

        List<long> GenerateClassReportsForMasterCoordinator(string rootPath, long classId);

        List<long> GenerateClassReportsForDailyReports(string rootPath, long classId);

        List<long> GenerateClassReportsForFinalReports(string rootPath, long classId);

        FileForDownload GetClassReportByType(ClassReportType reportType, string rootPath, long classId);

        #region other

        Dictionary<string, object> GetRegistrationListTextMap(long classId);

        #endregion
    }
}