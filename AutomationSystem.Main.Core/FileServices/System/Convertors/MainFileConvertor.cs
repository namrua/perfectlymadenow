using System;
using AutomationSystem.Main.Contract.Files.AppLogic.Models;
using AutomationSystem.Main.Core.FileServices.System.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.FileServices.System.Convertors
{

    /// <summary>
    /// Main file entities convertor
    /// </summary>
    public class MainFileConvertor : IMainFileConvertor
    {      

        // converts to registration file to EntityFileDetail
        public EntityFileDetail ConvertToEntityFileDetail(ClassRegistrationFile registrationFile)
        {
            var result = new EntityFileDetail
            {
                Id = registrationFile.ClassRegistrationFileId,              
                DisplayedName = registrationFile.DisplayedName,
                Code = registrationFile.Code,
                Assigned = registrationFile.Assigned,
                FileId = registrationFile.FileId
            };
            return result;
        }

        // converts to class file to EntityFileDetail
        public EntityFileDetail ConvertToEntityFileDetail(ClassFile classFile)
        {
            var result = new EntityFileDetail
            {
                Id = classFile.ClassFileId,
                DisplayedName = classFile.DisplayedName,
                Code = classFile.Code,
                Assigned = classFile.Assigned,
                FileId = classFile.FileId
            };
            return result;
        }

        // creates class registration file
        public ClassRegistrationFile CreateClassRegistrationFile(EntityFileToSave fileToSave, ClassRegistrationFile dbFile)
        {
            var result = new ClassRegistrationFile
            {
                ClassRegistrationFileId = dbFile?.ClassRegistrationFileId ?? 0,
                ClassRegistrationId = fileToSave.EntityId,
                Code = fileToSave.Code,
                DisplayedName = fileToSave.DisplayedName,
                Assigned = DateTime.Now,                  
                FileId = dbFile?.FileId ?? 0
            };
            return result;
        }

        // creates class file
        public ClassFile CreateClassFile(EntityFileToSave fileToSave, ClassFile dbFile)
        {
            var result = new ClassFile
            {
                ClassFileId = dbFile?.ClassFileId ?? 0,
                ClassId = fileToSave.EntityId,
                Code = fileToSave.Code,
                DisplayedName = fileToSave.DisplayedName,
                Assigned = DateTime.Now,
                FileId = dbFile?.FileId ?? 0
            };
            return result;
        }

    }

}
