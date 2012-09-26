using EntitiesLibrary;

namespace TaskManagerService.WCFServer
{
    public interface ITaskManagerApplication
    {
        ITask AddTask(ITask task);
    }
}
