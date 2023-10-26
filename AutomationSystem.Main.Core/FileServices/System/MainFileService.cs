using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Files.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.FileServices.System.Convertors;
using AutomationSystem.Main.Core.FileServices.System.Models;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Files.System;
using AutomationSystem.Shared.Contract.Files.System.Models;

namespace AutomationSystem.Main.Core.FileServices.System
{


    /// <summary>
    /// Main file service
    /// </summary>
    public class MainFileService : IMainFileService
    {

        public const string StoredFilesPath = "StoredFiles";

        // private components
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IClassDatabaseLayer classDb;
        private readonly ICoreFileService coreFileService;        
       
        private readonly IMainFileConvertor fileConvertor;
        private readonly Dictionary<MainStoredFile, AppDataStoredFile> appDataStoredFiles;
              

        // constructor
        public MainFileService(
            IRegistrationDatabaseLayer registrationDb,
            IClassDatabaseLayer classDb,
            ICoreFileService coreFileService,
            IMainFileConvertor fileConvertor)
        {
            this.registrationDb = registrationDb;
            this.classDb = classDb;
            this.coreFileService = coreFileService;

            this.fileConvertor = fileConvertor;
            appDataStoredFiles = new Dictionary<MainStoredFile, AppDataStoredFile>();
            appDataStoredFiles[MainStoredFile.FormerStudentUploadSheet] = new AppDataStoredFile(StoredFilesPath, "FormerStudentUploadSheet.xlsx", FileMimeType.Excel);
            appDataStoredFiles[MainStoredFile.StudentRegistrationUploadSheet] = new AppDataStoredFile(StoredFilesPath, "StudentRegistrationUploadSheet.xlsx", FileMimeType.Excel);
        }


        // gets file stored in the system
        public FileForDownload GetStoredFile(string rootPath, MainStoredFile storedFile)
        {
            if (!appDataStoredFiles.TryGetValue(storedFile, out var appDataStoredFile))
                throw new ArgumentException($"Unknown Stored file {storedFile}.");
            var result = coreFileService.GetFileForDownloadFromAppData(rootPath, appDataStoredFile);                       
            return result;
        }


        // gets EntityFileDetail from ClassRegistrationFile
        public EntityFileDetail ConvertToEntityFileDetail(ClassRegistrationFile registrationFile)
        {
            var result = fileConvertor.ConvertToEntityFileDetail(registrationFile);
            return result;
        }


        // gets class registration file details 
        public List<EntityFileDetail> GetClassRegistrationFileDetails(long registrationId)
        {
            var files = registrationDb.GetClassRegistrationFilesByRegistrationId(registrationId);
            var result = files.Select(fileConvertor.ConvertToEntityFileDetail).ToList();
            return result;            
        }

        // gets class file details 
        public List<EntityFileDetail> GetClassFileDetails(long classId)
        {
            var files = classDb.GetClassFilesByClassId(classId);
            var result = files.Select(fileConvertor.ConvertToEntityFileDetail).ToList();
            return result;
        }


        // saves class registration file
        public long SaveClassRegistrationFile(EntityFileToSave fileToSave)
        {
            if (fileToSave.EntityTypeId != EntityTypeEnum.MainClassRegistration)
                throw new InvalidOperationException($"Entity of file to save is not MainClassRegistration ({fileToSave.EntityTypeId}).");

            long result;
            var dbRegistrationFile = registrationDb.GetClassRegistrationFileByCode(fileToSave.EntityId, fileToSave.Code);
            var newRegistrationFile = fileConvertor.CreateClassRegistrationFile(fileToSave, dbRegistrationFile);
            if (dbRegistrationFile == null)
            {                
                newRegistrationFile.FileId = coreFileService.InsertFile(fileToSave.Content, fileToSave.Code, fileToSave.FileName, fileToSave.FileTypeId);
                result = registrationDb.InsertClassRegistrationFile(newRegistrationFile);
            }
            else
            {             
                coreFileService.UpdateFileContent(newRegistrationFile.FileId, fileToSave.Content, fileToSave.Code, fileToSave.FileName);
                registrationDb.UpdateClassRegistrationFile(newRegistrationFile);
                result = newRegistrationFile.ClassRegistrationFileId;
            }
            return result;
        }


        // saves class file
        public long SaveClassFile(EntityFileToSave fileToSave)
        {
            if (fileToSave.EntityTypeId != EntityTypeEnum.MainClass)
                throw new InvalidOperationException($"Entity of file to save is not MainClass ({fileToSave.EntityTypeId}).");

            long result;
            var dbClassFile = classDb.GetClassFileByCode(fileToSave.EntityId, fileToSave.Code);
            var newClassFile = fileConvertor.CreateClassFile(fileToSave, dbClassFile);
            if (dbClassFile == null)
            {                
                newClassFile.FileId = coreFileService.InsertFile(fileToSave.Content, fileToSave.Code, fileToSave.FileName, fileToSave.FileTypeId);
                result = classDb.InsertClassFile(newClassFile);
            }
            else
            {              
                coreFileService.UpdateFileContent(newClassFile.FileId, fileToSave.Content, fileToSave.Code, fileToSave.FileName);
                classDb.UpdateClassFile(newClassFile);
                result = newClassFile.ClassFileId;
            }
            return result;
        }


        // gets file IDs from listed class registration files
        public List<long> GetFileIdsByClassRegistrationFileIds(IEnumerable<long> classRegistrationFileIds)
        {
            var classRegistrationFiles = registrationDb.GetClassRegistrationFilesByIds(classRegistrationFileIds);
            var result = classRegistrationFiles.Select(x => x.FileId).ToList();
            return result;

        }

        // gets file IDs from listed class files
        public List<long> GetFileIdsByClassFileIds(IEnumerable<long> classFileIds)
        {
            var classFiles = classDb.GetClassFilesByIds(classFileIds);
            var result = classFiles.Select(x => x.FileId).ToList();
            return result;
        }

    }

}
