using System.Web.Security;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic
{
    public class MaterialPasswordGenerator : IMaterialPasswordGenerator
    {
        public string GeneratePassword()
        {
            var result = Membership.GeneratePassword(8, 0);
            return result;
        }
    }
}
