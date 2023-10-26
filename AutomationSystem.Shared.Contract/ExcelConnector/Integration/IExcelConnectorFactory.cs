namespace AutomationSystem.Shared.Contract.ExcelConnector.Integration
{
    public interface IExcelConnectorFactory
    {
        IExcelConnector CreateExcelConnector();
    }
}
