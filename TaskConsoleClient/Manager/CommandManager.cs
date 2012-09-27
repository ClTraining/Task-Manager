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
            return client.AddTask(task);
        }

        public void Run()
        {
            using (var factory = new ChannelFactory<ITaskManagerService>(new NetTcpBinding(), "net.tcp://localhost:44444"))
            {
                client = factory.CreateChannel();
                var res = AddTask(new ConsoleHelper().Parse(Console.ReadLine()));
                Console.WriteLine(res);
            }
        }
    }

    class TaskManagerClient : ITaskManagerService
    {
        public ContractTask AddTask(ContractTask task)
        {
            return null;
        }
    }
}