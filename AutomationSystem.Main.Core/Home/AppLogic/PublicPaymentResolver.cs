using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Home.AppLogic
{
    /// <summary>
    /// Determines whether public payment is allowed for class
    /// </summary>
    public class PublicPaymentResolver : IPublicPaymentResolver
    {
        public bool IsPublicPaymentAllowedForClass(Class cls)
        {
            return cls.PayPalKeyId.HasValue;
        }
    }
}
