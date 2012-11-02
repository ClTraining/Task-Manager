using EntitiesLibrary;

namespace TaskManagerServiceLibrary.TaskMapper
{
    public interface ITaskMapper
    {
        ClientTask ConvertToClient(ServiceTask task);
    }
}