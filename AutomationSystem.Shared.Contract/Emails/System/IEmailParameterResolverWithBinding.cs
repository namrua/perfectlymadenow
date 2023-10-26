namespace AutomationSystem.Shared.Contract.Emails.System
{
    public interface IEmailParameterResolverWithBinding<in T> : IEmailParameterResolver
    {
        void Bind(T data);
    }
}
