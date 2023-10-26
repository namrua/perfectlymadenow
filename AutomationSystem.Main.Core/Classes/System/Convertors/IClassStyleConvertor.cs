using AutomationSystem.Main.Contract.Classes.AppLogic.Models.StyleAndBehavior;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.System.Convertors
{
    /// <summary>
    /// Converts class style objects
    /// </summary>
    public interface IClassStyleConvertor
    {
        // creates class business entity by class preference
        ClassStyle CreateClassStyleByClassPreference(ClassPreference classPreference, bool showClassBehaviorSettings);

        // initialize ClassStyleForEdit
        ClassStyleForEdit InitializeClassStyleForEdit(bool showClassBehaviorSettings);

        // converts ClassStyle to ClassStyleForm
        ClassStyleForm ConvertToClassStyleForm(ClassStyle classStyle, long classId);

        // converts ClassStyle to ClassStyleDetail
        ClassStyleDetail ConvertToClassStyleDetail(ClassStyle classStyle, bool showClassBehaviorSettings);

        // converts ClassStyleForm to ClassStyle
        ClassStyle ConverToClassStyle(ClassStyleForm form, long? headerPictureId);
    }
}
