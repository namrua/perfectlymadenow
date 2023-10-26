namespace AutomationSystem.Main.Web.Helpers
{
    /// <summary>
    /// Provides services for Views
    /// </summary>
    public static class ViewServices
    {
        // gets layout helper
        public static ILayoutHelper LayoutHelper => IocProvider.Get<ILayoutHelper>();
    }

}