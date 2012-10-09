namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public interface ICommand
    {
        string Name { get; set; }
        void Execute(object argument);
    }
}
