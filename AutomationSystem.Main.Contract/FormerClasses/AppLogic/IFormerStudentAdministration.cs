using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic
{


    /// <summary>
    /// Service for former students and classes administration
    /// </summary>
    public interface IFormerStudentAdministration
    {
        FormerStudentsForList GetFormerStudentsForList(FormerStudentFilter filter, bool search);
        
        FormerStudentDetail GetFormerStudentById(long formerStudentId);

        
        FormerStudentForEdit GetNewFormerStudentForEdit(long formerClassId);
        
        FormerStudentForEdit GetFormerStudentForEdit(long formerStudentId);
        
        FormerStudentForEdit GetFormFormerStudentForEdit(FormerStudentForm form);

        
        long SaveFormerStudent(FormerStudentForm form);
        
        void DeleteFormerStudent(long formerStudentId);
        

    }

}
