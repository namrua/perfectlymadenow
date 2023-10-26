using System.IO;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.StyleAndBehavior;

namespace AutomationSystem.Main.Contract.Classes.AppLogic
{
    public interface IClassStyleAdministration
    {
        ClassStylePageModel GetClassStylePageModel(long classId);

        ClassStyleForEdit GetClassStyleForEditByClassId(long classId);

        ClassStyleForEdit GetClassStyleForEditByForm(ClassStyleForm form);

        void SaveClassStyle(ClassStyleForm form, Stream headerPictureContent, string headerPictureName);
    }
}
