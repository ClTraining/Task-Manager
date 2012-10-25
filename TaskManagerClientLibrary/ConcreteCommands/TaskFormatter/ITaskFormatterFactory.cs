namespace TaskManagerClientLibrary.ConcreteCommands.TaskFormatter
{
    public interface ITaskFormatterFactory
    {
        ITaskFormatter GetListFormatter();
        ITaskFormatter GetSingleFormatter();
    }
}