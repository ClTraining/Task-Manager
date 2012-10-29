using EntitiesLibrary;

namespace TaskManagerServiceLibrary.TaskManager
{
    public interface ITaskMapper
    {
        ClientTask ConvertToContract(ServiceTask task);
    }
}