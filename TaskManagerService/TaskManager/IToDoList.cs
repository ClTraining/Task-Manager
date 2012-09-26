using EntitiesLibrary;

namespace TaskManagerHost.TaskManager
{
    public interface IToDoList
    {
        ServiceTask AddTask(ITask task);
    }
}