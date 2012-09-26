using EntitiesLibrary;

namespace TaskManagerHost.TaskManager
{
    public interface IToDoList
    {
        ITask AddTask(ITask task);
    }
}