using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Core.MaterialDistribution.Data.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.MaterialDistribution.Data
{
    /// <summary>
    /// Provides database layer for class materials
    /// </summary>
    public interface IClassMaterialDatabaseLayer
    {
        #region class materials 

        // gets class material by id
        ClassMaterial GetClassMaterialById(long classMaterialId, ClassMaterialIncludes includes = ClassMaterialIncludes.None);

        // gets class material by class id
        ClassMaterial GetClassMaterialByClassId(long classId, ClassMaterialIncludes includes = ClassMaterialIncludes.None);

        // inserts class material
        long InsertClassMaterial(ClassMaterial classMaterial);

        // updates class material
        void UpdateClassMaterial(long classId, ClassMaterial material);

        // sets class material to unlock
        void SetClassMaterialToUnlock(long classMaterialId);

        // sets class material to lock
        void SetClassMaterialToLock(long clsClassMaterialId);

        #endregion

        #region class material file

        // gets ClassMaterialFile by classMaterialId and languageId
        List<ClassMaterialFile> GetClassMaterialFileByClassMaterialAndLanguage(long classMaterialId, LanguageEnum languageId,
            ClassMaterialFileIncludes includes = ClassMaterialFileIncludes.None);

        // gets ClassMaterialFile by id
        ClassMaterialFile GetClassMaterialFileById(long classMaterialFileId, ClassMaterialFileIncludes includes = ClassMaterialFileIncludes.None);

        // inserts ClassMaterialFile
        long InsertClassMateriaFile(ClassMaterialFile materialFile);

        // updates ClassMaterialFile
        void UpdateClassMaterialFile(ClassMaterialFile materialFile, bool updateFileId);

        // deletes ClassMaterialFile
        void DeleteClassMaterialFile(long classMaterialFileId);

        #endregion

        #region class material recipient

        // gets ClassMaterialRecipient by id
        ClassMaterialRecipient GetClassMaterialRecipientById(
            long classMaterialRecipientId,
            ClassMaterialRecipientIncludes includes = ClassMaterialRecipientIncludes.None);

        // gets ClassMaterialRecipient by requestCode
        ClassMaterialRecipient GetClassMaterialRecipientByRequestCode(
            string requestCode,
            ClassMaterialRecipientIncludes includes = ClassMaterialRecipientIncludes.None);

        // gets ClassMaterialRecipient by recipient id and class id
        ClassMaterialRecipient GetClassMaterialRecipientByRecipientIdAndClassId(
            RecipientId recipientId,
            long classId,
            ClassMaterialRecipientIncludes includes = ClassMaterialRecipientIncludes.None);

        // updates ClassMaterialRecipient
        void UpdateClassMaterialRecipient(ClassMaterialRecipient materialRecipient);

        // inserts ClassMaterialRecipient
        long InsertClassMaterialRecipient(ClassMaterialRecipient materialRecipient);

        // inserts ClassMaterialRecipients
        void InsertClassMaterialRecipients(List<ClassMaterialRecipient> materialRecipients);

        // sets ClassMaterialRecipient language
        void SetClassMaterialRecipientLanguage(string requestCode, LanguageEnum languageId);

        // set last notification of class registration mateirals
        void SetClassMaterialRecipientNotified(IEnumerable<RecipientId> recipientIds, long classMaterialId, DateTime notified);

        // sets IsLock on class reigstration material
        void SetClassMaterialRecipientIsLocked(long materialRecipientId, bool isLocked);

        #endregion

        #region clas material download log
        
        // inserts ClassMaterialDownloadLog
        long InsertClassMaterialDownloadLog(ClassMaterialDownloadLog log);

        #endregion
    }
}
