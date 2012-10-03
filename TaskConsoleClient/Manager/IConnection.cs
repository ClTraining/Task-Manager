using TaskManagerHost.WCFService;

namespace TaskConsoleClient.Manager
{
    public interface IConnection
    {
        ITaskManagerService GetClient();
    }
}