using EntitiesLibrary.CommandArguments;

namespace TaskManagerClientLibrary.ConcreteCommands.TaskFormatter
{
    public interface ITaskFormatterFactory
    {
        ITaskFormatter GetFormatter(IListCommandArguments specification);
    }
}