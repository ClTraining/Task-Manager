using CQRS.ClientSpecifications;

namespace TaskManagerClientLibrary.ConcreteCommands.TaskFormatter
{
    public interface ITaskFormatterFactory
    {
        ITaskFormatter GetFormatter(IClientSpecification specification);
    }
}