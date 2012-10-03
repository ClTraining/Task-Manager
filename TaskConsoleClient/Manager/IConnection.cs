using TaskManagerHost.WCFService;
using TaskManagerHost.WCFService;

namespace TaskConsoleClient.Manager
{
    public interface IConnection
    {
        ITaskManagerService GetClient();
    }
}