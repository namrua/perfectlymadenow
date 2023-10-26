using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;

namespace AutomationSystem.Main.Contract.Classes.AppLogic
{
    public interface IClassAdministration
    {
        ClassesForList GetClassesForList(ClassFilter filter, bool search);

        ClassDetail GetClassDetailById(long classId);

        ClassForEdit GetNewClassForEdit(long profileId, ClassCategoryEnum classCategoryId);

        ClassForEdit GetClassForEditById(long classId);

        ClassForEdit GetFormClassForEdit(ClassForm form, ClassValidationResult classValidationResult);

        ClassValidationResult ValidateClassForm(ClassForm form);

        long SaveClass(ClassForm form, EnvironmentTypeEnum? env);

        void DeleteClass(long classId);
    }
}
