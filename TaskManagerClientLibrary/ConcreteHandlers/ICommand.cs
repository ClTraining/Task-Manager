namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public interface ICommand
    {
        string Name { get; set; }
        void Execute(object argument);
        object Convert(object input);
    }
}
