namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// Payment result
    /// </summary>
    public class PaymentResult
    {
        // public properties
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public long? ClassId { get; set; }
    }
}