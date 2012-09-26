using System;
using System.ServiceModel;
using TaskManagerHost.WCFServer;

namespace TaskConsoleClient.UI
{
    static class Program
    {
        static void Main()
        {
            var factory = new ChannelFactory<ITaskManagerService>(new NetTcpBinding(), "net.tcp://localhost:44444");
            while (true)
            {
                var task = new ConsoleHelper().Parse(Console.ReadLine());
                var client = factory.CreateChannel();
                var res = client.AddTask(task);
                Console.WriteLine(res);
            }
        }
    }
}
