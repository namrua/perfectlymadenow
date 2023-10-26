using System.Collections.Generic;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;
using AutomationSystem.Main.Core.FormerClasses.Data.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.FormerClasses.Data
{

    /// <summary>
    /// Provides former student and class database layer
    /// </summary>
    public interface IFormerDatabaseLayer
    {

        #region former classes      

        // gets former classes by filter
        List<FormerClass> GetFormerClassesByFilter(FormerClassFilter filter = null, FormerClassIncludes includes = FormerClassIncludes.None);

        // gets former class by id
        FormerClass GetFormerClassById(long formerClassId, FormerClassIncludes includes = FormerClassIncludes.None);

        // inserts former class
        long InsertFormerClass(FormerClass formerClass);

        // update former class
        void UpdateFormerClass(FormerClass formerClass, FormerOperationOption options = FormerOperationOption.None);

        // delete former class
        void DeleteFormerClass(long formerClassId, FormerOperationOption options = FormerOperationOption.None);

        #endregion


        #region former students

        // gets former students by filter
        List<FormerStudent> GetFormerStudentsByFilter(FormerStudentFilter filter = null, FormerStudentIncludes includes = FormerStudentIncludes.None);

        // gets former students by filter
        List<FormerStudent> GetFormerStudentsForReviewing(ClassRegistration registration, FormerStudentFilter formerStudentFilter, FormerStudentIncludes includes = FormerStudentIncludes.None);

        // gets former student by id
        FormerStudent GetFormerStudentById(long formerStudentId, FormerStudentIncludes includes = FormerStudentIncludes.None);

        // gets former student from filter scope by id
        FormerStudent GetFormerStudentByIdAndFilter(long formerStudentId, FormerStudentFilter formerStudentFilter, FormerStudentIncludes includes = FormerStudentIncludes.None);

        // inserts former student
        long InsertFormerStudent(FormerStudent formerStudent);

        // updates former student
        void UpdateFormerStudent(FormerStudent formerStudent, FormerOperationOption options = FormerOperationOption.None);

        // deletes former student
        void DeleteFormerStudent(long formerStudentId, FormerOperationOption options = FormerOperationOption.None);

        #endregion
    }

}
