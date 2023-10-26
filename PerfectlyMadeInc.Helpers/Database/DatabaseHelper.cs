
namespace PerfectlyMadeInc.Helpers.Database
{
    /// <summary>
    /// Encapsulates macros for processing database inputs
    /// </summary>
    public static class DatabaseHelper
    {

        // Trims and adjusts NVarchar field     
        public static string TrimNVarchar(string input, bool isRequired, int? maxLength)
        {
            if (input == null)
            {
                return isRequired ? null : "";
            }
            if (maxLength.HasValue && input.Length > maxLength)
                return input.Substring(0, maxLength.Value);
            return input;
        }

    }

}
