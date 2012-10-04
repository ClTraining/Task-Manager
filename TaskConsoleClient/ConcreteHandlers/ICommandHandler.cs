namespace TaskManagerConsole.UI.ConcreteHandlers
{
    public interface ICommandHandler
    {
        bool Matches(string input);
        void Execute(string input);
    }
}
