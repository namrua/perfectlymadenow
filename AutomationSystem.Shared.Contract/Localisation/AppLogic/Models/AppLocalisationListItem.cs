using System.ComponentModel;

namespace AutomationSystem.Shared.Contract.Localisation.AppLogic.Models
{
    /// <summary>
    /// Application localisation list item
    /// </summary>
    public class AppLocalisationListItem
    {

        // public properties
        public long OriginAppLocalisationId { get; set; }       
        public long Index { get; set; }                     // order index

        [DisplayName("Module")]
        public string Module { get; set; }

        [DisplayName("Label")]
        public string Label { get; set; }

        [DisplayName("Label")]
        public string Value { get; set; }

        // properties
        public bool IsEmpty => string.IsNullOrEmpty(Value);

    }

}
