using System;
using System.Collections.Generic;
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

        public ContractTask GetTaskById(int id)
        {
            return null;
        }

        public ContractTask Edit(ContractTask task)
        {
            throw new NotImplementedException();
        }

        public List<ContractTask> GetAllTasks()
        {
            throw new NotImplementedException();
        }
    }

    class TaskManagerClient
    {
        public ContractTask AddTask(ContractTask task)
        {
            using (var factory = new ChannelFactory<ITaskManagerService>(new NetTcpBinding(), "net.tcp://localhost:44444"))
            {
                var client = factory.CreateChannel();
                var res = client.AddTask(task);
                Console.WriteLine(res.Id);
                return res;
            }
        }
    }
}