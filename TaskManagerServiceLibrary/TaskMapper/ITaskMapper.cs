using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;

namespace TaskManagerServiceLibrary.TaskMapper
{
    public interface ITaskMapper
    {
        ClientTask ConvertToClient(ServiceTask task);
        ServiceTask ConvertArgsToTask(AddTaskArgs args);
    }
}