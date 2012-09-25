using TaskConsoleClient.Entities;

namespace TaskConsoleClient.WCFClient
{
    public interface ITaskManagerApplication
    {
        TaskContract AddTask(TaskContract task);
    }
}
