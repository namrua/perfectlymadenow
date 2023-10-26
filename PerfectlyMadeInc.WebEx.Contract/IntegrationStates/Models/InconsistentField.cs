using System.ComponentModel;

namespace PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models
{
    /// <summary>
    /// Encapsulates WebEx inconsistent field
    /// </summary>
    public class InconsistentField
    {

        [DisplayName("Field name")]
        public string FieldName { get; set; }

        [DisplayName("Value in the system")]
        public string SystemValue { get; set; }

        [DisplayName("Value in the WebEx")]
        public string WebExValue { get; set; }

        // constructor
        public InconsistentField() { }
        public InconsistentField(string fieldName, string systemValue, string webExValue)
        {
            FieldName = fieldName;
            SystemValue = systemValue;
            WebExValue = webExValue;
        }

    }
}
