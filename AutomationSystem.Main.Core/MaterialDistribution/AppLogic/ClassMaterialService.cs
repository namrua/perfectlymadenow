using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Home.AppLogic;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Core.MaterialDistribution.Data;
using AutomationSystem.Main.Core.MaterialDistribution.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Files.System;
using AutomationSystem.Shared.Contract.Files.System.Models;
using AutomationSystem.Shared.Contract.Localisation.System;
using PerfectlyMadeInc.Helpers.Database;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic
{
    /// <summary>
    /// Provides class materials and related services
    /// </summary>
    public class ClassMaterialService : IClassMaterialService
    {
        private readonly IClassDatabaseLayer classDb;
        private readonly IClassMaterialDatabaseLayer materialsDb;
        private readonly ICoreFileService coreFileService;
        private readonly ILocalisationService localisationService;
        private readonly IMaterialRecipientIntegrationProvider recipientIntegrationProvider;
        private readonly IPdfEncryptor encryptor;
        private readonly IMaterialAvailabilityResolver materialAvailabilityResolver;

        // constructor
        public ClassMaterialService(
            IClassDatabaseLayer classDb,
            IClassMaterialDatabaseLayer materialsDb, 
            ICoreFileService coreFileService,
            ILocalisationService localisationService,
            IMaterialRecipientIntegrationProvider recipientIntegrationProvider,
            IPdfEncryptor encryptor,
            IMaterialAvailabilityResolver materialAvailabilityResolver)
        {
            this.classDb = classDb;
            this.materialsDb = materialsDb;
            this.coreFileService = coreFileService;
            this.localisationService = localisationService;
            this.recipientIntegrationProvider = recipientIntegrationProvider;
            this.encryptor = encryptor;
            this.materialAvailabilityResolver = materialAvailabilityResolver;
        }

        // returns request info or null when request code was not found
        public ClassMaterialRequestInfo GetRequestInfo(string requestCode)
        {
            // gets full info by request
            var fullInfo = GetFullRequestInfo(requestCode);
            if (fullInfo == null)
            {
                throw HomeServiceException.New(HomeServiceErrorType.MaterialsNotAvailable, "Invalid request code")
                    .AddId(token: requestCode);
            }

            // checks availability
            if (!fullInfo.Availability.AreMaterialsAvailable)
            {
                throw HomeServiceException
                    .New(HomeServiceErrorType.MaterialsNotAvailable, fullInfo.Availability.Message)
                    .AddId(
                        classId: fullInfo.Class.ClassId,
                        entityId: new RecipientId(fullInfo.ClassMaterialRecipient.RecipientTypeId, fullInfo.ClassMaterialRecipient.RecipientId),
                        token: requestCode);
            }

            // assembles result
            var result = new ClassMaterialRequestInfo
            {
                ClassId = fullInfo.Class.ClassId,
                ClassMaterialId = fullInfo.ClassMaterial.ClassMaterialId,
                ClassMaterialRecipientId = fullInfo.ClassMaterialRecipient.ClassMaterialRecipientId,
                LanguageId = fullInfo.ClassMaterialRecipient.LanguageId,
                Availability = fullInfo.Availability
            };
            return result;
        }


        // gets public language selection page model
        public PublicLanguageSelectionPageModel GetPublicLanguageSelectionPageModel(string requestCode, long classId)
        {
            // loads class 
            var cls = classDb.GetClassById(classId);
            if (cls == null)
            {
                throw new ArgumentException($"There is no Class with {classId}.");
            }

            // loads localised languages
            var languages = new List<IEnumItem>();
            languages.Add(localisationService.GetLocalisedEnumItem(EnumTypeEnum.Language, (int)cls.OriginLanguageId));
            if (cls.TransLanguageId.HasValue)
            {
                languages.Add(localisationService.GetLocalisedEnumItem(EnumTypeEnum.Language,
                    (int) cls.TransLanguageId.Value));
            }

            // assembles result
            var result = new PublicLanguageSelectionPageModel
            {
                Languages = languages,
                RequestCode = requestCode
            };
            return result;
        }

        // gets public download page model
        public PublicDownloadPageModel GetPublicDownloadPageModel(long classMaterialId, long materialRecipientId, LanguageEnum languageId)
        {
            // gets files
            var materials = new List<ClassMaterialToDownload>();
            var materialFiles = materialsDb.GetClassMaterialFileByClassMaterialAndLanguage(classMaterialId, languageId);

            // gets registration
            var materialRecipient = materialsDb.GetClassMaterialRecipientById(materialRecipientId, ClassMaterialRecipientIncludes.ClassMaterialDownloadLogs);
            if (materialRecipient == null)
            {
                throw new ArgumentException($"There is no ClassMaterialRecipient with id {materialRecipientId}.");
            }

            foreach (var materialFile in materialFiles)
            {
                var downloadCount = materialRecipient.ClassMaterialDownloadLogs.Count(x => x.WasDownloaded && x.ClassMaterialFileId == materialFile.ClassMaterialFileId);
                var file = new ClassMaterialToDownload
                {
                    ClassMaterialFileId = materialFile.ClassMaterialFileId,
                    IsMaterialAvailable = materialAvailabilityResolver.ResolveDownloadRestrictions(materialRecipient, downloadCount).AreMaterialsAvailable,
                    Name = materialFile.DisplayName,
                };
                materials.Add(file);
            }

            // assembles result
            var result = new PublicDownloadPageModel
            {
                Materials = materials,
                RequestCode = materialRecipient.RequestCode
            };
            return result;
        }


        // sets language for registration
        public void SetClassRegistrationMaterialLanguage(string requestCode, LanguageEnum languageId)
        {
            var materialRecipient = materialsDb.GetClassMaterialRecipientByRequestCode(requestCode);
            if (materialRecipient == null)
            {
                throw HomeServiceException.New(HomeServiceErrorType.MaterialsNotAvailable, "Invalid request code").AddId(token: requestCode);
            }

            if (materialRecipient.LanguageId.HasValue)
            {
                throw new InvalidOperationException(
                    $"The ClassMaterialRecipient with id {materialRecipient.ClassMaterialRecipientId} has already set language.");
            }

            materialsDb.SetClassMaterialRecipientLanguage(requestCode, languageId);
        }


        // get material for download
        public FileForDownload GetMaterialForDownload(string requestCode, long classMaterialFileId, WebRequestInfo requestInfo)
        {
            // gets request info
            var fullInfo = GetFullRequestInfo(requestCode);
            if (fullInfo == null)
            {
                throw HomeServiceException.New(HomeServiceErrorType.MaterialsNotAvailable, "Invalid request code").AddId(token: requestCode);
            }

            // gets class materail file Id
            var materialFile = materialsDb.GetClassMaterialFileById(classMaterialFileId);
            if (materialFile == null)
            {
                throw new ArgumentException($"There is no ClassMaterialFile with id {classMaterialFileId}.");
            }

            if (materialFile.ClassMaterialId != fullInfo.ClassMaterial.ClassMaterialId)
            {
                throw new SecurityException(
                    $"Inconsistent request data: classMaterialFileId {classMaterialFileId} does not match fileId {fullInfo.ClassMaterial.ClassMaterialId} related to request code {requestCode}.");
            }

            // checks material file and download conditions
            var materialRecipient = fullInfo.ClassMaterialRecipient;
            var downloadCount = materialRecipient.ClassMaterialDownloadLogs.Count(x => x.WasDownloaded && x.ClassMaterialFileId == classMaterialFileId);
            var availability = fullInfo.Availability
                .MergeResults(materialAvailabilityResolver.ResolveFileRestrictions(materialFile, fullInfo.ClassMaterialRecipient))
                .MergeResults(materialAvailabilityResolver.ResolveDownloadRestrictions(fullInfo.ClassMaterialRecipient, downloadCount));

            // validates request
            if (!availability.AreMaterialsAvailable)
            {
                LogFailedDownload(
                    materialRecipient.ClassMaterialRecipientId,
                    classMaterialFileId,
                    requestInfo,
                    availability.Message);
                throw HomeServiceException.New(HomeServiceErrorType.MaterialsNotAvailable, availability.Message)
                    .AddId(
                        classId: fullInfo.Class.ClassId,
                        entityId: new RecipientId(fullInfo.ClassMaterialRecipient.RecipientTypeId, fullInfo.ClassMaterialRecipient.RecipientId),
                        token: requestCode);
            }

            // loads file and performs encryption
            var result = coreFileService.GetFileForDownloadById(materialFile.FileId);
            result.Content = encryptor.Encrypt(result.Content, fullInfo.ClassMaterial.CoordinatorPassword, fullInfo.ClassMaterialRecipient.Password);
            LogSuccessfulDownload(
                materialRecipient.ClassMaterialRecipientId,
                classMaterialFileId,
                requestInfo);
            return result;
        }

        #region private methods

        // returns request info or null when request code was not found
        public MaterialRequestFullInfo GetFullRequestInfo(string requestCode)
        {
            var materialRecipient = materialsDb.GetClassMaterialRecipientByRequestCode(
                requestCode, 
                ClassMaterialRecipientIncludes.ClassMaterialClassClassPersons | ClassMaterialRecipientIncludes.ClassMaterialDownloadLogs);
            if (materialRecipient == null)
            {
                return null;
            }

            var recipientIntegration = recipientIntegrationProvider.GetRecipientIntegrationByTypeId(materialRecipient.RecipientTypeId);

            var classMaterials = materialRecipient.ClassMaterial;
            var cls = materialRecipient.ClassMaterial.Class;
            var availability = materialAvailabilityResolver.ResolveClassRestrictions(cls, classMaterials)
                .MergeResults(materialAvailabilityResolver.ResolveMaterialRecipientRestrictions(materialRecipient))
                .MergeResults(recipientIntegration.ResolveRecipientRestrictions(materialRecipient.RecipientId, cls));

            var result = new MaterialRequestFullInfo
            {
                Class = cls,
                ClassMaterial = classMaterials,
                ClassMaterialRecipient = materialRecipient,
                Availability = availability
            };
            return result;
        }

        // logs successful download
        private void LogSuccessfulDownload(
            long materialRecipientId,
            long materialFileId, 
            WebRequestInfo requestInfo)
        {
            var log = new ClassMaterialDownloadLog
            {
                ClassMaterialRecipientId = materialRecipientId,
                ClassMaterialFileId = materialFileId,
                Time = DateTime.Now,
                IpAddress = DatabaseHelper.TrimNVarchar(requestInfo.IpAddress ?? "unknown", true, 32),
                WasDownloaded = true,
            };
            materialsDb.InsertClassMaterialDownloadLog(log);
        }

        // loags failed download
        private void LogFailedDownload(
            long materialRecipientId,
            long materialFileId,
            WebRequestInfo requestInfo,
            string message)
        {
            var log = new ClassMaterialDownloadLog
            {
                ClassMaterialRecipientId = materialRecipientId,
                ClassMaterialFileId = materialFileId,
                Time = DateTime.Now,
                IpAddress = DatabaseHelper.TrimNVarchar(requestInfo.IpAddress ?? "unknown", true, 32),
                WasDownloaded = false,
                Message = DatabaseHelper.TrimNVarchar(message, false, 512)
            };
            materialsDb.InsertClassMaterialDownloadLog(log);
        }

        #endregion
    }
}
