using System;
using System.ServiceModel;
using TaskManagerHost.WCFServer;

namespace TaskConsoleClient.Manager
{
    public class NetTcpConnection : IConnection
    {
        private readonly ChannelFactory<ITaskManagerService> factory;

        public NetTcpConnection(string address)
        {
            factory = new ChannelFactory<ITaskManagerService>
                                        (new NetTcpBinding(), string.Format("net.tcp://{0}:44444", address));
        }

        public ITaskManagerService GetClient()
        {
            return factory.CreateChannel();
        }

        public bool TestConnection()
        {
            var test = false;
            try
            {
                test = factory.CreateChannel().TestConnection();
            }
            catch (EndpointNotFoundException)
            {
                Console.Write("Wrong address. Press Enter and try else.");
                Console.ReadLine();
            }

            return test;
        }
    }
}