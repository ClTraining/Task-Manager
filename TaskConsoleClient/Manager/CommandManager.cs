using System.Collections.Generic;
using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using NSubstitute;
using Xunit;

namespace TaskConsoleClient.Manager
{
    class CommandManager : ICommandManager
    {
        private readonly IConnection conn;

        public CommandManager(IConnection conn)
        {
            this.conn = conn;
        }

        public ContractTask AddTask(ContractTask task)
        public List<ContractTask> GetAllTasks()
        {
            return conn.GetClient().AddTask(task); ;
        }

        public ContractTask GetTaskById(int id)
        {
            using (var factory = new ChannelFactory<ITaskManagerService>(new NetTcpBinding(), "net.tcp://localhost:44444"))
        {
                client = factory.CreateChannel();
                AddTask(new ConsoleHelper().Parse(Console.ReadLine()));
            }
        }
    }
}
=======
п»їusing System;
using System.Collections.Generic;
using EntitiesLibrary;
using TaskConsoleClient.UI;

namespace TaskConsoleClient.Manager
{
    class CommandManager : ICommandManager
    {

        public ContractTask Edit(ContractTask task)
        {
            return conn.GetClient().Edit(task);
        }

        public List<ContractTask> GetAllTasks()
        {
            return conn.GetClient().GetAllTasks();
        }
    }

    public class CommandManagerTests
    {
        readonly ContractTask task = new ContractTask();

        public List<ContractTask> GetAllTasks()
        public void should_send_add_task_to_service()
        {
            throw new NotImplementedException();
        }

    }
}

