using System.ComponentModel;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    /// <summary>
    /// Encapsulates email template parameter item
    /// </summary>
    public class EmailTemplateParameterDetail
    {

        [DisplayName("Id")]
        public long EmailParameterId { get; set; }

        [DisplayName("IsRequired")]
        public bool IsRequired { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

    }

}
