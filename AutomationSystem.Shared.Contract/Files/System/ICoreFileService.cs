using System.Collections.Generic;
using System.IO;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Files.System.Models;

namespace AutomationSystem.Shared.Contract.Files.System
{
    /// <summary>
    /// Provides core file services
    /// </summary>
    public interface ICoreFileService
    {
        
        // maps File type to mime type
        string GetMimeTypeByFileType(FileTypeEnum fileTypeId);

        // gets file for download from app data
        FileForDownload GetFileForDownloadFromAppData(string rootPath, AppDataStoredFile file);

        // gets file for download by file id
        FileForDownload GetFileForDownloadById(long fileId);

        // gets file for public download by file id
        FileForDownload GetFileForPublicDownloadById(long fileId);

        // gets file for download by file ids
        List<FileForDownload> GetFileForDownloadByIds(IEnumerable<long> fileIds);

        // inserts file into database, returns fileId
        long InsertFile(byte[] content, string name, string fileName, FileTypeEnum fileTypeId, LanguageEnum? languageId = null, bool isPublic = false);

        // inserts file stream into database, returns fileId
        long InsertFile(Stream content, string name, string fileName, FileTypeEnum fileTypeId, LanguageEnum? languageId = null, bool isPublic = false);

        // updates file content
        void UpdateFileContent(long fileId, byte[] content, string name = null, string fileName = null);

        // clones existing file
        long CloneFile(long fileId, string name);

        // delete file
        void DeleteFile(long fileId);

        // converts Stream to byte[]
        byte[] ConvertStreamToByteArray(Stream stream, bool seekToBegin = false);
    }

}
