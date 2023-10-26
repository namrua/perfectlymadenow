using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic.Convertors
{

    /// <summary>
    /// Converts former student objects
    /// </summary>
    public interface IFormerStudentConvertor
    {
        // initializes FormerStudentForEdit
        FormerStudentForEdit InitializeFormerStudentForEdit();

        // converts former student to former student detail
        FormerStudentDetail ConvertToFormerStudentDetail(FormerStudent formerStudent, bool includeFormerClass = false);

        // converts former student to fs form
        FormerStudentForm ConvertToFormerStudentForm(FormerStudent formerStudent);

        // converts form former student to db former student
        FormerStudent ConvertToFormerStudent(FormerStudentForm form, bool isTemporary);

        FormerStudentListItem ConvertToFormerStudentListItem(FormerStudent formerStudent, bool includeFormerClass = false);

    }

}
