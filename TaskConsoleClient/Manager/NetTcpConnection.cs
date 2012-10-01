using System;
using System.ServiceModel;
using TaskManagerHost.WCFServer;

namespace TaskConsoleClient.Manager
{
    public class NetTcpConnection : IConnection
    {
        private readonly ChannelFactory<ITaskManagerService> factory;

        public NetTcpConnection()
        {
            factory = new ChannelFactory<ITaskManagerService>(new NetTcpBinding(), "net.tcp://localhost:44444");
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
                factory.Open();
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