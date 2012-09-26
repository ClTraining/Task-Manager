using System;
using System.ServiceModel;
using EntitiesLibrary;
using TaskManagerService.WCFServer;

namespace TaskConsoleClient.UI
{
    static class Program
    {
        static void Main()
        {
            const string serviceAddress = "net.tcp://localhost:44444";
            using (var factory = new ChannelFactory<ITaskManagerService>(new NetTcpBinding(), serviceAddress))
            {
                var client = factory.CreateChannel();
                var taskResult = client.AddTask(new ContractTask());
                Console.WriteLine(taskResult.Id);
            }
        }
    }
}
