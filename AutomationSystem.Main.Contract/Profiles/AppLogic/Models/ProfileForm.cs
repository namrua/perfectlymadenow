using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AutomationSystem.Main.Contract.Profiles.AppLogic.Models
{
    /// <summary>
    /// Profile form
    /// </summary>
    public class ProfileForm
    {

        [HiddenInput]
        public long ProfileId { get; set; }

        [HiddenInput]
        public string OriginMoniker { get; set; }

        [DisplayName("Name")]
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [DisplayName("Moniker")]
        [Required]
        [MaxLength(16)]
        [RegularExpression(@"^[a-zA-Z]*$")]
        public string Moniker { get; set; }


        // todo: move to ProfileForEdit when it will exist
        public string ForbiddenMoniker { get; set; }

    }


}
