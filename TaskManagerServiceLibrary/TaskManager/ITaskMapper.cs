using EntitiesLibrary;

namespace TaskManagerServiceLibrary.TaskManager
{
    public interface ITaskMapper
    {
        ClientTask ConvertToClient(ServiceTask task);
    }
}