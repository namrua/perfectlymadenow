using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Home.AppLogic
{
    /// <summary>
    /// Determines whether public payment is allowed for class
    /// </summary>
    public interface IPublicPaymentResolver
    {
        bool IsPublicPaymentAllowedForClass(Class cls);
    }
}
