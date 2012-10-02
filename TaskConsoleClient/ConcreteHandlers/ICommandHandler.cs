namespace TaskConsoleClient.ConcreteHandlers
{
    public interface ICommandHandler
    {
        bool Matches(string input);
        void Execute();
    }
}
