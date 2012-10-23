namespace TaskManagerClientLibrary.ConcreteHandlers.TaskFormatter
{
    public interface ITaskFormatterFactory
    {
        ITaskFormatter GetListFormatter();
        ITaskFormatter GetSingleFormatter();
    }
}