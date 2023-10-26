using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.System.Convertors
{
    /// <summary>
    /// Class business detail convertor
    /// </summary>
    public interface IClassBusinessConvertor
    {
        // creates class business entity by class preference
        ClassBusiness CreateClassBusinessByClassPreference(Class cls, ClassPreference preference);

        // initializes ClassBusinessForEdit 
        ClassBusinessForEdit InitializeClassBusinessForEdit(Class cls);

        // converts ClassBusiness to ClassBusinessForm
        ClassBusinessForm ConvertToClassBusinessForm(ClassBusiness business, long classId);

        // converts ClassBusiness to ClassBusinessDetail
        ClassBusinessDetail ConvertToClassBusinesDetail(ClassBusiness business, Class cls);

        // converts ClassBusinessForm to ClassBusiness
        ClassBusiness ConvertToClassBusines(ClassBusinessForm form, Class cls);
    }
}
