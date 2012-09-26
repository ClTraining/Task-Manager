using EntitiesLibrary;

namespace TaskManagerService.TaskManager
{
    public interface ITaskFactory
    {
        ServiceTask Create(ITask task);
    }
}
