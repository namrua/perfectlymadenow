namespace AutomationSystem.Console.ConsoleCommands
{
    public interface IConsoleCommand<in T>
    {
        void Execute(T commandParameters);
    }
}
