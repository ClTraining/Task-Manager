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
                var task = new ContractTask {Id = 0, Name = "afdsfds"};
                var client = factory.CreateChannel();
                var taskResult = client.AddTask(task);
                Console.WriteLine(taskResult.Id);
            }
        }
    }
}
