using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Convertors;
using AutomationSystem.Main.Core.MaterialDistribution.Data;
using AutomationSystem.Main.Core.MaterialDistribution.Data.Models;
using AutomationSystem.Main.Core.MaterialDistribution.System.Emails;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Files.System;
using AutomationSystem.Shared.Contract.Files.System.Models;
using AutomationSystem.Shared.Contract.Incidents.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using AutomationSystem.Main.Core.Classes.System;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic
{
    /// <summary>
    /// Provides class materials administration
    /// </summary>
    public class ClassMaterialAdministration : IClassMaterialAdministration
    {
        private readonly IClassMaterialDatabaseLayer materialDb;
        private readonly ICoreFileService coreFileService;
        private readonly IIdentityResolver identityResolver;
        private readonly IIncidentLogger incidentLogger;
        private readonly IClassMaterialConvertor materialConvertor;
        private readonly IClassMaterialFileConvertor materialFileConvertor;
        private readonly IClassMaterialRecipientConvertor materialRecipientConvertor;
        private readonly IClassMaterialMonitoringConvertor monitoringConvertor;
        private readonly IPdfEncryptor encryptor;
        private readonly IMaterialRecipientIntegrationProvider recipientIntegrationProvider;
        private readonly IClassOperationChecker classOperationChecker;
        private readonly IClassMaterialOperationChecker classMaterialOperationChecker;
        private readonly IMaterialEmailService materialEmailService;
        private readonly IClassTypeResolver classTypeResolver;


        // constructor
        public ClassMaterialAdministration(
            IClassMaterialDatabaseLayer materialDb,
            ICoreFileService coreFileService,
            IIdentityResolver identityResolver,
            IMaterialRecipientIntegrationProvider recipientIntegrationProvider,
            IIncidentLogger incidentLogger,
            IClassMaterialConvertor materialConvertor,
            IClassMaterialFileConvertor materialFileConvertor,
            IClassMaterialRecipientConvertor materialRecipientConvertor,
            IClassMaterialMonitoringConvertor monitoringConvertor,
            IPdfEncryptor encryptor,
            IClassOperationChecker classOperationChecker,
            IClassMaterialOperationChecker classMaterialOperationChecker,
            IMaterialEmailService materialEmailService,
            IClassTypeResolver classTypeResolver)
        {
            this.materialDb = materialDb;
            this.coreFileService = coreFileService;
            this.identityResolver = identityResolver;
            this.recipientIntegrationProvider = recipientIntegrationProvider;
            this.incidentLogger = incidentLogger;
            this.materialConvertor = materialConvertor;
            this.materialFileConvertor = materialFileConvertor;
            this.materialRecipientConvertor = materialRecipientConvertor;
            this.monitoringConvertor = monitoringConvertor;
            this.encryptor = encryptor;
            this.classOperationChecker = classOperationChecker;
            this.classMaterialOperationChecker = classMaterialOperationChecker;
            this.materialEmailService = materialEmailService;
            this.classTypeResolver = classTypeResolver;
        }

        #region class material

        // gets class materials page model
        public ClassMaterialsPageModel GetClassMaterialsPageModel(long classId)
        {
            var material = GetClassMaterialByClassId(classId, ClassMaterialIncludes.ClassClassType | ClassMaterialIncludes.ClassMaterialFiles);
            var cls = material.Class;
            identityResolver.CheckEntitleForClass(cls);

            var result = new ClassMaterialsPageModel();
            result.Class = ClassConvertor.ConvertToClassShortDetial(cls);

            // resolves availability of materials for class
            if (!classTypeResolver.AreMaterialsAllowed(cls.ClassCategoryId))
            {
                result.AreMaterialsDisabled = true;
                result.MaterialsDisabledMessage = "Materials are not available for the class.";
                return result;
            }

            // fills materials
            result.Detail = materialConvertor.ConvertToClassMaterialDetail(material);
            result.Materials = material.ClassMaterialFiles
                .Select(materialFileConvertor.ConvertToClassMaterialFileDetail).ToList();
            result.ClassMaterialState = classMaterialOperationChecker.GetClassMaterialState(cls, material);
            result.CanUnlockMaterials = classOperationChecker.IsOperationAllowed(ClassOperation.MaterialDistribution, result.Class.ClassState)
                && classMaterialOperationChecker.IsOperationAllowed(ClassMaterialOperation.Unlock, result.ClassMaterialState);
            result.CanSendNotification = classOperationChecker.IsOperationAllowed(ClassOperation.MaterialDistribution, result.Class.ClassState)
                && classMaterialOperationChecker.IsOperationAllowed(ClassMaterialOperation.SendNotification, result.ClassMaterialState);
            result.CanLockMaterials = classOperationChecker.IsOperationAllowed(ClassOperation.MaterialDistribution, result.Class.ClassState)
                && classMaterialOperationChecker.IsOperationAllowed(ClassMaterialOperation.Lock, result.ClassMaterialState);
            return result;
        }

        // gets class material form by class id
        public ClassMaterialForm GetClassMaterialFormByClassId(long classId)
        {
            var material = GetClassMaterialByClassId(classId, ClassMaterialIncludes.Class);
            var cls = material.Class;
            identityResolver.CheckEntitleForClass(cls);

            var result = materialConvertor.ConvertToClassMaterialForm(material, classId);
            return result;
        }

        // saves class materials
        public void SaveClassMaterial(ClassMaterialForm form)
        {
            var material = GetClassMaterialByClassId(form.ClassId, ClassMaterialIncludes.Class);
            var cls = material.Class;
            identityResolver.CheckEntitleForClass(cls);

            var toUpdate = materialConvertor.ConvertToClassMaterial(form, cls.TimeZoneId);
            materialDb.UpdateClassMaterial(form.ClassId, toUpdate);
        }

        #endregion

        #region class material commands

        // unlocks class
        public void UnlockClassMaterials(long classId)
        {
            var material = GetClassMaterialByClassId(classId, ClassMaterialIncludes.Class);
            var cls = material.Class;
            identityResolver.CheckEntitleForClass(cls);
            classOperationChecker.CheckOperation(ClassOperation.MaterialDistribution, cls);
            classMaterialOperationChecker.CheckOperation(ClassMaterialOperation.Unlock, cls, material);

            // sets class as unlocked
            materialDb.SetClassMaterialToUnlock(material.ClassMaterialId);
        }

        // lock class
        public void LockClassMaterials(long classId)
        {
            var material = GetClassMaterialByClassId(classId, ClassMaterialIncludes.Class);
            var cls = material.Class;
            identityResolver.CheckEntitleForClass(cls);
            classOperationChecker.CheckOperation(ClassOperation.MaterialDistribution, cls);
            classMaterialOperationChecker.CheckOperation(ClassMaterialOperation.Lock, cls, material);

            // sets class as unlocked
            materialDb.SetClassMaterialToLock(material.ClassMaterialId);
        }

        // send material notification
        public void SendMaterialNotification(long classId)
        {
            var material = GetClassMaterialByClassId(
                classId, 
                ClassMaterialIncludes.Class | ClassMaterialIncludes.ClassMaterialRecipients | ClassMaterialIncludes.ClassClassPersons);
            var cls = material.Class;
            classOperationChecker.CheckOperation(ClassOperation.MaterialDistribution, cls);
            classMaterialOperationChecker.CheckOperation(ClassMaterialOperation.SendNotification, cls, material);

            // loads all recipients
            var allRecipients = new HashSet<RecipientId>(
                recipientIntegrationProvider.GetAllRecipientIntegratons()
                    .SelectMany(x => x.GetAllRecipientIdsForClass(cls)));
            var existingRecipients = new HashSet<RecipientId>(material.ClassMaterialRecipients.Select(x => new RecipientId(x.RecipientTypeId, x.RecipientId)));

            // creates missing ClassMaterialRecipient entities
            var preselectedLanguageId = GetClassPreselectedLanguageId(cls);
            var newnMaterialRecipients = allRecipients
                .Where(x => !existingRecipients.Contains(x))
                .Select(x => materialRecipientConvertor.CreateInitialClassMaterialRecipient(material.ClassMaterialId, x, preselectedLanguageId))
                .ToList();
            materialDb.InsertClassMaterialRecipients(newnMaterialRecipients);

            // sends notification email to all recipients that aren't locked
            var recipientsToNotify = new List<RecipientId>();
            recipientsToNotify.AddRange(newnMaterialRecipients.Select(x => new RecipientId(x.RecipientTypeId, x.RecipientId)));
            recipientsToNotify.AddRange(material.ClassMaterialRecipients
                .Where(x => !x.IsLocked && allRecipients.Contains(new RecipientId(x.RecipientTypeId, x.RecipientId)))
                .Select(x => new RecipientId(x.RecipientTypeId, x.RecipientId)));
            var result = materialEmailService.SendMaterialEmailsToRecipients(EmailTypeEnum.MaterialsNotification, classId, recipientsToNotify);

            // process notification result
            materialDb.SetClassMaterialRecipientNotified(
                result.ProcessedEmailIdEntityIdPairs.Select(x => x.Item2),
                material.ClassMaterialId,
                DateTime.Now);

            if (result.Exception != null)
            {
                throw result.Exception;
            }
        }

        #endregion

        #region class material file administration

        // gets new ClassMaterialFileForEdit
        public ClassMaterialFileForEdit GetNewClassMaterialFileForEdit(long classId)
        {
            // check existence of the class
            var material = GetClassMaterialByClassId(classId, ClassMaterialIncludes.Class);
            identityResolver.CheckEntitleForClass(material.Class);

            var result = materialFileConvertor.InitializeClassmaterialFileForEdit();
            result.Form.ClassId = classId;
            return result;
        }

        // gets ClassMaterialFileForEdit by classMaterialFileId
        public ClassMaterialFileForEdit GetClassMaterialFileForEditById(long classMaterialFileId)
        {
            // gets material file
            var materialFile = GetClassMaterialFileById(classMaterialFileId, ClassMaterialFileIncludes.ClassMaterialClass);
            identityResolver.CheckEntitleForClass(materialFile.ClassMaterial.Class);

            // initializes material file fo edit
            var result = materialFileConvertor.InitializeClassmaterialFileForEdit();
            result.Form = materialFileConvertor.ConvertoToClassMaterialFileForm(materialFile, materialFile.ClassMaterial.ClassId);
            return result;
        }

        // gets ClassMaterialFileForEdit by form
        public ClassMaterialFileForEdit GetClassMaterialFileForEditByForm(ClassMaterialFileForm form)
        {
            // checks permissions
            var material = GetClassMaterialByClassId(form.ClassId, ClassMaterialIncludes.Class);
            identityResolver.CheckEntitleForClass(material.Class);

            // initializes material file for edit
            var result = materialFileConvertor.InitializeClassmaterialFileForEdit();
            result.Form = form;
            return result;
        }

        // saves ClassMaterialFile
        public void SaveClassMaterialFile(ClassMaterialFileForm form, Stream pdfMaterial, string pdfMaterialName)
        {
            // determines if new or existing class material file id
            if (form.ClassMaterialFileId == 0)
            {
                // loads class to obtain ClassMaterialId and checks privileges
                var material = GetClassMaterialByClassId(form.ClassId, ClassMaterialIncludes.Class);
                var cls = material.Class;
                identityResolver.CheckEntitleForClass(cls);

                // if there is no file for new materials, invalid operation exception is thrown
                if (pdfMaterial == null)
                {
                    throw new InvalidOperationException("The content has to be uploaded for new class material file.");
                }

                // saves new file
                var fileId = coreFileService.InsertFile(pdfMaterial, form.DisplayName, pdfMaterialName,
                    FileTypeEnum.Pdf, languageId: form.LanguageId);

                // converts and saves class material
                var materialFile = materialFileConvertor.ConvertToClassMaterialFile(form, material.ClassMaterialId, fileId);
                materialDb.InsertClassMateriaFile(materialFile);
            }
            else
            {
                // loads classMaterialFileId (to get ClassMaterialId and origin FileId)
                var materialFile = GetClassMaterialFileById(form.ClassMaterialFileId, ClassMaterialFileIncludes.ClassMaterialClass);
                identityResolver.CheckEntitleForClass(materialFile.ClassMaterial.Class);

                // checks whether ClassId and ClassMaterialsFileId are consistent
                var classId = materialFile.ClassMaterial.ClassId;
                if (classId != form.ClassId)
                {
                    throw new SecurityException($"Inconsistent form data: form.ClassId {form.ClassId} does not match real ClassId {classId} of ClassMaterialFile {form.ClassMaterialFileId}");
                }

                // process new file - adds it if it is possible, set newFileId value
                long? newFileId = null, fileIdToDelete = null;
                if (pdfMaterial != null)
                {
                    // saves new file and sets origin file to fileToDeleteId
                    newFileId = coreFileService.InsertFile(pdfMaterial, form.DisplayName, pdfMaterialName,
                        FileTypeEnum.Pdf, languageId: form.LanguageId);

                    // if there is 
                    fileIdToDelete = materialFile.FileId;
                }

                // converts and saves the class material file
                var materialFileToSave =
                    materialFileConvertor.ConvertToClassMaterialFile(form, materialFile.ClassMaterialId,
                        newFileId ?? 0);
                materialDb.UpdateClassMaterialFile(materialFileToSave, newFileId.HasValue);

                // deletes obsolete file 
                if (fileIdToDelete.HasValue)
                {
                    coreFileService.DeleteFile(fileIdToDelete.Value);
                }
            }
        }

        // deletes class material file
        public long DeleteClassMaterialFileAndReturnClassId(long classMaterialFileId)
        {
            // gets material file
            var materialFile = GetClassMaterialFileById(classMaterialFileId, ClassMaterialFileIncludes.ClassMaterialClass);
            identityResolver.CheckEntitleForClass(materialFile.ClassMaterial.Class);

            // deletes class materials and file
            materialDb.DeleteClassMaterialFile(classMaterialFileId);
            coreFileService.DeleteFile(materialFile.FileId);

            return materialFile.ClassMaterial.ClassId;
        }

        #endregion

        #region material recipient administraiton

        // gets MaterialRecipientPageModel by materials recipient id
        public MaterialRecipientPageModel GetMaterialRecipientPageModelByMaterialRecipientId(long materialRecipientId)
        {
            // loads material recipient
            var materialRecipient = GetClassMaterialRecipientById(
                materialRecipientId,
                ClassMaterialRecipientIncludes.ClassMaterialClassClassType
                | ClassMaterialRecipientIncludes.ClassMaterialClassMaterialFiles
                | ClassMaterialRecipientIncludes.ClassMaterialDownloadLogs);

            return GetMaterialRecipientPageModelByMaterialRecipient(materialRecipient);
        }

        // gets MaterialRecipientPageModel by recipient id
        public MaterialRecipientPageModel GetMaterialRecipientPageModelByRecipientId(RecipientId recipientId)
        {
            var result = new MaterialRecipientPageModel
            {
                RecipientId = recipientId
            };

            var recipientIntegration = recipientIntegrationProvider.GetRecipientIntegrationByTypeId(recipientId.TypeId);
            var classId = recipientIntegration.CheckAndTryGetClassId(recipientId.Id, out var materialsDisabledMessage);
            if (!classId.HasValue)
            {
                result.IsMaterialsDisabled = true;
                result.MaterialsDisabledMessage = materialsDisabledMessage;
                return result;
            }

            // loads material recipient
            var materialRecipient = materialDb.GetClassMaterialRecipientByRecipientIdAndClassId(
                recipientId,
                classId.Value,
                ClassMaterialRecipientIncludes.ClassMaterialClassClassType 
                | ClassMaterialRecipientIncludes.ClassMaterialClassMaterialFiles 
                | ClassMaterialRecipientIncludes.ClassMaterialDownloadLogs);
            if (materialRecipient == null)
            {
                result.IsMaterialsDisabled = true;
                result.MaterialsDisabledMessage = "Materials are not available yet.";
                return result;
            }

            return GetMaterialRecipientPageModelByMaterialRecipient(materialRecipient);
        }

        // gets MaterialRecipientForEdit by materialRecipientId
        public MaterialRecipientForEdit GetMaterialRecipientForEdit(long materialRecipientId)
        {
            var materialRecipient = GetClassMaterialRecipientById(materialRecipientId, ClassMaterialRecipientIncludes.ClassMaterialClass);
            var cls = materialRecipient.ClassMaterial.Class;
            identityResolver.CheckEntitleForClass(cls);

            var result = materialRecipientConvertor.InitializeMaterialRecipientForEdit(cls);
            result.Form = materialRecipientConvertor.ConvertToMaterialRecipientForm(materialRecipient);
            return result;
        }

        // gets MaterialRecipientForEdit by form
        public MaterialRecipientForEdit GetMaterialRecipientForEditByForm(MaterialRecipientForm form)
        {
            var materialRecipient = GetClassMaterialRecipientById(form.ClassMaterialRecipientId, ClassMaterialRecipientIncludes.ClassMaterialClass);
            var cls = materialRecipient.ClassMaterial.Class;
            identityResolver.CheckEntitleForClass(cls);

            var result = materialRecipientConvertor.InitializeMaterialRecipientForEdit(cls);
            result.Form = form;
            return result;
        }

        // saves material recipient
        public void SaveClassMaterialRecipient(MaterialRecipientForm form)
        {
            var materialRecipient = GetClassMaterialRecipientById(form.ClassMaterialRecipientId, ClassMaterialRecipientIncludes.ClassMaterialClass);
            var cls = materialRecipient.ClassMaterial.Class;
            identityResolver.CheckEntitleForClass(cls);

            var toUpdate = materialRecipientConvertor.ConvertToClassMaterialRecipient(form);
            materialDb.UpdateClassMaterialRecipient(toUpdate);
        }

        // get encrypted material for download 
        public FileForDownload GetMaterialForDownload(long materialRecipientId, long classMaterialFileId)
        {
            // loads material recipient, class 
            var materialRecipient = GetClassMaterialRecipientById(materialRecipientId, ClassMaterialRecipientIncludes.ClassMaterialClass);
            var classMaterial = materialRecipient.ClassMaterial;
            var cls = materialRecipient.ClassMaterial.Class;
            identityResolver.CheckEntitleForClass(cls);

            // loads material file
            var materialFile = GetClassMaterialFileById(classMaterialFileId);
            if (materialFile.ClassMaterialId != classMaterial.ClassMaterialId)
            {
                throw new SecurityException(
                    $"Inconsistent request data: classMaterialFileId {classMaterialFileId} does not match class material {classMaterial.ClassMaterialId} related to material recipient id {materialRecipientId}.");
            }

            // loads file and performs encryption
            var result = coreFileService.GetFileForDownloadById(materialFile.FileId);
            result.Content = encryptor.Encrypt(result.Content, classMaterial.CoordinatorPassword, materialRecipient.Password);
            return result;
        }

        #endregion

        #region  material recipient commands

        // unlocks material recipient
        public RecipientId UnlockMaterialRecipient(long materialRecipientId)
        {
            var materialRecipient = GetClassMaterialRecipientById(materialRecipientId, ClassMaterialRecipientIncludes.ClassMaterialClass);
            var cls = materialRecipient.ClassMaterial.Class;

            identityResolver.CheckEntitleForClass(cls);
            classOperationChecker.CheckOperation(ClassOperation.MaterialDistribution, cls);
            classMaterialOperationChecker.CheckOperation(ClassMaterialOperation.UnlockRecipient, cls, materialRecipient.ClassMaterial);
            if (!materialRecipient.IsLocked)
            {
                throw new InvalidOperationException($"Materials of material recipient with id {materialRecipientId} are already unlocked.");
            }

            materialDb.SetClassMaterialRecipientIsLocked(materialRecipientId, false);

            return new RecipientId(materialRecipient.RecipientTypeId, materialRecipient.RecipientId);
        }

        // lock material recipient
        public RecipientId LockMaterialRecipient(long materialRecipientId)
        {
            var materialRecipient = GetClassMaterialRecipientById(materialRecipientId, ClassMaterialRecipientIncludes.ClassMaterialClass);
            var cls = materialRecipient.ClassMaterial.Class;

            identityResolver.CheckEntitleForClass(cls);
            classOperationChecker.CheckOperation(ClassOperation.MaterialDistribution, cls);
            classMaterialOperationChecker.CheckOperation(ClassMaterialOperation.LockRecipient, cls, materialRecipient.ClassMaterial);
            if (materialRecipient.IsLocked)
            {
                throw new InvalidOperationException($"Materials of material recipient with id {materialRecipientId} are already locked.");
            }

            materialDb.SetClassMaterialRecipientIsLocked(materialRecipientId, true);

            return new RecipientId(materialRecipient.RecipientTypeId, materialRecipient.RecipientId);
        }

        // send material notification
        public RecipientId SendMaterialNotificationToRecipient(long materialRecipientId)
        {
            var materialRecipient = GetClassMaterialRecipientById(materialRecipientId, ClassMaterialRecipientIncludes.ClassMaterialClass);
            var cls = materialRecipient.ClassMaterial.Class;

            identityResolver.CheckEntitleForClass(cls);
            classOperationChecker.CheckOperation(ClassOperation.MaterialDistribution, cls);
            classMaterialOperationChecker.CheckOperation(ClassMaterialOperation.SendNotificationToRecipient, cls, materialRecipient.ClassMaterial);
            if (materialRecipient.IsLocked)
            {
                throw new InvalidOperationException($"Materials of material recipient with id {materialRecipientId} are locked and notification cannot be sent.");
            }

            // sends notification for material recipient
            SendNotificationForMaterialRecipientId(materialRecipient, cls);

            return new RecipientId(materialRecipient.RecipientTypeId, materialRecipient.RecipientId);
        }

        // distributes material to recipient including all necessary checks
        public void DistributeMaterialsToRecipient(long classId, RecipientId recipientId, bool isHandledFromPublicPages)
        {
            try
            {
                // loads class materials, checks whether material distribution is allowed for the class state
                var material = GetClassMaterialByClassId(classId, ClassMaterialIncludes.Class);
                var cls = material.Class;
                var classState = ClassConvertor.GetClassState(cls);
                var classMaterialState = classMaterialOperationChecker.GetClassMaterialState(cls, material);
                if (classOperationChecker.IsOperationDisabled(ClassOperation.MaterialDistribution, classState))
                {
                    return;
                }

                // checks whether class material recipient exists
                var materialRecipient = materialDb.GetClassMaterialRecipientByRecipientIdAndClassId(recipientId, cls.ClassId);
                if (materialRecipient == null)
                {
                    if (classMaterialOperationChecker.IsOperationDisabled(ClassMaterialOperation.InitializeMaterialRecipient, classMaterialState))
                    {
                        return;
                    }

                    // create class material recipient
                    var preselectedLanguageId = GetClassPreselectedLanguageId(cls);
                    materialRecipient = materialRecipientConvertor.CreateInitialClassMaterialRecipient(
                        material.ClassMaterialId,
                        recipientId,
                        preselectedLanguageId);
                    materialDb.InsertClassMaterialRecipient(materialRecipient);
                }
                
                // checks class material restrictions
                if (classMaterialOperationChecker.IsOperationDisabled(ClassMaterialOperation.SendNotificationToRecipient, classMaterialState))
                {
                    return;
                }

                // checks material recipient restrictions
                if (materialRecipient.IsLocked)
                {
                    return;
                }

                // sends notification for material recipient
                SendNotificationForMaterialRecipientId(materialRecipient, cls, isHandledFromPublicPages);
            }
            catch (Exception e)
            {
                if (!isHandledFromPublicPages) throw;
                var incident = IncidentForLog.New(IncidentTypeEnum.MaterialError, e);
                incident.Entity(recipientId);
                incidentLogger.LogIncident(incident);
            }
        }

        #endregion

        #region monitoring

        // gets class materials monitoring page model
        public ClassMaterialMonitoringPageModel GetClassMaterialMonitoringPageModel(long classId, EntityTypeEnum recipientTypeId)
        {
            // tests whether class exists and access privileges
            var material = GetClassMaterialByClassId(
                classId,
                ClassMaterialIncludes.ClassMaterialRecipientsClassMaterialDownloadLogs | ClassMaterialIncludes.ClassClassPersons);
            identityResolver.CheckEntitleForClass(material.Class);

            var materialRecipients = material.ClassMaterialRecipients.ToDictionary(x => new RecipientId(x.RecipientTypeId, x.RecipientId));
            var materialMonitoringListItemCreator = new Func<RecipientId, ClassMaterialMonitoringListItem>(recipientId =>
                {
                    if (materialRecipients.TryGetValue(recipientId, out var materialRecipient))
                    {
                        return monitoringConvertor.ConvertToClassMaterialMonitoringListItem(materialRecipient);
                    }

                    return monitoringConvertor.CreateClassMaterialMonitoringListItem(recipientId);
                });

            var recipientIntegration = recipientIntegrationProvider.GetRecipientIntegrationByTypeId(recipientTypeId);
            var recipients = recipientIntegration.GetClassMaterialMonitoringListItems(material.Class, materialMonitoringListItemCreator);

            // assembles result
            var result = new ClassMaterialMonitoringPageModel
            {
                ClassId = classId,
                Recipients = recipients
            };
            return result;
        }

        #endregion

        #region private methods

        // gets class materials by class id and checks if exists
        public ClassMaterial GetClassMaterialByClassId(long classId, ClassMaterialIncludes includes = ClassMaterialIncludes.None)
        {
            var result = materialDb.GetClassMaterialByClassId(classId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no class material for class with id {classId}");
            }

            return result;
        }

        // gets material recipient and checks if exists
        private ClassMaterialRecipient GetClassMaterialRecipientById(
            long materialRecipientId,
            ClassMaterialRecipientIncludes includes = ClassMaterialRecipientIncludes.None)
        {
            var result = materialDb.GetClassMaterialRecipientById(materialRecipientId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no ClassMaterialRecipient with id {materialRecipientId}.");
            }

            return result;
        }

        // gets class materials file and checks if exits
        private ClassMaterialFile GetClassMaterialFileById(long classMaterialFileId, ClassMaterialFileIncludes includes = ClassMaterialFileIncludes.None)
        {
            var result = materialDb.GetClassMaterialFileById(classMaterialFileId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no ClassMaterialFile with id {classMaterialFileId}.");
            }

            return result;
        }

        // figures out preselected language of class
        private LanguageEnum? GetClassPreselectedLanguageId(Class cls)
        {
            return !cls.TransLanguageId.HasValue ? cls.OriginLanguageId : (LanguageEnum?)null;
        }

        // sends notification for material recipient
        private void SendNotificationForMaterialRecipientId(ClassMaterialRecipient materialRecipient, Class cls, bool isHandledFromPublicPages = false)
        {
            // sends notification email to recipient
            var recipientToNotify = new List<RecipientId>(new[]
            {
                new RecipientId(materialRecipient.RecipientTypeId, materialRecipient.RecipientId)
            });
            var result = materialEmailService.SendMaterialEmailsToRecipients(
                EmailTypeEnum.MaterialsNotification,
                cls.ClassId,
                recipientToNotify,
                isHandledFromPublicPages);

            // process notification result
            materialDb.SetClassMaterialRecipientNotified(
                result.ProcessedEmailIdEntityIdPairs.Select(x => x.Item2),
                materialRecipient.ClassMaterialId, DateTime.Now);

            if (result.Exception != null && !isHandledFromPublicPages)
            {
                throw result.Exception;
            }
        }

        // gets MaterialRecipientPageModel by material recipient
        private MaterialRecipientPageModel GetMaterialRecipientPageModelByMaterialRecipient(ClassMaterialRecipient materialRecipient)
        {
            var cls = materialRecipient.ClassMaterial.Class;
            var material = materialRecipient.ClassMaterial;
            identityResolver.CheckEntitleForClass(cls);

            // completing count by materialFileId
            var countsByMaterialFileId = materialRecipient.ClassMaterialDownloadLogs
                .Where(x => x.WasDownloaded)
                .GroupBy(x => x.ClassMaterialFileId)
                .ToDictionary(x => x.Key, y => y.Count());
            var languageFilterId = materialRecipient.LanguageId;

            // assembles result
            var result = new MaterialRecipientPageModel
            {
                ClassMaterialRecipientId = materialRecipient.ClassMaterialRecipientId,
                RecipientId = new RecipientId(materialRecipient.RecipientTypeId, materialRecipient.RecipientId),
                Class = ClassConvertor.ConvertToClassShortDetial(cls),
                ClassMaterialState = classMaterialOperationChecker.GetClassMaterialState(cls, material),
                Detail = materialRecipientConvertor.ConvertToMaterialRecipientDetail(materialRecipient),
                Materials = material.ClassMaterialFiles
                    .Where(x => !languageFilterId.HasValue || x.LanguageId == languageFilterId)
                    .Select(x => materialFileConvertor.ConvertToClassMaterialFileDetailWithDownloadCounts(x, countsByMaterialFileId)).ToList()
            };
            result.CanSendNotification = classOperationChecker.IsOperationAllowed(ClassOperation.MaterialDistribution, result.Class.ClassState)
                && classMaterialOperationChecker.IsOperationAllowed(ClassMaterialOperation.SendNotificationToRecipient, result.ClassMaterialState)
                && !result.Detail.IsLocked;
            result.CanUnlockMaterials = classOperationChecker.IsOperationAllowed(ClassOperation.MaterialDistribution, result.Class.ClassState)
                && classMaterialOperationChecker.IsOperationAllowed(ClassMaterialOperation.UnlockRecipient, result.ClassMaterialState)
                && result.Detail.IsLocked;
            result.CanLockMaterials = classOperationChecker.IsOperationAllowed(ClassOperation.MaterialDistribution, result.Class.ClassState)
                && classMaterialOperationChecker.IsOperationAllowed(ClassMaterialOperation.LockRecipient, result.ClassMaterialState)
                && !result.Detail.IsLocked;
            return result;
        }

        #endregion
    }
}
