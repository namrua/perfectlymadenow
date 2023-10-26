using AutomationSystem.Main.Contract.Files.AppLogic.Models;
using AutomationSystem.Main.Core.FileServices.System.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.FileServices.System.Convertors
{
    /// <summary>
    /// Main file entities convertor
    /// </summary>
    public interface IMainFileConvertor
    {       

        // converts to registration file to EntityFileDetail
        EntityFileDetail ConvertToEntityFileDetail(ClassRegistrationFile registrationFile);

        // converts to class file to EntityFileDetail
        EntityFileDetail ConvertToEntityFileDetail(ClassFile classFile);

        // creates class registration file
        ClassRegistrationFile CreateClassRegistrationFile(EntityFileToSave fileToSave, ClassRegistrationFile dbFile);

        // creates class file
        ClassFile CreateClassFile(EntityFileToSave fileToSave, ClassFile dbFile);

    }

}
