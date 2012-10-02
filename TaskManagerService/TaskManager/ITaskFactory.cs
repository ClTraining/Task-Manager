using EntitiesLibrary;

namespace TaskManagerHost.TaskManager
{
    public interface ITaskFactory
    {
        ServiceTask Create();
    }
}
