using System;
using System.ServiceModel;
using EntitiesLibrary;
using TaskConsoleClient.UI;
using TaskManagerHost.WCFServer;

namespace TaskConsoleClient.Manager
{
    class CommandManager: ICommandManager
    {
        public void Run()
        {
            using (var factory = new ChannelFactory<ITaskManagerService>(new NetTcpBinding(), "net.tcp://localhost:44444"))
            {
                var client = factory.CreateChannel();
                var res = client.AddTask(new ConsoleHelper().Parse(Console.ReadLine()));
                Console.WriteLine(res.Id);
            }
        }

        public ContractTask AddTask(ContractTask task)
        {
            return null;// client.AddTask(task);
        }
    }

    class TaskManagerClient
    {
        private ITaskManagerService client;

        public ContractTask AddTask(ContractTask task)
        {
            return null;
        }
    }
}