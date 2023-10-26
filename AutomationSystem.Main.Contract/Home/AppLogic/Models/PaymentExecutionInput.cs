using System.ComponentModel.DataAnnotations;

namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// Encapsulates PayPal transaction input for payment execution
    /// </summary>
    public class PaymentExecutionInput
    {
        public long RegistrationId { get; set; }

        [Required] public string Nonce { get; set; }
        [Required] public string Type { get; set; }

        public string DetailsJson { get; set; }
    }
}