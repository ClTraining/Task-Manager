using EntitiesLibrary;

namespace TaskManagerApp.TaskManager
{
    public interface IToDoList
    {
        ITask AddTask(ITask task);
    }
}