namespace AutomationSystem.Shared.Core.AsyncRequests.System
{
    /// <summary>
    /// Factory for async request executor manager
    /// </summary>
    public interface IAsyncRequestExecutorManagerFactory
    {
        IAsyncRequestExecutorManager CreateAsyncRequestExecutorManager();
    }
}
