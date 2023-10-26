using System.Data.Common;

namespace AutomationSystem.Shared.Model
{
    public partial class CoreEntities
    {
        public CoreEntities(string nameOrConnectionString) : base(nameOrConnectionString) { }
    }
}
