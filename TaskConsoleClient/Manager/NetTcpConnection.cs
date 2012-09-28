using System.ServiceModel;
using TaskManagerHost.WCFServer;

namespace TaskConsoleClient.Manager
{
    public class NetTcpConnection : IConnection
    {
        public ITaskManagerService GetClient()
        {
            return new ChannelFactory<ITaskManagerService>
                                        (new NetTcpBinding(), "net.tcp://192.168.22.124:44444")
                                        .CreateChannel();
        }
    }
}