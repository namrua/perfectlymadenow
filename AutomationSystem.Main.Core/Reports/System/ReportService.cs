using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.FileServices.System;
using AutomationSystem.Main.Core.FileServices.System.Models;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Reports.System.DataProviders;
using AutomationSystem.Main.Core.Reports.System.Models.ReportService;
using AutomationSystem.Main.Core.Reports.System.ReportGenerators;
using AutomationSystem.Shared.Contract.Files.System.Models;

namespace AutomationSystem.Main.Core.Reports.System
{
    /// <summary>
    /// Reporting interface
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly IMainFileService mainFileService;      
        private readonly IClassReportFactory reportFactory;                                                    

        public ReportService(IMainFileService mainFileService, IClassReportFactory reportFactory)
        {                          
            this.mainFileService = mainFileService;          
            this.reportFactory = reportFactory;                                   
        }

        public List<ClassReportItem> GetClassReportsItemsByClassId(long classId)
        {
            var components = reportFactory.InitializeClassReportComponentsForClass(classId);
            var availableReportTypes = components.AvailabilityResolver.GetAvailableReportTypes();
            var generators = reportFactory.GetClassReportGeneratorsByReportTypes(availableReportTypes);
            var result = generators.Select(x => GetClassReportItem(x.TypeInfo, components.Data)).ToList();
            return result;
        }

        public List<long> GenerateClassReportsForMasterCoordinator(string rootPath, long classId)
        {
            var components = reportFactory.InitializeClassReportComponentsForClass(classId);
            var reportTypesToGenerate = components.AvailabilityResolver.GetReportTypesForMasterCoordinatorEmail();
            var result = GenerateAndSaveReports(reportTypesToGenerate, components, rootPath);
            return result;
        }
        
        public List<long> GenerateClassReportsForDailyReports(string rootPath, long classId)
        {
            var components = reportFactory.InitializeClassReportComponentsForClass(classId);
            var reportTypesToGenerate = components.AvailabilityResolver.GetReportTypesForDailyReports();
            var result = GenerateAndSaveReports(reportTypesToGenerate, components, rootPath);
            return result;
        }

        public List<long> GenerateClassReportsForFinalReports(string rootPath, long classId)
        {
            var components = reportFactory.InitializeClassReportComponentsForClass(classId);
            var reportTypesToGenerate = components.AvailabilityResolver.GetReportTypesForFinalReports();
            var result = GenerateAndSaveReports(reportTypesToGenerate, components, rootPath);
            return result;
        }

        public FileForDownload GetClassReportByType(ClassReportType reportType, string rootPath, long classId)
        {            
            var components = reportFactory.InitializeClassReportComponentsForClass(classId);            
            CheckAvailability(components, reportType);

            // generates content
            var generator = reportFactory.GetClassReportGeneratorByReportType(reportType);
            var content = generator.GenerateReport(components, rootPath, GetReportFolder(components));

            // creates result          
            var fileName = GetReportFileName(generator.TypeInfo, components.Data);
           
            var result = new FileForDownload
            {
                Content = content,
                FileName = fileName + generator.TypeInfo.FileExtension,
                MimeType = generator.TypeInfo.MimeType
            };
            return result;
        }
        
        #region other documents

        public Dictionary<string, object> GetRegistrationListTextMap(long classId)
        {
            // loads approved registrations            
            var components = reportFactory.InitializeClassReportComponentsForClass(classId);
           
            // gets registration list text            
            var registrationListText = components.Common.GetRegistrationListText();
           
            // assembles result
            var result = new Dictionary<string, object>();
            result.Add(ReportConstants.RegistrationListParameter, registrationListText);
            return result;
        }

        #endregion

        #region private methods

        private string GetReportFileName(ClassReportTypeInfo classReportTypeInfo, IClassDataProvider classDataProvider)
        {
            var result = MainTextHelper.GetReportFileName(classReportTypeInfo.FileNameBase, 
                classDataProvider.ReportSetting.LocationCode, classDataProvider.Class.EventStart);
            return result;
        }

        private ClassReportItem GetClassReportItem(ClassReportTypeInfo info, IClassDataProvider data)
        {
            var result = new ClassReportItem
            {
                ReportType = info.ReportType,
                FileTypeId = info.FileTypeId,
                Name = GetReportFileName(info, data)
            };
            return result;
        }

        private List<long> GenerateAndSaveReports(HashSet<ClassReportType> reportTypesToGenerate, IClassReportComponents components, string rootPath)
        {
            var result = new List<long>();
            var generators = reportFactory.GetClassReportGeneratorsByReportTypes(reportTypesToGenerate);
            foreach (var generator in generators)
            {
                var classFileId = GenerateAndSaveReport(generator, components, rootPath);
                result.Add(classFileId);
            }
            return result;
        }

        private string GetReportFolder(IClassReportComponents components)
        {
            return components.Data.Class.Currency.Name;
        }

        private long GenerateAndSaveReport(IClassReportGenerator generator, IClassReportComponents components, string rootPath)
        {
            var info = generator.TypeInfo;
            var fileName = GetReportFileName(info, components.Data);
            var fileToSave = new EntityFileToSave
            {
                EntityId = components.Data.Class.ClassId,
                EntityTypeId = EntityTypeEnum.MainClass,
                Code = info.ReservedCode,
                DisplayedName = fileName,
                FileName = fileName + info.FileExtension,
                FileTypeId = info.FileTypeId,
                Content = generator.GenerateReport(components, rootPath, GetReportFolder(components))
            };
            var result = mainFileService.SaveClassFile(fileToSave);
            return result;
        }
        
        private void CheckAvailability(IClassReportComponents components, ClassReportType reportType)
        {
            if (!components.AvailabilityResolver.IsReportTypeAvailable(reportType))
                throw new InvalidOperationException($"Report type {reportType} is not available for Class with id {components.Data.Class.ClassId}.");
        }

        #endregion
    }
}
