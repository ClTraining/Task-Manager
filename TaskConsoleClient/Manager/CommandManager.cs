using System;
using System.ServiceModel;
using EntitiesLibrary;
using TaskConsoleClient.UI;
using TaskManagerHost.WCFServer;

namespace TaskConsoleClient.Manager
{
    class CommandManager: ICommandManager
    {
        private readonly TaskManagerClient client;
        public CommandManager()
        {
            client = new TaskManagerClient();
        }

        public ContractTask AddTask(ContractTask task)
        {
            return client.AddTask(task);
        }
    }

    class TaskManagerClient
    {
        private ITaskManagerService client;

        public ContractTask AddTask(ContractTask task)
        {
            using (var factory = new ChannelFactory<ITaskManagerService>(new NetTcpBinding(), "net.tcp://localhost:44444"))
            {
                client = factory.CreateChannel();
                var res = client.AddTask(task);
                Console.WriteLine(res);
                return res;
            }
        }
    }
}