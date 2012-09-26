using EntitiesLibrary;


namespace TaskManagerService.TaskManager
{
    public interface IToDoList
    {
        ITask AddTask(ITask task);
    }
}