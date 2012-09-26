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
                AddTask(new ConsoleHelper().Parse(Console.ReadLine()));
            }
        }
    }
}