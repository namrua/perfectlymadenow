using System.Collections.Generic;
using AutomationSystem.Main.Contract.Files.AppLogic.Models;
using AutomationSystem.Main.Core.FileServices.System.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Files.System.Models;

namespace AutomationSystem.Main.Core.FileServices.System
{
    /// <summary>
    /// Main file service
    /// </summary>
    public interface IMainFileService
    {

        // gets file stored in the system
        FileForDownload GetStoredFile(string rootPath, MainStoredFile storedFile);


        // gets EntityFileDetail from ClassRegistrationFile
        EntityFileDetail ConvertToEntityFileDetail(ClassRegistrationFile registrationFile);


        // gets class registration file details 
        List<EntityFileDetail> GetClassRegistrationFileDetails(long registrationId);

        // gets class file details 
        List<EntityFileDetail> GetClassFileDetails(long classId);

        // saves class registration file
        long SaveClassRegistrationFile(EntityFileToSave fileToSave);

        // saves class file
        long SaveClassFile(EntityFileToSave fileToSave);

        // gets file IDs from listed class registration files
        List<long> GetFileIdsByClassRegistrationFileIds(IEnumerable<long> classRegistrationFileIds);

        // gets file IDs from listed class files
        List<long> GetFileIdsByClassFileIds(IEnumerable<long> classFileIds);

    }

}
