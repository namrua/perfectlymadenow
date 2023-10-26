using AutomationSystem.Shared.Contract.ExcelConnector.Integration;

namespace AutomationSystem.Shared.Core.ExcelConnector.Integration
{
    public class ExcelConnectorFactory : IExcelConnectorFactory
    {
        public IExcelConnector CreateExcelConnector()
        {
            return new ExcelConnector();
        }
    }
}
