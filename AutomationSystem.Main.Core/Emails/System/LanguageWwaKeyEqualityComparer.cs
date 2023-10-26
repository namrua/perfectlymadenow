using System.Collections.Generic;
using AutomationSystem.Main.Core.Emails.System.Models;

namespace AutomationSystem.Main.Core.Emails.System
{
    /// <summary>
    /// LanguageWwaKey equality comparer
    /// </summary>
    internal class LanguageWwaKeyEqualityComparer : IEqualityComparer<LanguageWwaKey>
    {

        public bool Equals(LanguageWwaKey x, LanguageWwaKey y)
        {
            return x?.LanguageId == y?.LanguageId && x?.IsWwa == y?.IsWwa;
        }

        public int GetHashCode(LanguageWwaKey key)
        {
            var combined = key.LanguageId + "|" + key.IsWwa;
            return (combined.GetHashCode());
        }

    }
}