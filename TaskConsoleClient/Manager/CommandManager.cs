using System;
using System.ServiceModel;
using EntitiesLibrary;
using TaskConsoleClient.UI;
using TaskManagerHost.WCFServer;

namespace TaskConsoleClient.Manager
{
    class CommandManager: ICommandManager
    {
        private ITaskManagerService client;

        public CommandManager()
        {
        }

        public ContractTask AddTask(ContractTask task)
        {
            var res = client.AddTask(task);
            Console.WriteLine("Task ID = {0}, {1}", res.Id, res.Name);
            return null;
        }

        public void Run()
        {
            using (var factory = new ChannelFactory<ITaskManagerService>(new NetTcpBinding(), "net.tcp://localhost:44444"))
            {
                client = factory.CreateChannel();
                AddTask(new ConsoleHelper().Parse(Console.ReadLine()));
            }
        }
    }
}
