using EntitiesLibrary;

namespace TaskManagerServiceLibrary.TaskManager
{
    public interface ITaskFactory
    {
        ServiceTask Create();
    }
}