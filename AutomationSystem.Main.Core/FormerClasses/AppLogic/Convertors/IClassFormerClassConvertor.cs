using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic.Convertors
{
    /// <summary>
    /// Converts class and registration to former class and student
    /// </summary>
    public interface IClassFormerClassConvertor
    {

        // converts Class to Former class
        FormerClass ConvertToFormerClass(Class cls, int ownerId);

        // converts Class registration to Former student
        FormerStudent ConvertToFormerStudent(ClassRegistration registration, int ownerId);

    }

}
