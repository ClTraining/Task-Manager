namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public interface ICommandHandler
    {
        string Name { get; set; }
        void Execute(object argument);
        object Convert(object input);
    }
}
