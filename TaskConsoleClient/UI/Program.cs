using System;
using System.ServiceModel;
using TaskManagerHost.WCFServer;

namespace TaskConsoleClient.UI
{
    static class Program
    {
        static void Main()
        {
            var task = new ConsoleHelper().Parse(Console.ReadLine());
            var factory = new ChannelFactory<ITaskManagerService>(new NetTcpBinding(), "net.tcp://localhost:44444");
            //var client = new TaskManagerService(new ToDoList(new TaskFactory(), new MemoRepository()));
            var client = factory.CreateChannel();
            var result = client.AddTask(task);
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
