using System;
using System.ServiceModel;
using TaskManagerService.WCFService;

namespace TaskConsoleClient.Manager
{
    public class NetTcpConnection : IConnection
    {
        private readonly ChannelFactory<ITaskManagerService> factory;

        public NetTcpConnection()
        {
            factory = new ChannelFactory<ITaskManagerService>("tcpEndPoint");
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
                Console.Write("Wrong server address.");
                Console.ReadLine();
            }
            return test;
        }
    }
}