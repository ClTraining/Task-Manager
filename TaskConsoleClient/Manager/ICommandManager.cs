#region Using



#endregion

using TaskConsoleClient.Entities;

namespace TaskConsoleClient.Manager
{
    public interface ICommandManager
    {
        TaskContract AddTask(TaskContract task);
    }
}
