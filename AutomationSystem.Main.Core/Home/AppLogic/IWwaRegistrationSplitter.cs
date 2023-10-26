using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Core.Home.AppLogic
{
    public interface IWwaRegistrationSplitter
    {
        List<ClassPublicDetail> SplitWwaClasses(List<ClassPublicDetail> classes);

        List<RegistrationTypeListItem> FilterRegistrationTypes(List<RegistrationTypeListItem> types, bool? forWwa);

        bool? ResolveForWwa(bool? forWwa, RegistrationTypeEnum? backFromWwa, ClassCategoryEnum classCategoryId, bool isWwaAllowed);
    }
}
