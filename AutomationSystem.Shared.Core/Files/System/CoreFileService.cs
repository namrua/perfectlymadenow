using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Files.System;
using AutomationSystem.Shared.Contract.Files.System.Models;
using AutomationSystem.Shared.Core.Files.Data;
using File = AutomationSystem.Shared.Model.File;

namespace AutomationSystem.Shared.Core.Files.System
{
    /// <summary>
    /// Provides core file services
    /// </summary>
    public class CoreFileService : ICoreFileService
    {

        private readonly IFileDatabaseLayer fileDb;
        private readonly Dictionary<FileTypeEnum, string> mimeTypeMap;           


        // constructor
        public CoreFileService(IFileDatabaseLayer fileDb)
        {
            this.fileDb = fileDb;
            mimeTypeMap = new Dictionary<FileTypeEnum, string>
            {
                { FileTypeEnum.Generic, FileMimeType.Generic },
                { FileTypeEnum.Excel, FileMimeType.Excel },
                { FileTypeEnum.Word, FileMimeType.Word },
                { FileTypeEnum.Csv, FileMimeType.Csv },
                { FileTypeEnum.Jpg, FileMimeType.Jpg },
                { FileTypeEnum.Png, FileMimeType.Png },
                { FileTypeEnum.Gif, FileMimeType.Gif },
                { FileTypeEnum.Pdf, FileMimeType.Pdf }
            };
        }


        // maps File type to mime type
        public string GetMimeTypeByFileType(FileTypeEnum fileTypeId)
        {
            if (!mimeTypeMap.TryGetValue(fileTypeId, out var result))
                throw new ArgumentException($"There is no mapping of File type {fileTypeId} to mime type.");
            return result;
        }


        // gets file for download from app data
        public FileForDownload GetFileForDownloadFromAppData(string rootPath, AppDataStoredFile file)
        {
            var fullPath = Path.Combine(rootPath, file.Path, file.FileName);
            var content = global::System.IO.File.ReadAllBytes(fullPath);

            var result = new FileForDownload
            {
                FileName = file.FileName,
                Content = content,
                MimeType = file.FileMimeType
            };
            return result;
        }


        // gets file for download by file id
        public FileForDownload GetFileForDownloadById(long fileId)
        {
            var file = fileDb.GetFileById(fileId);
            if (file == null)
                throw new ArgumentException($"There is no File with id {fileId}.");

            var result = ConvertToFileForDownload(file);            
            return result;
        }


        // gets file for public download by file id
        public FileForDownload GetFileForPublicDownloadById(long fileId)
        {
            var file = fileDb.GetFileById(fileId);
            if (file == null)
                throw new ArgumentException($"There is no File with id {fileId}.");

            if (!file.IsPublic)
                throw new SecurityException($"Unauthorized access to file with id {fileId}");

            var result = ConvertToFileForDownload(file);
            return result;
        }


        // gets file for download by file ids
        public List<FileForDownload> GetFileForDownloadByIds(IEnumerable<long> fileIds)
        {
            var files = fileDb.GetFilesByIds(fileIds);
            var result = files.Select(ConvertToFileForDownload).ToList();
            return result;
        }


        // inserts file into database, returns fileId
        public long InsertFile(byte[] content, string name, string fileName, FileTypeEnum fileTypeId, LanguageEnum? languageId = null, bool isPublic = false)
        {
            var file = new File
            {
                Name = name,
                FileName = fileName,
                FileTypeId = fileTypeId,
                LanguageId = languageId,                
                Data = content,
                IsPublic = isPublic
            };
            var result = fileDb.InsertFile(file);
            return result;
        }

        // inserts file stream into database, returns fileId
        public long InsertFile(Stream content, string name, string fileName, FileTypeEnum fileTypeId,
            LanguageEnum? languageId = null, bool isPublic = false)
        {
            var byteContent = ConvertStreamToByteArray(content);
            var result = InsertFile(byteContent, name, fileName, fileTypeId, languageId, isPublic);
            return result;
        }


        // updates file content
        public void UpdateFileContent(long fileId, byte[] content, string name = null, string fileName = null)
        {
            fileDb.UpdateFileContent(fileId, content, name, fileName);
        }


        // clones existing file
        public long CloneFile(long fileId, string name)
        {
            var result = fileDb.CloneFile(fileId, name);
            return result;
        }

        // delete file
        public void DeleteFile(long fileId)
        {
            fileDb.DeleteFile(fileId);
        }

        // converts Stream to byte[]
        public byte[] ConvertStreamToByteArray(Stream stream, bool seekToBegin = false)
        {
            if (!(stream is MemoryStream ms))
            {
                ms = new MemoryStream();
                stream.CopyTo(ms);
                if (seekToBegin)
                    stream.Seek(0, SeekOrigin.Begin);
            }
            var result = ms.ToArray();
            return result;
        }

        #region private methods

        // converts file to file to download
        private FileForDownload ConvertToFileForDownload(File file)
        {
            var result = new FileForDownload
            {
                FileName = file.FileName,
                Content = file.Data,
                MimeType = GetMimeTypeByFileType(file.FileTypeId)
            };
            return result;
        }

        #endregion

    }

}
