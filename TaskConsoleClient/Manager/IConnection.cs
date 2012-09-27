using TaskManagerHost.WCFServer;

namespace TaskConsoleClient.Manager
{
    public interface IConnection
    {
        ITaskManagerService GetClient();
    }
}