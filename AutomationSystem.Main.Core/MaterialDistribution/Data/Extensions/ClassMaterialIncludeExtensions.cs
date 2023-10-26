using System.Data.Entity.Infrastructure;
using AutomationSystem.Main.Core.MaterialDistribution.Data.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.MaterialDistribution.Data.Extensions
{
    /// <summary>
    /// Aggregates class material include extensions
    /// </summary>
    public static class ClassMaterialIncludeExtensions
    {
        // adds includes for ClassMaterial
        public static DbQuery<ClassMaterial> AddIncludes(this DbQuery<ClassMaterial> query, ClassMaterialIncludes includes)
        {
            if (includes.HasFlag(ClassMaterialIncludes.Class))
            {
                query = query.Include("Class");
            }

            if (includes.HasFlag(ClassMaterialIncludes.ClassClassType))
            {
                query = query.Include("Class.ClassType");
            }

            if (includes.HasFlag(ClassMaterialIncludes.ClassClassPersons))
            {
                query = query.Include("Class.ClassPersons");
            }

            if (includes.HasFlag(ClassMaterialIncludes.ClassMaterialFiles))
            {
                query = query.Include("ClassMaterialFiles");
            }

            if (includes.HasFlag(ClassMaterialIncludes.ClassMaterialRecipients))
            {
                query = query.Include("ClassMaterialRecipients");
            }

            if (includes.HasFlag(ClassMaterialIncludes.ClassMaterialRecipientsClassMaterialDownloadLogs))
            {
                query = query.Include("ClassMaterialRecipients.ClassMaterialDownloadLogs");
            }
            
            return query;
        }

        // adds includes for ClassMaterialRecipient
        public static DbQuery<ClassMaterialRecipient> AddIncludes(this DbQuery<ClassMaterialRecipient> query, ClassMaterialRecipientIncludes includes)
        {
            if (includes.HasFlag(ClassMaterialRecipientIncludes.ClassMaterial))
            {
                query = query.Include("ClassMaterial");
            }

            if (includes.HasFlag(ClassMaterialRecipientIncludes.ClassMaterialClass))
            {
                query = query.Include("ClassMaterial.Class");
            }

            if (includes.HasFlag(ClassMaterialRecipientIncludes.ClassMaterialClassClassType))
            {
                query = query.Include("ClassMaterial.Class.ClassType");
            }

            if (includes.HasFlag(ClassMaterialRecipientIncludes.ClassMaterialClassClassPersons))
            {
                query = query.Include("ClassMaterial.Class.ClassPersons");
            }

            if (includes.HasFlag(ClassMaterialRecipientIncludes.ClassMaterialClassMaterialFiles))
            {
                query = query.Include("ClassMaterial.ClassMaterialFiles");
            }

            if (includes.HasFlag(ClassMaterialRecipientIncludes.ClassMaterialDownloadLogs))
            {
                query = query.Include("ClassMaterialDownloadLogs");
            }
                
            return query;
        }

        // adds includes for ClassMaterialFile
        public static DbQuery<ClassMaterialFile> AddIncludes(this DbQuery<ClassMaterialFile> query, ClassMaterialFileIncludes includes)
        {
            if (includes.HasFlag(ClassMaterialFileIncludes.ClassMaterial))
            {
                query = query.Include("ClassMaterial");
            }

            if (includes.HasFlag(ClassMaterialFileIncludes.ClassMaterialClass))
            {
                query = query.Include("ClassMaterial.Class");
            }

            return query;
        }
    }
}