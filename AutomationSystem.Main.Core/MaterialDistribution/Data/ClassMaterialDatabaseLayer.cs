using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Core.MaterialDistribution.Data.Extensions;
using AutomationSystem.Main.Core.MaterialDistribution.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;

namespace AutomationSystem.Main.Core.MaterialDistribution.Data
{
    /// <summary>
    /// Provides database layer for class materials
    /// </summary>
    public class ClassMaterialDatabaseLayer : IClassMaterialDatabaseLayer
    {
        #region class materials 

        // gets class material by id
        public ClassMaterial GetClassMaterialById(long classMaterialId, ClassMaterialIncludes includes = ClassMaterialIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassMaterials.AddIncludes(includes).ActiveInActiveClass().FirstOrDefault(x => x.ClassMaterialId == classMaterialId);
                result = ClassMaterialRemoveInactive.RemoveInactiveForClassMaterial(result, includes);
                return result;
            }
        }

        // gets class material by class id
        public ClassMaterial GetClassMaterialByClassId(long classId, ClassMaterialIncludes includes = ClassMaterialIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassMaterials.AddIncludes(includes).ActiveInActiveClass().FirstOrDefault(x => x.ClassId == classId);
                result = ClassMaterialRemoveInactive.RemoveInactiveForClassMaterial(result, includes);
                return result;
            }
        }

        // inserts class material
        public long InsertClassMaterial(ClassMaterial classMaterial)
        {
            using (var context = new MainEntities())
            {
                context.ClassMaterials.Add(classMaterial);
                context.SaveChanges();
                return classMaterial.ClassMaterialId;
            }
        }

        // updates class material
        public void UpdateClassMaterial(long classId, ClassMaterial material)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.ClassMaterials.ActiveInActiveClass().FirstOrDefault(x => x.ClassId == classId);
                if (toUpdate == null)
                {
                    throw new ArgumentException($"There is no Class materials related to class with Id {classId}.");
                }

                toUpdate.CoordinatorPassword = material.CoordinatorPassword;
                toUpdate.AutomationLockTime = material.AutomationLockTime;
                toUpdate.AutomationLockTimeUtc = material.AutomationLockTimeUtc;

                context.SaveChanges();
            }
        }

        // sets class material to unlock
        public void SetClassMaterialToUnlock(long classMaterialId)
        {
            using (var context = new MainEntities())
            {
                var material = context.ClassMaterials.ActiveInActiveClass().FirstOrDefault(x => x.ClassMaterialId == classMaterialId);
                if (material == null)
                {
                    throw new ArgumentException($"There is no ClassMaterial with id {classMaterialId}.");
                }

                material.IsLocked = false;
                material.Locked = null;
                material.IsUnlocked = true;
                material.Unlocked = DateTime.Now;

                context.SaveChanges();
            }
        }

        // sets class material to lock
        public void SetClassMaterialToLock(long classMaterialId)
        {
            using (var context = new MainEntities())
            {
                var material = context.ClassMaterials.ActiveInActiveClass().FirstOrDefault(x => x.ClassMaterialId == classMaterialId);
                if (material == null)
                {
                    throw new ArgumentException($"There is no ClassMaterial with id {classMaterialId}.");
                }

                material.IsUnlocked = false;
                material.Unlocked = null;
                material.IsLocked = true;
                material.Locked = DateTime.Now;

                context.SaveChanges();
            }
        }

        #endregion

        #region class material file

        // gets ClassMaterialFile by classMaterialId and languageId
        public List<ClassMaterialFile> GetClassMaterialFileByClassMaterialAndLanguage(long classMaterialId, LanguageEnum languageId,
            ClassMaterialFileIncludes includes = ClassMaterialFileIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassMaterialFiles.AddIncludes(includes).Active()
                    .Where(x => x.ClassMaterialId == classMaterialId && x.LanguageId == languageId).ToList();
                return result;
            }
        }

        // gets ClassMaterialFile by id
        public ClassMaterialFile GetClassMaterialFileById(long classMaterialFileId,
            ClassMaterialFileIncludes includes = ClassMaterialFileIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassMaterialFiles.AddIncludes(includes).Active()
                    .FirstOrDefault(x => x.ClassMaterialFileId == classMaterialFileId);
                return result;
            }
        }

        // inserts ClassMaterialFile
        public long InsertClassMateriaFile(ClassMaterialFile materialFile)
        {
            using (var context = new MainEntities())
            {
                context.ClassMaterialFiles.Add(materialFile);
                context.SaveChanges();
                return materialFile.ClassMaterialFileId;
            }
        }

        // updates ClassMaterialFile
        public void UpdateClassMaterialFile(ClassMaterialFile materialFile, bool updateFileId)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.ClassMaterialFiles.Active().FirstOrDefault(x => x.ClassMaterialFileId == materialFile.ClassMaterialFileId);
                if (toUpdate == null)
                {
                    throw new ArgumentException($"There is no ClassMaterialFile with id {materialFile.ClassMaterialFileId}.");
                }

                toUpdate.DisplayName = materialFile.DisplayName;
                toUpdate.LanguageId = materialFile.LanguageId;
                if (updateFileId)
                {
                    toUpdate.FileId = materialFile.FileId;
                }

                context.SaveChanges();
            }
        }

        // deletes ClassMaterialFile
        public void DeleteClassMaterialFile(long classMaterialFileId)
        {
            using (var context = new MainEntities())
            {
                var toDelete = context.ClassMaterialFiles.Active().FirstOrDefault(x => x.ClassMaterialFileId == classMaterialFileId);
                if (toDelete == null)
                {
                    return;
                }

                context.ClassMaterialFiles.Remove(toDelete);
                context.SaveChanges();
            }
        }

        #endregion

        #region class material recipient

        // gets ClassMaterialRecipient by id
        public ClassMaterialRecipient GetClassMaterialRecipientById(
            long classMaterialRecipientId,
            ClassMaterialRecipientIncludes includes = ClassMaterialRecipientIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassMaterialRecipients.AddIncludes(includes).Active()
                    .FirstOrDefault(x => x.ClassMaterialRecipientId == classMaterialRecipientId);
                result = ClassMaterialRemoveInactive.RemoveInactiveForClassMaterialRecipient(result, includes);
                return result;
            }
        }

        // gets ClassMaterialRecipient by requestCode
        public ClassMaterialRecipient GetClassMaterialRecipientByRequestCode(
            string requestCode,
            ClassMaterialRecipientIncludes includes = ClassMaterialRecipientIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassMaterialRecipients.AddIncludes(includes).Active().FirstOrDefault(x => x.RequestCode == requestCode);
                result = ClassMaterialRemoveInactive.RemoveInactiveForClassMaterialRecipient(result, includes);
                return result;
            }
        }

        // gets ClassMaterialRecipient by recipient id and class id
        public ClassMaterialRecipient GetClassMaterialRecipientByRecipientIdAndClassId(
            RecipientId recipientId,
            long classId,
            ClassMaterialRecipientIncludes includes = ClassMaterialRecipientIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassMaterialRecipients
                    .AddIncludes(includes).Active()
                    .FirstOrDefault(x => x.ClassMaterial.ClassId == classId
                                         && x.RecipientId == recipientId.Id
                                         && x.RecipientTypeId == recipientId.TypeId);

                result = ClassMaterialRemoveInactive.RemoveInactiveForClassMaterialRecipient(result, includes);
                return result;
            }
        }

        // updates ClassMaterialRecipient
        public void UpdateClassMaterialRecipient(ClassMaterialRecipient materialRecipient)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.ClassMaterialRecipients.Active()
                    .FirstOrDefault(x => x.ClassMaterialRecipientId == materialRecipient.ClassMaterialRecipientId);
                if (toUpdate == null)
                {
                    throw new ArgumentException($"There is no ClassMaterialRecipient with id {materialRecipient.ClassMaterialRecipientId}.");
                }

                toUpdate.Password = materialRecipient.Password;
                toUpdate.LanguageId = materialRecipient.LanguageId;
                toUpdate.DownloadLimit = materialRecipient.DownloadLimit;

                context.SaveChanges();
            }
        }

        // inserts ClassMaterialRecipient
        public long InsertClassMaterialRecipient(ClassMaterialRecipient materialRecipient)
        {
            using (var context = new MainEntities())
            {
                context.ClassMaterialRecipients.Add(materialRecipient);
                context.SaveChanges();

                return materialRecipient.ClassMaterialRecipientId;
            }
        }

        // inserts ClassMaterialRecipients
        public void InsertClassMaterialRecipients(List<ClassMaterialRecipient> materialRecipients)
        {
            using (var context = new MainEntities())
            {
                context.ClassMaterialRecipients.AddRange(materialRecipients);
                context.SaveChanges();
            }
        }

        // sets ClassMaterialRecipient language
        public void SetClassMaterialRecipientLanguage(string requestCode, LanguageEnum languageId)
        {
            using (var context = new MainEntities())
            {
                var registrationMaterial = context.ClassMaterialRecipients.Active().FirstOrDefault(x => x.RequestCode == requestCode);
                if (registrationMaterial == null)
                {
                    throw new ArgumentException($"There is no ClassMaterialRecipients with RequestCode {requestCode}.");
                }

                registrationMaterial.LanguageId = languageId;
                context.SaveChanges();
            }
        }

        // set last notification of class registration mateirals
        public void SetClassMaterialRecipientNotified(IEnumerable<RecipientId> recipientIds, long classMaterialId, DateTime notified)
        {
            using (var context = new MainEntities())
            {
                var materialRecipients = new List<ClassMaterialRecipient>();
                foreach (var groupedRecipientIds in recipientIds.GroupBy(x => x.TypeId))
                {
                    var recipientIdsForType = groupedRecipientIds.Select(x => x.Id).ToList();
                    materialRecipients.AddRange(
                        context.ClassMaterialRecipients.Active()
                            .Where(x => x.ClassMaterialId == classMaterialId
                                        && x.RecipientTypeId == groupedRecipientIds.Key
                                        && recipientIdsForType.Contains(x.RecipientId)));
                }

                foreach (var materialRecipient in materialRecipients)
                {
                    materialRecipient.Notified = notified;
                }

                context.SaveChanges();
            }
        }

        // sets IsLock on class reigstration material
        public void SetClassMaterialRecipientIsLocked(long materialRecipientId, bool isLocked)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.ClassMaterialRecipients.Active()
                    .FirstOrDefault(x => x.ClassMaterialRecipientId == materialRecipientId);
                if (toUpdate == null)
                {
                    throw new ArgumentException($"There is no ClassMaterialRecipients with id {materialRecipientId}.");
                }

                toUpdate.IsLocked = isLocked;
                toUpdate.Locked = isLocked ? (DateTime?) DateTime.Now : null;

                context.SaveChanges();
            }
        }

        #endregion

        #region class material download log

        // inserts ClassMaterialDownloadLog
        public long InsertClassMaterialDownloadLog(ClassMaterialDownloadLog log)
        {
            using (var context = new MainEntities())
            {
                context.ClassMaterialDownloadLogs.Add(log);
                context.SaveChanges();
                return log.ClassMaterialDownloadLogId;
            }
        }

        #endregion
    }
}
