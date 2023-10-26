using System.Linq;
using AutomationSystem.Main.Core.MaterialDistribution.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;

namespace AutomationSystem.Main.Core.MaterialDistribution.Data.Extensions
{
    public static class ClassMaterialRemoveInactive
    {
        // removes inactive includes for ClassMaterial
        public static ClassMaterial RemoveInactiveForClassMaterial(ClassMaterial entity, ClassMaterialIncludes includes)
        {
            if (entity == null)
            {
                return null;
            }

            if (includes.HasFlag(ClassMaterialIncludes.ClassClassPersons))
            {
                entity.Class.ClassPersons = entity.Class.ClassPersons.AsQueryable().Active().ToList();
            }

            if (includes.HasFlag(ClassMaterialIncludes.ClassMaterialFiles))
            {
                entity.ClassMaterialFiles = entity.ClassMaterialFiles.AsQueryable().Active().ToList();
            }

            if (includes.HasFlag(ClassMaterialIncludes.ClassMaterialRecipients)
                || includes.HasFlag(ClassMaterialIncludes.ClassMaterialRecipientsClassMaterialDownloadLogs))
            {
                entity.ClassMaterialRecipients = entity.ClassMaterialRecipients.AsQueryable().Active().ToList();
            }

            if (includes.HasFlag(ClassMaterialIncludes.ClassMaterialRecipientsClassMaterialDownloadLogs))
            {
                foreach(var recipient in entity.ClassMaterialRecipients)
                {
                    recipient.ClassMaterialDownloadLogs = recipient.ClassMaterialDownloadLogs.AsQueryable().Active().ToList();
                }
            }

            return entity;
        }

        // removes inactive includes for ClassMaterialRecipient
        public static ClassMaterialRecipient RemoveInactiveForClassMaterialRecipient(ClassMaterialRecipient entity, ClassMaterialRecipientIncludes includes)
        {
            if (entity == null)
            {
                return null;
            }

            if (includes.HasFlag(ClassMaterialRecipientIncludes.ClassMaterialClassClassPersons))
            {
                entity.ClassMaterial.Class.ClassPersons = entity.ClassMaterial.Class.ClassPersons.AsQueryable().Active().ToList();
            }

            if (includes.HasFlag(ClassMaterialRecipientIncludes.ClassMaterialClassMaterialFiles))
            {
                entity.ClassMaterial.ClassMaterialFiles = entity.ClassMaterial.ClassMaterialFiles.AsQueryable().Active().ToList();
            }

            if (includes.HasFlag(ClassMaterialRecipientIncludes.ClassMaterialDownloadLogs))
            {
                entity.ClassMaterialDownloadLogs = entity.ClassMaterialDownloadLogs.AsQueryable().Active().ToList();
            }

            return entity;
        }
    }
}
