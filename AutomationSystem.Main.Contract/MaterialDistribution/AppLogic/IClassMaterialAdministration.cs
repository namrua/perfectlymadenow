using System.IO;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Shared.Contract.Files.System.Models;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic
{

    /// <summary>
    /// Provides class materials administration
    /// </summary>
    public interface IClassMaterialAdministration
    {

        #region class materials & monitoring

        // gets class materials page model
        ClassMaterialsPageModel GetClassMaterialsPageModel(long classId);

        // gets class material form by class id
        ClassMaterialForm GetClassMaterialFormByClassId(long classId);

        // saves class materials
        void SaveClassMaterial(ClassMaterialForm form);

        #endregion


        #region class material commands

        // unlocks class
        void UnlockClassMaterials(long classId);

        // lock class
        void LockClassMaterials(long classId);

        // send material notification
        void SendMaterialNotification(long classId);

        #endregion


        #region class material file administration

        // gets new ClassMaterialFileForEdit
        ClassMaterialFileForEdit GetNewClassMaterialFileForEdit(long classId);

        // gets ClassMaterialFileForEdit by classMaterialFileId
        ClassMaterialFileForEdit GetClassMaterialFileForEditById(long classMaterialFileId);

        // gets ClassMaterialFileForEdit by form
        ClassMaterialFileForEdit GetClassMaterialFileForEditByForm(ClassMaterialFileForm form);

        // saves ClassMaterialFile
        void SaveClassMaterialFile(ClassMaterialFileForm form, Stream pdfMaterial, string pdfMaterialName);

        // deletes class material file
        long DeleteClassMaterialFileAndReturnClassId(long classMaterialFileId);

        #endregion


        #region material recipient administraiton

        // gets MaterialRecipientPageModel by recipient id
        MaterialRecipientPageModel GetMaterialRecipientPageModelByRecipientId(RecipientId recipientId);

        // gets MaterialRecipientPageModel by materials recipient id
        MaterialRecipientPageModel GetMaterialRecipientPageModelByMaterialRecipientId(long materialRecipientId);

        // gets MaterialRecipientForEdit by materialRecipientId
        MaterialRecipientForEdit GetMaterialRecipientForEdit(long materialRecipientId);

        // gets MaterialRecipientForEdit by form
        MaterialRecipientForEdit GetMaterialRecipientForEditByForm(MaterialRecipientForm form);

        // saves material recipient
        void SaveClassMaterialRecipient(MaterialRecipientForm form);

        // get encrypted material for download 
        FileForDownload GetMaterialForDownload(long materialRecipientId, long classMaterialFileId);

        #endregion


        #region material recipient commands

        // unlocks material recipient
        RecipientId UnlockMaterialRecipient(long materialRecipientId);

        // lock material recipient
        RecipientId LockMaterialRecipient(long materialRecipientId);

        // send material notification
        RecipientId SendMaterialNotificationToRecipient(long materialRecipientId);

        // distributes material to recipient including all necessary checks
        void DistributeMaterialsToRecipient(long classId, RecipientId recipientId, bool isHandledFromPublicPages);

        #endregion

        #region monitoring

        // gets class materials monitoring page model
        ClassMaterialMonitoringPageModel GetClassMaterialMonitoringPageModel(long classId, EntityTypeEnum recipientTypeId);

        #endregion
    }
}
