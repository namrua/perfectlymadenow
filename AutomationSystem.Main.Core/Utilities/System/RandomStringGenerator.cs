using System;

namespace AutomationSystem.Main.Core.Utilities.System
{
    /// <summary>
    /// Generates random strings for misc purposes
    /// </summary>
    public static class RandomStringGenerator
    {
        // generates random request code for URLs
        // Warning: size of the code is limited by 36 characters
        public static string GenerateRequestCode()
        {
            var result = Guid.NewGuid().ToString();
            return result;
        }

    }

}
