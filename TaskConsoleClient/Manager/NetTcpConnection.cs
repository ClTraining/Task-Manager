using System.ServiceModel;
using TaskManagerHost.WCFServer;

namespace TaskConsoleClient.Manager
{
    public class NetTcpConnection : IConnection
    {
        public ITaskManagerService GetClient()
        {
            return new ChannelFactory<ITaskManagerService>
                                        (new NetTcpBinding(), "net.tcp://localhost:44444")
                                        .CreateChannel();
        }
    }
}