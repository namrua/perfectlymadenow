using System.Collections.Generic;
using AutomationSystem.Shared.Core.Files.Data.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Files.Data
{
    /// <summary>
    /// File database layer
    /// </summary>
    public interface IFileDatabaseLayer
    {

        // gets files by list of ids
        List<File> GetFilesById(List<long> ids, FileIncludes includes = FileIncludes.None);

        // get file by file id
        File GetFileById(long fileId, FileIncludes includes = FileIncludes.None);

        // get files by ids
        List<File> GetFilesByIds(IEnumerable<long> fileIds, FileIncludes includes = FileIncludes.None);

        // insert file
        long InsertFile(File file);

        // updates file content
        void UpdateFileContent(long fileId, byte[] content, string name = null, string fileName = null);

        // clone existing file 
        long CloneFile(long fileId, string name);

        // delete file
        void DeleteFile(long fileId);

    }

}
