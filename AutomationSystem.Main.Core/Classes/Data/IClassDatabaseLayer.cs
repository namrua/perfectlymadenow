using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.Data
{
    /// <summary>
    /// Provides database layer for classes
    /// </summary>
    public interface IClassDatabaseLayer
    {
        #region class

        List<ClassCategory> GetClassCategoriesByIds(IEnumerable<ClassCategoryEnum> classCategoryIds);

        // gets classes by filter
        List<Class> GetClassesByFilter(ClassFilter filter = null, ClassIncludes includes = ClassIncludes.None);

        // gets classes by ids
        List<Class> GetClassesByIds(IEnumerable<long> classIds, ClassIncludes includes = ClassIncludes.None);

        // get class by id
        Class GetClassById(long classId, ClassIncludes includes = ClassIncludes.None);

        // gets count of approved registrations for class Id
        int GetApprovedRegistrationCountByClassId(long classId);

        bool PayPalKeyOnAnyClass(long payPalKeyId);

        bool PersonOnAnyClass(long personId);

        bool PriceListOnAnyClass(long priceListId);

        // checks if class is link to profile
        bool AnyClassOnProfile(long profileId);

        // inserts class
        long InsertClass(Class cls);

        // updates class
        void UpdateClass(Class cls, bool isFullyEditable);

        // deletes class
        void DeleteClass(long classId, ClassOperationOption options = ClassOperationOption.None);

        // sets class as canceled
        void SetClassAsCanceled(long classId, ClassOperationOption options = ClassOperationOption.None);

        // sets class as finished
        void SetClassAsFinished(long classId, ClassOperationOption options = ClassOperationOption.None);

        #endregion


        #region class persons

        // gets class persons by classId
        List<ClassPerson> GetClassPersonsByClassId(long classId);

        // gets class persons by class id and list of roles
        List<ClassPerson> GetClassPersonsByClassIdAndRoles(long classId, params PersonRoleTypeEnum[] roles);

        #endregion


        #region class actions

        // gets class action types
        List<ClassActionType> GetClassActionTypes();

        // gets class actions by class id
        List<ClassAction> GetClassActions(long classId, ClassActionIncludes includes = ClassActionIncludes.None);

        // get class action by id
        ClassAction GetClassActionById(long classActionId, ClassActionIncludes includes = ClassActionIncludes.None);


        // insert class action
        long InsertClassAction(ClassAction classAction);

        // set as processed
        // CheckOperation - checks that IsProcessed == false
        void SetClassActionAsProcessed(long classActionId, ClassOperationOption options = ClassOperationOption.None);

        // deletes class action, returns class id
        // CheckOperation - checks that IsProcessed == false, cannot delete processed operation
        long? DeleteClassAction(long classActionId, ClassOperationOption options = ClassOperationOption.None);

        #endregion


        #region class registration files

        // get list of class files by class id
        List<ClassFile> GetClassFilesByClassId(long classId);

        // get list of class files by ids
        List<ClassFile> GetClassFilesByIds(IEnumerable<long> classFileIds);

        // gets class file by name
        ClassFile GetClassFileByCode(long classId, string code);

        // insert class file
        long InsertClassFile(ClassFile classFile);

        // update class file
        void UpdateClassFile(ClassFile classFile);

        #endregion


        #region class report settings

        // updates ClassReportSetting of the class [no options]
        void UpdateClassReportSettingByClassId(long classId, ClassReportSetting setting, ClassOperationOption options = ClassOperationOption.None);

        #endregion


        #region class business

        // gets class expense types
        List<ClassExpenseType> GetClassExpenseTypes();

        // updates ClassBusiness of the class [no options]
        void UpdateClassBusinessByClassId(long classId, ClassBusiness business, 
            bool updateOnlyCustomExpenses, ClassOperationOption options = ClassOperationOption.None);

        // updates Class's expenses [no options]
        void UpdateClassExpenses(long classId, List<ClassExpense> classExpenses, ClassOperationOption options = ClassOperationOption.None);

        #endregion


        #region class style

        // gets registration color scheme types
        List<RegistrationColorScheme> GetRegistrationColorSchemes();

        // updates ClassStyle of the class [no options]
        void UpdateClassStyle(long classId, ClassStyle style, bool updateHeaderPictureId, ClassOperationOption options = ClassOperationOption.None);

        #endregion
    }
}
