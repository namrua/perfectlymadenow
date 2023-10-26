namespace AutomationSystem.Main.Core.Reports.System.Models.FinancialBusiness
{
    /// <summary>
    /// Encapsulates Rate + Quantity item (e.g. Program Revenue Detail item)
    /// </summary>
    public class QuantityAndRateItem<TType>
    {
        public TType Type { get; set; }
        public decimal Rate { get; set; }
        public int Quantity { get; set; }
    }
}
