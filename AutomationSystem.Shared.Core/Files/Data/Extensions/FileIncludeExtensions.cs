using System.Data.Entity.Infrastructure;
using AutomationSystem.Shared.Core.Files.Data.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Files.Data.Extensions
{
    /// <summary>
    /// Aggregates include extensions
    /// </summary>
    public static class FileIncludeExtensions
    {
        // adds includes for File
        public static DbQuery<File> AddIncludes(this DbQuery<File> query, FileIncludes includes)
        {
            if (includes.HasFlag(FileIncludes.FileType))
                query = query.Include("FileType");
            if (includes.HasFlag(FileIncludes.Language))
                query = query.Include("Language");
            return query;
        }
    }
}
