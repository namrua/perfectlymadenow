using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic
{
    public interface IFormerClassAdministration
    {
        FormerClassesForList GetFormerClassesForList(FormerClassFilter filter, bool search);
        
        FormerClassDetail GetFormerClassById(long formerClassId);
        
        FormerClassStudents GetFormerClassStudents(long formerClassId);

        
        FormerClassForEdit GetNewFormerClassForEdit();
        
        FormerClassForEdit GetFormerClassForEdit(long formerClassId);
        
        FormerClassForEdit GetFormFormerClassForEdit(FormerClassForm form);
        
        bool ValidateFormerClassForm(FormerClassForm form);

        
        long SaveFormerClass(FormerClassForm form);
        
        void DeleteFormerClass(long formerClassId);
    }
}
