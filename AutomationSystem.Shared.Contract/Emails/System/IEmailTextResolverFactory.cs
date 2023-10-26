namespace AutomationSystem.Shared.Contract.Emails.System
{
    public interface IEmailTextResolverFactory
    {
        IEmailTextResolver CreateEmailTextResolver(params IEmailParameterResolver[] resolvers);
    }
}
