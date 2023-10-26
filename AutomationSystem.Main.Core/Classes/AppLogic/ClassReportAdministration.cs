using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.Classes.AppLogic;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Reports;
using AutomationSystem.Main.Core.Classes.AppLogic.Factories;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Classes.System.Emails;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Core.Emails.System.Models;
using AutomationSystem.Main.Core.FileServices.System;
using AutomationSystem.Main.Core.FileServices.System.Models;
using AutomationSystem.Main.Core.MainAsyncRequestManagers.System;
using AutomationSystem.Main.Core.Persons.AppLogic.Convertors;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Persons.Data.Models;
using AutomationSystem.Main.Core.Preferences.System;
using AutomationSystem.Main.Core.Reports.System;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;
using System;
using System.Linq;

namespace AutomationSystem.Main.Core.Classes.AppLogic
{
    public class ClassReportAdministration : IClassReportAdministration
    {
        private readonly IClassDatabaseLayer classDb;
        private readonly IPersonDatabaseLayer personDb;
        private readonly IMainFileService mainFileService;
        private readonly IMainAsyncRequestManager asyncRequestManager;
        private readonly IReportService reportService;
        private readonly IMainPreferenceProvider mainPreferences;
        private readonly IIdentityResolver identityResolver;
        private readonly IClassReportSettingFactory classReportSettingFactory;
        private readonly IMainMapper mainMapper;
        private readonly IEmailAttachmentProviderFactory emailAttachmentProviderFactory;
        private readonly IPersonConvertor personConvertor;
        private readonly IClassEmailService classEmailService;
        private readonly IClassTypeResolver classTypeResolver;

        public ClassReportAdministration(
            IClassDatabaseLayer classDb,
            IPersonDatabaseLayer personDb,
            IMainFileService mainFileService,
            IMainAsyncRequestManager asyncRequestManager,
            IReportService reportService,
            IMainPreferenceProvider mainPreferences,
            IIdentityResolver identityResolver,
            IMainMapper mainMapper,
            IClassReportSettingFactory classReportSettingFactory, 
            IEmailAttachmentProviderFactory emailAttachmentProviderFactory,
            IPersonConvertor personConvertor,
            IClassEmailService classEmailService,
            IClassTypeResolver classTypeResolver)
        {
            this.classDb = classDb;
            this.personDb = personDb;
            this.mainFileService = mainFileService;
            this.asyncRequestManager = asyncRequestManager;
            this.reportService = reportService;
            this.mainPreferences = mainPreferences;
            this.identityResolver = identityResolver;
            this.mainMapper = mainMapper;
            this.classReportSettingFactory = classReportSettingFactory;
            this.emailAttachmentProviderFactory = emailAttachmentProviderFactory;
            this.personConvertor = personConvertor;
            this.classEmailService = classEmailService;
            this.classTypeResolver = classTypeResolver;
        }

        #region report settings

        public ClassReportSettingForEdit GetClassReportSettingForEditByClassId(long classId)
        {
            var cls = GetClassById(classId, ClassIncludes.ClassReportSetting);
            identityResolver.CheckEntitleForClass(cls);
            
            var result = classReportSettingFactory.CreateClassReportSettingForEdit(cls.ProfileId);
            result.Form = mainMapper.Map<ClassReportSettingForm>(cls.ClassReportSetting);
            result.Form.ClassId = classId;
            return result;
        }

        public ClassReportSettingForEdit GetClassReportSettingForEditByForm(ClassReportSettingForm form)
        {
            var cls = GetClassById(form.ClassId);
            identityResolver.CheckEntitleForClass(cls);

            var result = classReportSettingFactory.CreateClassReportSettingForEdit(cls.ProfileId);
            result.Form = form;
            return result;
        }

        public void SaveClassReportSetting(ClassReportSettingForm form)
        {
            var toCheck = GetClassById(form.ClassId);
            identityResolver.CheckEntitleForClass(toCheck);

            var toUpdate = mainMapper.Map<ClassReportSetting>(form);
            classDb.UpdateClassReportSettingByClassId(form.ClassId, toUpdate);
        }

        #endregion

        #region reports

        public ClassReportsPageModel GetClassReportsPageModel(long classId)
        {
            var cls = GetClassById(classId, ClassIncludes.ClassType);
            identityResolver.CheckEntitleForClass(cls);

            var result = new ClassReportsPageModel();
            result.Class = ClassConvertor.ConvertToClassShortDetial(cls);

            // checks whether reports are allowed
            if (!classTypeResolver.AreReportsAllowed(cls.ClassCategoryId))
            {
                result.AreReportsDisabled = true;
                result.ReportsDisabledMessage = "Reports are not available for the class.";
                return result;
            }

            // resolves privileges for class
            result.IsSupervisedByMaster = classTypeResolver.IsSupervisedByMasterCoordinator(cls.ClassCategoryId);
            result.MasterCoordinatorEmail = mainPreferences.GetMasterCoordinatorEmail();

            // files and request
            result.Reports = reportService.GetClassReportsItemsByClassId(classId);
            result.Files = mainFileService.GetClassFileDetails(classId)
                .Where(x => MainFileReservedNames.IsReportCode(x.Code)).ToList();
            result.GeneratingRequests = asyncRequestManager.GetLastRequestsByEntityAndTypes(EntityTypeEnum.MainClass,
                classId, AsyncRequestTypeEnum.GenerateFinancialForms);
            return result;
        }

        public long GenerateFinancialForms(long classId)
        {
            var toCheck = GetClassById(classId);
            identityResolver.CheckEntitleForClass(toCheck);

            var result = asyncRequestManager.AddDocumentRequestForClass(classId, AsyncRequestTypeEnum.GenerateFinancialForms, (int)SeverityEnum.High);
            return result.AsyncRequestId;
        }

        public ClassRecipientSelectionForEdit GetClassRecipientSelectionForEdit(long classId, ClassCommunicationType type)
        {
            // loads persons
            var cls = GetClassById(classId, ClassIncludes.ClassPersons);
            identityResolver.CheckEntitleForClass(cls);

            var recipientIdsWithRoles = ClassConvertor.GetClassPersonIdsWithRoles(cls);
            var persons = personDb.GetPersonsByIds(recipientIdsWithRoles.Keys, PersonIncludes.Address);

            // assembles result
            var result = new ClassRecipientSelectionForEdit
            {
                Recipients = persons.Select(x => personConvertor.ConvertToPersonShortDetailWithCustomRoles(x, recipientIdsWithRoles[x.PersonId])).ToList()
            };
            result.Form.ClassId = classId;
            result.Form.Type = type;           
            return result;
        }

        public void SendMessageToRecipients(ClassRecipientSelectionForm form)
        {
            var toCheck = GetClassById(form.ClassId);
            identityResolver.CheckEntitleForClass(toCheck);

            switch (form.Type)
            {
                // sends registration list to recipients
                case ClassCommunicationType.RegistrationList:
                    var registrationList = reportService.GetRegistrationListTextMap(form.ClassId);
                    classEmailService.SendClassTextMapEmailByTypeToPersons(EmailTypeEnum.RegistrationList,
                        form.ClassId, registrationList, form.RecipientIds);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(form.Type), $"Unknown class communication type {form.Type}.");
            }
        }

        public void SendRegistrationListToMasterCoordinator(string rootPath, long classId)
        {
            var cls = GetClassById(classId);
            identityResolver.CheckEntitleForClass(cls);

            if (!classTypeResolver.IsSupervisedByMasterCoordinator(cls.ClassCategoryId))
                throw new InvalidOperationException($"Class with class category {cls.ClassCategoryId} is not supervised by master coordinator.");

            // generates reports for master coordinator           
            var classFileIds = reportService.GenerateClassReportsForMasterCoordinator(rootPath, classId);

            // prepares data for email and send email to master coordinator
            var textMap = reportService.GetRegistrationListTextMap(classId);
            var attachmentFileIds = mainFileService.GetFileIdsByClassFileIds(classFileIds);
            var attachments = emailAttachmentProviderFactory.CreateSimpleEmailAttachmentProvider(attachmentFileIds.ToArray());
            classEmailService.SendClassTextMapEmailByTypeToRecipient(EmailTypeEnum.RegistrationListMaster, classId,
                textMap, RecipientType.MasterCoordinator, attachments);
        }

        #endregion

        #region private methods

        public Class GetClassById(long classId, ClassIncludes includes = ClassIncludes.None)
        {
            var result = classDb.GetClassById(classId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Class with id {classId}.");
            }

            return result;
        }

        #endregion
    }
}