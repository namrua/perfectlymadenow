using System.Collections.Generic;
using AutomationSystem.Main.Web.Models;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Web.Helpers
{
    /// <summary>
    /// Provides layout helping method
    /// </summary>
    public interface ILayoutHelper
    {

        // gets language selector model
        LanguageSelectorModel GetLanguageSelectorModel();

        // gets main menu items
        List<HierarchicalMenuItem> GetMainMenuItems(string activePageId);

        // gets tab menu items
        List<HierarchicalMenuItem> GetTabMenuItems(string tabId, string activeTabId, object parameters = null);
        
    }

}